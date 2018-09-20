namespace KurtBit.Virtualization.Core
{
    using System.Management;

    public static class ManagmentObjectExtensions
    {
        public static ManagementObject FirstOrDefault(this ManagementObjectCollection collection)
        {
            foreach (ManagementObject item in collection)
            {
                return item;
            }

            return null;
        }
    }
}
