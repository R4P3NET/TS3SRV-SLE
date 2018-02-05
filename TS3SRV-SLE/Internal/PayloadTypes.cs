using System;

namespace TS3SRV_SLE.Internal
{
    public enum PayloadTypes : byte
    {
        Undefined = 0x00,
        Keyrequest = 0x01,
        Dataupload = 0x02,
    }
}
