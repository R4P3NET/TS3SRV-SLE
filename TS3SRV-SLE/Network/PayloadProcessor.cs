using System;

namespace TS3SRV_SLE.Network
{
    public class PayloadProcessor
    {
        private ConnectionHandler CHandler;
        
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
            
        }

        private void OnOutgoing(OutgoingPayloadHandler Payload)
        {

        }


        #region "Begin LowLevel Payload Handling Routines"

        private void ProcessIncomingPayload()
        {

        }
        #endregion
    }
}
