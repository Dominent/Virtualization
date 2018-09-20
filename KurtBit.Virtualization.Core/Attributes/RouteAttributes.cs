namespace KurtBit.Virtualization.Core.Attributes
{
    using System.Net.Http;

    public class GetAttribute : MethodConstraintedRouteAttribute
    {
        public GetAttribute(string template) : base(template ?? "", HttpMethod.Get) { }
    }

    public class PostAttribute : MethodConstraintedRouteAttribute
    {
        public PostAttribute(string template) : base(template ?? "", HttpMethod.Post) { }
    }
}
