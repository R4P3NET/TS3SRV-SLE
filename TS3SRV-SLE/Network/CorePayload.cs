using System;

using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class CorePayload
    {
        public PayloadTypes PayloadType { get => (PayloadTypes)(PType & 0xF0); protected set => PType = (byte)((PType & 0x0F) | ((byte)value & 0xF0)); }
        public PayloadFlags PayloadFlags { get => (PayloadFlags)(Flags & 0xF0); protected set => Flags = (byte)((Flags & 0x0F) | ((byte)value & 0xF0)); }

        public byte[] Header { get; protected set; }
        public byte[] AuthKey { get; protected set; }
        public byte[] Payload { get; protected set; }

        public byte[] RawPayload { get; set; }

        public ushort SequenceId { get; set; }

        public byte PType { get; set; }
        public byte Flags { get; set; }
    }
}
