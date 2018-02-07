using System;
using System.Collections.Generic;
using System.Text;

using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class OutgoingPayloadHandler : CorePayload
    {
        public OutgoingPayloadHandler()
        {
            Header = new byte[Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN];
        }

        public OutgoingPayloadHandler(byte[] RawData, PayloadTypes PType)
        {
            Header = new byte[Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN];
            Payload = RawData;
            PayloadType = PType;
        }

        public void ConstructHeader()
        {
            Header[0] = 0x01; //Static
            Header[1] = unchecked((byte)(SequenceId >> 8));
            Header[2] = unchecked((byte)(SequenceId >> 0));
            Header[3] = PType;
        }
    }
}
