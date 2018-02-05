using System;

using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class IncomingPayloadHandler : CorePayload
    {
        public IncomingPayloadHandler(byte[] RawData)
        {
            try
            {
                if (RawData.Length > Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN && AuthKey == null)
                {
                    Array.Copy(RawData, Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN, AuthKey, 0, RawData.Length - Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN);
                }
                Array.Copy(RawData, 0, Header, 0, Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN);
                ProcessHeader(Header);
            }
            catch
            {

            }
        }

        private void ProcessHeader(byte[] Header)
        {
            if(Header.Length > Properties.TS3SRV_WEBLIST_PROTOCOL_HEADERLEN)
            {
                SequenceId = BitConverter.ToUInt16(new byte[2] { Header[1], Header[2] }, 0);
                PType = Header[3];
            }
        }
    }
}
