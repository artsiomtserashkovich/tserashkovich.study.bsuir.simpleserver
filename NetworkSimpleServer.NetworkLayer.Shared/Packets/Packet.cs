﻿using System;

namespace NetworkSimpleServer.NetworkLayer.Core.Packets
{
    [Serializable]
    public class Packet
    {
        public Guid SessionId { get; set; }

        public Guid PacketId { get; set; }

        public int BuffferSize { get; set; }

        public int BufferOffset { get; set; }

        public int PayloadSize { get; set; }
        
        public PacketClientCommand PacketClientCommand { get; set; }

        public PacketServerResponse PacketServerResponse { get; set; }

        public byte[] Payload { get; set; }
    }
}