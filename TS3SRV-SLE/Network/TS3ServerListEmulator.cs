using System;
using System.Collections.Generic;
using System.Text;
using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class Ts3ServerListEmulator
    {
        public Ts3ServerListEmulator(ServerProperties serverProperties)
        {
            var PProcessor = new PayloadProcessor(serverProperties);
        }
    }
}
