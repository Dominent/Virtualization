namespace KurtBit.Virtualization.Core.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerOpperationAttribute : Attribute
    {
        public SwaggerOpperationAttribute(string operationId)
        {
            this.OperationId = operationId;
        }

        public string OperationId { get; private set; }
    }
}
