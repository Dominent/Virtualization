namespace KurtBit.Virtualization.Commands.StopVirtualMachine
{
    using KurtBit.Virtualization.Core;
    using KurtBit.Virtualization.Core.Attributes;
    using System.Management;
    using System.Web.Http;

    public class StopVirtualMachineController : ApiController
    {
        [NonAction]
        public void StopVirtualMachineWMI(string guid)
        {
            using (var searcher = new ManagementObjectSearcher(
               "root/virtualization/v2",
               $"SELECT * from Msvm_ComputerSystem WHERE Name='{guid}'"))
            {
                var instance = searcher.Get().FirstOrDefault();

                ManagementBaseObject inParams = instance.GetMethodParameters("RequestStateChange");

                inParams["RequestedState"] = 3;

                ManagementBaseObject outParams = instance.InvokeMethod(
                    "RequestStateChange",
                    inParams,
                    null);
            }
        }

        /// <summary>
        /// Stops the desired virtual machine
        /// </summary>
        /// <param name="guid">The uinque identifier of the virtual machine</param>
        /// <returns></returns>
        [HttpPost()]
        [Route("api/v1/virtualmachines/{guid}/stop")]
        [SwaggerOpperation("virtualmachines")]
        public IHttpActionResult StopVirtualMachine(string guid)
        {
            this.StopVirtualMachineWMI(guid);

            return Ok();
        }
    }
}
