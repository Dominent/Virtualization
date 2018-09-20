namespace KurtBit.Virtualization.Commands.CreateVirtualMachine
{
    using KurtBit.Virtualization.Core;
    using KurtBit.Virtualization.Core.Attributes;
    using System;
    using System.Management;
    using System.Web.Http;

    public class CreateVirutalMachineController : ApiController
    {
        public const string LOCAL_SERVER_NAME = ".";

        [NonAction]
        public void CreateVirtualMachineWMI(string serverName, string vmName)
        {
            const string VIRTUALIZATION_NAMESPACE = "root/virtualization/v2";
            const string VIRTUAL_SYSTEM_SETTING_DATA = "Msvm_VirtualSystemSettingData";
            const string METHOD_NAME = "DefineSystem";

            using (var virtualSystemSettingClass = new ManagementClass(VIRTUAL_SYSTEM_SETTING_DATA))
            {
                var scope = new ManagementScope($@"\\{serverName}\{VIRTUALIZATION_NAMESPACE}", null);

                virtualSystemSettingClass.Scope = scope;

                using (var virtualSystemSetting = virtualSystemSettingClass.CreateInstance())
                {
                    virtualSystemSetting["ElementName"] = vmName;

                    string embeddedInstance = virtualSystemSetting.GetText(TextFormat.WmiDtd20);

                    using (var virtualSystemManagementService = new ManagementClass("Msvm_VirtualSystemManagementService"))
                    {
                        virtualSystemManagementService.Scope = new ManagementScope(VIRTUALIZATION_NAMESPACE);

                        using (var inParams =
                            virtualSystemManagementService.GetMethodParameters("DefineSystem"))
                        {
                            inParams["SystemSettings"] = embeddedInstance;

                            using (var outParams =
                                virtualSystemManagementService.GetInstances()
                                .FirstOrDefault()
                                .InvokeMethod(METHOD_NAME, inParams, null))
                            {
                                Console.WriteLine("ret={0}", outParams["ReturnValue"]);
                            };
                        }
                    }
                }
            }
        }

        [Post("api/v1/virtualmachines")]
        [SwaggerOpperation("virtualmachines")]
        public IHttpActionResult CreateVirtualMachine([FromBody]string virtualMachineName)
        {
            this.CreateVirtualMachineWMI(LOCAL_SERVER_NAME, virtualMachineName);

            return Ok();
        }
    }
}
