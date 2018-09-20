namespace KurtBit.Virtualization.Commands.DestroyVirtualMachine
{
    using KurtBit.Virtualization.Core;
    using KurtBit.Virtualization.Core.Attributes;
    using System.Management;
    using System.Web.Http;

    public class DestroyVirtualMachineController : ApiController
    {
        public const string LOCAL_SERVER_NAME = ".";

        [NonAction]
        public void DestroyVirtualMachineWMI(string guid, string serverName)
        {
            const string VIRTUALIZATION_NAMESPACE = "root/virtualization/v2";
            const string VIRTUAL_SYSTEM_MANAGMENT_SERVICE = "Msvm_VirtualSystemManagementService";

            var scope = new ManagementScope($@"\\{serverName}\{VIRTUALIZATION_NAMESPACE}", null);

            using (var searcher = new ManagementObjectSearcher(
                "root/virtualization/v2",
                $"SELECT * from Msvm_ComputerSystem WHERE Name='{guid}'"))
            {
                var instance = searcher.Get().FirstOrDefault();

                using (var virtualSystemManagementService = new ManagementClass(VIRTUAL_SYSTEM_MANAGMENT_SERVICE))
                {
                    virtualSystemManagementService.Scope = new ManagementScope(VIRTUALIZATION_NAMESPACE);

                    ManagementBaseObject inParams = virtualSystemManagementService.GetMethodParameters("DestroySystem");

                    inParams["AffectedSystem"] = instance.Path;
                   
                    ManagementBaseObject outParams = virtualSystemManagementService
                       .GetInstances()
                       .FirstOrDefault()
                       .InvokeMethod(
                            "DestroySystem", inParams, null);

                    //if((uint)outParams["ReturnValue"] == 0)
                    //{
                    //    OK
                    //}
                }
            }
        }

        [HttpDelete]
        [Route("api/v1/virtualmachines/{guid}")]
        [SwaggerOpperation("virtualmachines")]
        public IHttpActionResult DestroyVirtualMachine(string guid)
        {
            this.DestroyVirtualMachineWMI(guid, LOCAL_SERVER_NAME);

            return Ok();
        }
    }
}
