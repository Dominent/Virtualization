namespace KurtBit.Virtualization.Server.Swagger
{
    using Swashbuckle.Application;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Http;
    using System.Xml.Linq;

    public class SwaggerConfig
    {
        public static void Register(
            HttpConfiguration config, IEnumerable<string> documentationRegistry)
        {
            CombineDocumentationRegistryFiles(documentationRegistry);

            config.EnableSwagger(cfg =>
            {
                cfg.SingleApiVersion("v1", "KurtBit.Virtualization.WebAPI");
                cfg.OperationFilter<SwaggerOperationNameFilter>();
                cfg.IncludeXmlComments(GetXMLDocumentationPath());
            }).EnableSwaggerUi();
        }

        private static void CombineDocumentationRegistryFiles(IEnumerable<string> documentationRegistry)
        {
            var documentationPath = GetXMLDocumentationPath();
            var documentation = XElement.Load(documentationPath);
            foreach (var document in documentationRegistry)
            {
                if (!File.Exists(document))
                {
                    continue;
                }

                var xml = XElement.Load(document);
                foreach (XElement element in xml.Descendants())
                {
                    documentation.Add(element);
                }
            }

            documentation.Save(documentationPath);
        }

        public static string GetXMLDocumentationPath()
        {
            return String.Format(@"{0}\KurtBit.Virtualization.Server.xml",
                AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
