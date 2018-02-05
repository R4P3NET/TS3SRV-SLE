using System;
using System.Collections.Generic;
using System.Text;

using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class OutgoingPayloadHandler : CorePayload
    {
        public OutgoingPayloadHandler(byte[] RawData, PayloadTypes PType)
        {
            Payload = RawData;
            PayloadType = PType;
            Header = new byte[Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN];
        }

        public void ConstructHeader()
        {
            Header[0] = 0x01; //Static

        }
    }
}
