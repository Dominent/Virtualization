namespace KurtBit.Virtualization.Core
{
    public enum VirtualMachineStateType
    {
        Paused = 32768,
        Stopped = 3,
        Running = 2,
        Saving = 32773,
        Stopping = 32774,
        Snapshotting = 32771,
        Suspended = 32769,
        Starting = 32770
    }
}
