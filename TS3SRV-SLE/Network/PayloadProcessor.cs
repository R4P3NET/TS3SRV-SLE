using System;
using System.Threading.Tasks;
using NLog;
using TS3SRV_SLE.Internal;
using System.Timers;

namespace TS3SRV_SLE.Network
{
    public class PayloadProcessor
    {
        private ConnectionHandler CHandler;

        private static readonly Logger Logger = LogManager.GetLogger(Properties.TS3SRV_LOGGER_NAME);

        private byte[] TS3SRV_WEBLIST_AUTHKEY;
        private ushort TS3SRV_WEBLIST_SEQUENCEID;

        private readonly ServerProperties _serverProperties;

        private readonly Timer _timer = new Timer(600000);

        private bool awaitingAuthKey = false;

        public PayloadProcessor(ServerProperties serverProperties)
        {
            CHandler = new ConnectionHandler() { HandleIncoming = OnIncoming };
            CHandler.InitializeConnection();
            _serverProperties = serverProperties;
            TS3SRV_WEBLIST_AUTHKEY = new byte[4];
            TS3SRV_WEBLIST_SEQUENCEID = 1;
            _timer.Elapsed += TimerElapsed;
            TimerElapsed(null, null);
            _timer.Start();
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            await ProcessInitialKeyRequest();
            while (awaitingAuthKey)
            {
                await Task.Delay(25);
            }
            var outgoingPayload = new OutgoingPayloadHandler(_serverProperties)
            {
                SequenceId = TS3SRV_WEBLIST_SEQUENCEID,
                AuthKey = TS3SRV_WEBLIST_AUTHKEY,
            };
            outgoingPayload.ConstructHeader();
            if (outgoingPayload.ConstructDataPacket())
            {
                await CHandler.SendPayloadAsync(outgoingPayload.RawPayload);
                Logger.Log(LogLevel.Info, "Sent packet with info to serverlist");
                Console.WriteLine("Sent packet to the serverlist");
            }
        }

        private void OnIncoming(IncomingPayloadHandler payload)
        {
            payload = ProcessIncomingPayload(payload);

            //TODO
        }


        #region "Begin LowLevel Payload Handling Routines"

        public async Task ProcessInitialKeyRequest()
        {
            awaitingAuthKey = true;
            OutgoingPayloadHandler Payload = new OutgoingPayloadHandler()
            {
                SequenceId = TS3SRV_WEBLIST_SEQUENCEID,
                PayloadType = PayloadTypes.Keyrequest,
            };
            Payload.ConstructHeader();
            await CHandler.SendPayloadAsync(Payload.Header);
            TS3SRV_WEBLIST_SEQUENCEID++;
        }


        private IncomingPayloadHandler ProcessIncomingPayload(IncomingPayloadHandler Payload)
        {
            Payload.AuthKey = new byte[4];
            Payload.Header = new byte[4];

            //Payload Contains Authkey, save it
            if (Payload.RawPayload.Length == (Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN + Properties.TS3SRV_WEBLIST_PROTOCOL_AUTHKEYLEN))
            {
                Array.Copy(Payload.RawPayload, Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN, Payload.AuthKey, 0, Payload.RawPayload.Length - Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN);
                TS3SRV_WEBLIST_AUTHKEY = Payload.AuthKey;
                awaitingAuthKey = false;
                Logger.Log(LogLevel.Info, "Got Authkey, saving it. ");
                Console.WriteLine("GOT AUTH KEY");
            }

            Array.Copy(Payload.RawPayload, 0, Payload.Header, 0, Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN);

            if (Payload.RawPayload.Length == Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN + 1)
            {
                var responseCode = (ResponseStatus)Payload.RawPayload[Payload.RawPayload.Length - 1];
                Console.WriteLine("Got response: " + responseCode);
                if (responseCode.Equals(ResponseStatus.Ok))
                {
                    //Logger.Log(LogLevel.Info, "Successfully added server to the serverlist: " + responseCode);
                    Console.WriteLine("Successfully added server to the serverlist: " + responseCode);
                    TS3SRV_WEBLIST_SEQUENCEID++;
                } else if (responseCode.Equals(ResponseStatus.Busy))
                {
                    _timer.Interval = 60000;
                    _timer.Stop();
                    _timer.Start();
                }
            }

            //Ignore first header byte as it's not needed
            Payload.SequenceId = BitConverter.ToUInt16(new byte[2] { Payload.Header[2], Payload.Header[1] }, 0);
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
