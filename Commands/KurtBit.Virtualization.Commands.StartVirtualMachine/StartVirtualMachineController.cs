namespace KurtBit.Virtualization.Commands.StartVirtualMachine
{
    using KurtBit.Virtualization.Core;
    using KurtBit.Virtualization.Core.Attributes;
    using System.Management;
    using System.Web.Http;

    public class StartVirtualMachineController: ApiController
    {
        [NonAction]
        public void StartVirtualMachineWMI(string guid)
        {
            using (var searcher = new ManagementObjectSearcher(
               "root/virtualization/v2",
               $"SELECT * from Msvm_ComputerSystem WHERE Name='{guid}'"))
            {
                var instance = searcher.Get().FirstOrDefault();

                ManagementBaseObject inParams = instance.GetMethodParameters("RequestStateChange");

                inParams["RequestedState"] = 2;

                ManagementBaseObject outParams = instance.InvokeMethod(
                    "RequestStateChange",
                    inParams,
                    null);
            }
        }

        [HttpPost()]
        [Route("api/v1/virtualmachines/{guid}/start")]
        [SwaggerOpperation("virtualmachines")]
        public IHttpActionResult StartVirtualMachine(string guid)
        {
            this.StartVirtualMachineWMI(guid);

            return Ok();
        }
    }
}
