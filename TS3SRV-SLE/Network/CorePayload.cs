using System;

using TS3SRV_SLE.Internal;

namespace TS3SRV_SLE.Network
{
    public class CorePayload
    {
        public PayloadTypes PayloadType { get => (PayloadTypes)(PType & 0xFF); set => PType = (byte)((PType & 0x0F) | ((byte)value & 0xFF)); }
        public PayloadFlags PayloadFlags { get => (PayloadFlags)(Flags & 0x0F); set => Flags = (byte)(Flags | (byte)value); }

        public byte[] Header { get; set; }
        public byte[] AuthKey { get; set; }
        public byte[] Payload { get; set; }

        public byte[] RawPayload { get; set; }

        public ushort SequenceId { get; set; }

        public byte PType { get; set; }
        public byte Flags { get; set; }

        public bool CanCreateChannel { get => (PayloadFlags & PayloadFlags.CanCreateChannel) != 0; set { if (value) Flags |= (byte)PayloadFlags.CanCreateChannel; else Flags &= (byte)~PayloadFlags.CanCreateChannel; } }
        public bool IsPasswordProtected { get => (PayloadFlags & PayloadFlags.IsPasswordProtected) != 0; set { if (value) Flags |= (byte)PayloadFlags.IsPasswordProtected; else Flags &= (byte)~PayloadFlags.IsPasswordProtected; } }
    }
}
