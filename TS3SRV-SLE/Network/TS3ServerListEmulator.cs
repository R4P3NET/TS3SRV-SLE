using System;
using System.Collections.Generic;
using System.Text;
using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class TS3ServerListEmulator
    {
        #region "Server Properties"
        public string TS3SRV_NAME;
        public int TS3SRV_PORT;
        public int TS3SRV_SLOTCOUNT;
        public int TS3SRV_CLIENTCOUNT;
        #endregion

        public TS3ServerListEmulator(ServerProperties serverProperties)
        {
            //var properties = new ServerProperties() { CanCreateChannels = true, Clients = 20, Slots = 32, Name = "hradebka", IsPasswordProtected = true, Port = 9987};
            PayloadProcessor PProcessor = new PayloadProcessor(serverProperties);

        }
    }
}
