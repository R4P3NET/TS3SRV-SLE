using System;
using System.Net;
using System.Net.Sockets;

using NLog;
using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class ConnectionHandler
    {
        private IPAddress TS3SRV_WEBLIST_IP;
        private IPEndPoint TS3SRV_WEBLIST_ENDPOINT;

        private Socket TS3SRV_WEBLIST_SOCKET;

        public Action<IncomingPayloadHandler> HandleIncoming;

        private Logger Log = LogManager.GetCurrentClassLogger();

        public ConnectionHandler()
        {
            try
            {
                TS3SRV_WEBLIST_IP = Dns.GetHostAddresses(Properties.TS3SRV_WEBLISTURL)[0];
                TS3SRV_WEBLIST_ENDPOINT = new IPEndPoint(TS3SRV_WEBLIST_IP, Properties.TS3SRV_WEBLISTPORT);

                TS3SRV_WEBLIST_SOCKET = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            }
            catch
            {
            }
        }

        public void InitializeConnection()
        {
            try
            {
                TS3SRV_WEBLIST_SOCKET.Connect(TS3SRV_WEBLIST_ENDPOINT);
                if(TS3SRV_WEBLIST_SOCKET.Connected)
                {
                    InitializeListener();
                }
            }
            catch
            {

            }
        }


        public void SendPayload(byte[] Payload)
        {
            try
            {
                
            }
            catch
            {

            }
        }


        private void InitializeListener()
        {
            try
            {
                SocketHandler SHandler = new SocketHandler()
                {
                    TS3SRV_SOCKET = TS3SRV_WEBLIST_SOCKET,
                };

                TS3SRV_WEBLIST_SOCKET.BeginReceive(SHandler.TS3SRV_SOCKET_BUFFER,0,SHandler.TS3SRV_SOCKET_BUFFER.Length,SocketFlags.None,new AsyncCallback(Listener),SHandler);
            }
            catch
            {

            }
        }

        private void Listener(IAsyncResult AResult)
        {
            SocketHandler SHandler = (SocketHandler)AResult.AsyncState;
            int RecvLen = SHandler.TS3SRV_SOCKET.EndReceive(AResult);
            if(RecvLen > 0 && RecvLen <= SHandler.TS3SRV_SOCKET_BUFFER.Length)
            {
                try
                {
                    HandleIncoming(new IncomingPayloadHandler(SHandler.TS3SRV_SOCKET_BUFFER));
                    //Enter Listener Loop
                    InitializeListener();
                }
                catch
                {

                }
            }
        }
    }
}
