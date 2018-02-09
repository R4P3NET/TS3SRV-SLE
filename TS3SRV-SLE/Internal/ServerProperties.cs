using System;
using System.Collections.Generic;
using System.Text;

namespace TS3SRV_SLE.Internal
{
    public class ServerProperties
    {
        public string Name { get; set; }

        public int Port { get; set; }

        public int Slots { get; set; }

        public int Clients { get; set; }

        public bool PasswordProtected { get; set; }

        public bool CanCreateChannels { get; set; }
    }
}
