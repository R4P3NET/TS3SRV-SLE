using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace TS3SRV_SLE.Internal
{
    public class ServerProperties
    {
        [Option(Required = true, HelpText = "Name of the server")]
        public string Name { get; set; }

        [Option(Required = true, HelpText = "Port of the server")]
        public int Port { get; set; }

        [Option(Required = true, HelpText = "Number of slots")]
        public int Slots { get; set; }

        [Option(Required = true, HelpText = "Number of connected clients")]
        public int Clients { get; set; }

        [Option('p', "ispasswordprotected", Default = false, Required = true)]
        public bool IsPasswordProtected { get; set; }

        [Option('c', "cancreatechannels", Default = false, Required = true)]
        public bool CanCreateChannels { get; set; }
    }
}
