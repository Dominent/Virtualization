namespace KurtBit.Virtualization.Server.Swagger
{
    using KurtBit.Virtualization.Core;
    using KurtBit.Virtualization.Core.Attributes;
    using Swashbuckle.Swagger;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Description;

    public class SwaggerOperationNameFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var opperationAttributeName = apiDescription.ActionDescriptor
               .GetCustomAttributes<SwaggerOpperationAttribute>()
               .Select(a => a.OperationId)
               .FirstOrDefault();

            operation.tags = new List<string>() { opperationAttributeName };
        }
    }
}
