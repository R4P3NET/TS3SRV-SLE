using System;
using System.Net.Sockets;

using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class SocketHandler
    {
        public Socket TS3SRV_SOCKET;
        public byte[] TS3SRV_SOCKET_BUFFER = new byte[Properties.TS3SRV_WEBLIST_SOCKET_BUFFSIZE];
    }
}
