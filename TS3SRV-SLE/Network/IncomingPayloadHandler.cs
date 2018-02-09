using System;

using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class IncomingPayloadHandler : CorePayload
    {
        public IncomingPayloadHandler(byte[] rawData)
        {
            RawPayload = rawData;
        }
    }
}
