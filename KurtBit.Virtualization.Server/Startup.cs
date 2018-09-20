namespace KurtBit.Virtualization.Server
{
    using KurtBit.Virtualization.Server.Swagger;
    using Owin;
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using System.Web.Http.Dispatcher;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            var documentationRegistry = new List<string>();

            config.Services.Replace(
                typeof(IAssembliesResolver),
                new CommandAsemblyResolver(documentationRegistry));

            config.MapHttpAttributeRoutes();

            config.EnsureInitialized();

            SwaggerConfig.Register(config, documentationRegistry);

            app.UseWebApi(config);
        }
    }
}
