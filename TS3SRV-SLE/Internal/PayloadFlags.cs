using System;

namespace TS3SRV_SLE.Internal
{
    [Flags]
    public enum PayloadFlags : byte
    {
        Undefined = 0x00,
        CanCreateChannel = 0x02,
        IsPasswordProtected = 0x01,
    }
}
