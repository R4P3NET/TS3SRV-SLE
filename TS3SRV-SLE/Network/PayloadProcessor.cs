using System;
using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class PayloadProcessor
    {
        private ConnectionHandler CHandler;

        private byte[] TS3SRV_WEBLIST_AUTHKEY;

        public PayloadProcessor()
        {
            CHandler = new ConnectionHandler()
            {
                HandleIncoming = OnIncoming,
                HandleOutgoing = OnOutgoing,
            };
        }

        private void OnIncoming(IncomingPayloadHandler Payload)
        {
            Payload = ProcessIncomingPayload(Payload);
        }

        private void OnOutgoing(OutgoingPayloadHandler Payload)
        {

        }


        #region "Begin LowLevel Payload Handling Routines"

        private IncomingPayloadHandler ProcessIncomingPayload(IncomingPayloadHandler Payload)
        {
            //Payload Contains Authkey, save it
            if (Payload.RawPayload.Length == (Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN + Properties.TS3SRV_WEBLIST_PROTOCOL_AUTHKEYLEN + 2) && TS3SRV_WEBLIST_AUTHKEY == null)
            {
                Array.Copy(Payload.RawPayload, Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN, Payload.AuthKey, 0, Payload.RawPayload.Length - Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN);
                TS3SRV_WEBLIST_AUTHKEY = Payload.AuthKey;
            }
            
            Array.Copy(Payload.RawPayload, 0, Payload.Header, 0, Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN);
            
            //Process Header
            if (Payload.Header.Length > Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN)
            {
                Payload.SequenceId = BitConverter.ToUInt16(new byte[2] { Payload.Header[1], Payload.Header[2] }, 0);
                Payload.PType = Payload.Header[3];
            }
            return Payload;
        }
        #endregion
    }
}
