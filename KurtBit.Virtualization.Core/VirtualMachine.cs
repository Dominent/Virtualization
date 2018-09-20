namespace KurtBit.Virtualization.Core
{
    using System;
    using System.Management;
    using System.Text;

    public class VirtualMachine
    {
        public static VirtualMachine FromWMIObject(ManagementBaseObject wmiObject)
        {
            return new VirtualMachine()
            {
                Guid = wmiObject["Name"].ToString(),
                Name = wmiObject["ElementName"].ToString(),
                State = (VirtualMachineStateType)Convert.ToInt32(wmiObject["enabledState"])
            };
        }

        public string Guid { get; set; }

        public string Name { get; set; }

        public VirtualMachineStateType State { get; set; }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();

            strBuilder.Append(
                $"Name: {this.Name}, State: {Enum.GetName(typeof(VirtualMachineStateType), this.State)} ");

            return strBuilder.ToString();
        }
    }
}
