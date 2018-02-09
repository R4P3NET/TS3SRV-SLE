namespace TS3SRV_SLE.Internal
{
    public enum ResponseStatus : byte
    {
        Ok = 0x00,
        BadData = 0x01,
        Busy = 0x05,
        Error = 0x07,
    }
}
