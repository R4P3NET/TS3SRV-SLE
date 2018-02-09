using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
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

        private static readonly Logger Logger = LogManager.GetLogger(Properties.TS3SRV_LOGGER_NAME);

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
                if (TS3SRV_WEBLIST_SOCKET.Connected)
                {
                    Logger.Log(LogLevel.Info, "Successfully connected to a weblist server, starting to send data");
                    InitializeListener();
                }
            }
            catch
            {

            }
        }


        public async Task SendPayloadAsync(byte[] Payload)
        {
            try
            {
                //TS3SRV_WEBLIST_SOCKET.BeginSend(Payload, 0, Payload.Length, SocketFlags.None, null, null);
                await TS3SRV_WEBLIST_SOCKET.SendAsync(Payload, SocketFlags.None);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
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

                TS3SRV_WEBLIST_SOCKET.BeginReceive(SHandler.TS3SRV_SOCKET_BUFFER, 0, SHandler.TS3SRV_SOCKET_BUFFER.Length, SocketFlags.None, Listener, SHandler);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void Listener(IAsyncResult AResult)
        {
            SocketHandler SHandler = (SocketHandler)AResult.AsyncState;
            int recvLen = SHandler.TS3SRV_SOCKET.EndReceive(AResult);
            InitializeListener();
            if (recvLen > 0 && recvLen <= SHandler.TS3SRV_SOCKET_BUFFER.Length)
            {
                try
                {
                    byte[] actualDataBytes = new byte[recvLen];
                    Array.Copy(SHandler.TS3SRV_SOCKET_BUFFER, 0, actualDataBytes, 0, recvLen);
                    HandleIncoming(new IncomingPayloadHandler(actualDataBytes));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
