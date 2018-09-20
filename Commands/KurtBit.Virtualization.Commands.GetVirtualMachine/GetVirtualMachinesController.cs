namespace KurtBit.Virtualization.Commands.GetVirtualMachine
{
    using KurtBit.Virtualization.Core;
    using KurtBit.Virtualization.Core.Attributes;
    using System.Collections.Generic;
    using System.Management;
    using System.Web.Http;

    public class GetVirtualMachinesController : ApiController
    {
        [NonAction]
        public IEnumerable<VirtualMachine> GetVirtualMachinesWMI()
        {
            var virtualMachines = new List<VirtualMachine>();

            using (var searcher = new ManagementObjectSearcher(
                "root/virtualization/v2",
                "SELECT * from Msvm_ComputerSystem"))
            {
                foreach (var item in searcher.Get())
                {
                    virtualMachines.Add(VirtualMachine.FromWMIObject(item));
                }
            }

            return virtualMachines;
        }

        [Get("api/v1/virtualmachines")]
        [SwaggerOpperation("virtualmachines")]
        public IHttpActionResult GetVirtualMachines()
        {
            return Ok(this.GetVirtualMachinesWMI());
        }
    }
}
