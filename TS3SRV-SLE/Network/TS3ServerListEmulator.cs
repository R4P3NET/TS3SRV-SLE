using System;
using System.Collections.Generic;
using System.Text;

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

        public TS3ServerListEmulator()
        {
            PayloadProcessor PProcessor = new PayloadProcessor();

        }
    }
}
