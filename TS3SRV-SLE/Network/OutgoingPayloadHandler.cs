using System;
using System.Collections.Generic;
using System.Text;

using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class OutgoingPayloadHandler : CorePayload
    {

        public ServerProperties ServerProperties { get; set; }

        public OutgoingPayloadHandler()
        {
            Header = new byte[Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN];
        }

        public OutgoingPayloadHandler(ServerProperties serverProperties)
        {
            Header = new byte[Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN];
            PayloadType = PayloadTypes.Dataupload;
            CanCreateChannel = serverProperties.CanCreateChannels;
            IsPasswordProtected = serverProperties.IsPasswordProtected;
            ServerProperties = serverProperties;
        }

        public OutgoingPayloadHandler(byte[] rawData, PayloadTypes pType)
        {
            Header = new byte[Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN];
            Payload = rawData;
            PayloadType = pType;
        }

        public void ConstructHeader()
        {
            Header[0] = 0x01; //Static
            Header[1] = unchecked((byte)(SequenceId >> 8));
            Header[2] = unchecked((byte)(SequenceId >> 0));
            Header[3] = PType;
        }

        public bool ConstructDataPacket()
        {
            if (ServerProperties == null)
                return false;
            var raw = new byte[500];
            var index = 0;
            Array.Copy(Header, 0, raw, index, Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN);
            index += Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN;
            Array.Copy(AuthKey, 0, raw, index, Properties.TS3SRV_WEBLIST_PROTOCOL_AUTHKEYLEN);
            index += Properties.TS3SRV_WEBLIST_PROTOCOL_AUTHKEYLEN;
            var portBytes = BitConverter.GetBytes(ServerProperties.Port);
            Array.Copy(portBytes, 0, raw, index, Properties.TS3SRV_WEBLIST_PROTOCOL_PORTLEN);
            index += Properties.TS3SRV_WEBLIST_PROTOCOL_PORTLEN;
            var slotsBytes = BitConverter.GetBytes(ServerProperties.Slots);
            Array.Copy(slotsBytes, 0, raw, index, Properties.TS3SRV_WEBLIST_PROTOCOL_SLOTSCLIENTSLEN);
            index += Properties.TS3SRV_WEBLIST_PROTOCOL_SLOTSCLIENTSLEN;
            var clientsBytes = BitConverter.GetBytes(ServerProperties.Clients);
            Array.Copy(clientsBytes, 0, raw, index, Properties.TS3SRV_WEBLIST_PROTOCOL_SLOTSCLIENTSLEN);
            index += Properties.TS3SRV_WEBLIST_PROTOCOL_SLOTSCLIENTSLEN;
            raw[index++] = Flags;
            var nameBytes = Encoding.UTF8.GetBytes(ServerProperties.Name);
            raw[index++] = BitConverter.GetBytes(nameBytes.Length)[0];
            Array.Copy(nameBytes, 0, raw, index, nameBytes.Length);
            index += nameBytes.Length;
            RawPayload = new byte[index + 1];
            Array.Copy(raw, 0, RawPayload, 0, index + 1);
            return true;
        }
    }
}
