using System;
using NLog;
using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class PayloadProcessor
    {
        private ConnectionHandler CHandler;

        private static readonly Logger Logger = LogManager.GetLogger(Properties.TS3SRV_LOGGER_NAME);

        private byte[] TS3SRV_WEBLIST_AUTHKEY;
        private ushort TS3SRV_WEBLIST_SEQUENCEID;


        public PayloadProcessor()
        {
            CHandler = new ConnectionHandler() { HandleIncoming = OnIncoming };

            TS3SRV_WEBLIST_AUTHKEY = new byte[4];
            TS3SRV_WEBLIST_SEQUENCEID = 0;

        }

        private void OnIncoming(IncomingPayloadHandler Payload)
        {
            Payload = ProcessIncomingPayload(Payload);
            //TODO
        }


        #region "Begin LowLevel Payload Handling Routines"

        public void ProcessInitialKeyRequest()
        {
            OutgoingPayloadHandler Payload = new OutgoingPayloadHandler()
            {
                SequenceId = (ushort)(TS3SRV_WEBLIST_SEQUENCEID + 1),
                PayloadType = PayloadTypes.Keyrequest,
            };
            Payload.ConstructHeader();
            CHandler.SendPayload(Payload.Header);
        }


        private IncomingPayloadHandler ProcessIncomingPayload(IncomingPayloadHandler Payload)
        {
            //Payload Contains Authkey, save it
            if (Payload.RawPayload.Length == (Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN + Properties.TS3SRV_WEBLIST_PROTOCOL_AUTHKEYLEN))
            {
                Array.Copy(Payload.RawPayload, Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN, Payload.AuthKey, 0, Payload.RawPayload.Length - Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN);
                TS3SRV_WEBLIST_AUTHKEY = Payload.AuthKey;
                Logger.Log(LogLevel.Info, "Got Authkey, saving it");
            }
            
            Array.Copy(Payload.RawPayload, 0, Payload.Header, 0, Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN);
            
            //Ignore first header byte as it's not needed
            Payload.SequenceId = BitConverter.ToUInt16(new byte[2] { Payload.Header[1], Payload.Header[2] }, 0);
            Payload.PType = Payload.Header[3];

            TS3SRV_WEBLIST_SEQUENCEID = Payload.SequenceId;
            return Payload;
        }
        
        /*
        TODO
        private OutgoingPayloadHandler ProcessOutgoingPayload(byte[] Data, PayloadTypes PType, params PayloadFlags[] Flags)
        {

        }
        */

        #endregion
    }
}
