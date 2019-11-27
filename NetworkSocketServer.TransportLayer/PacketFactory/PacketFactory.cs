﻿using System;
using System.Text;
using NetworkSocketServer.TransportLayer.DTO;

namespace NetworkSocketServer.TransportLayer.PacketFactory
{
    class PacketFactory : IPacketFactory
    {
        private readonly Guid _sessionId;

        public PacketFactory(Guid sessionId)
        {
            _sessionId = sessionId;
        }

        public Packet CreateAnswerSuccessWrite(int bufferSize, int bytesWrite)
        {
            return new Packet
            {
                SessionId = _sessionId,
                Position = bufferSize,
                Offset = bytesWrite,
                PacketClientCommand = PacketClientCommand.None,
                PacketServerResponse = PacketServerResponse.Answer,
                Payload = null,
            };
        }

        public Packet CreateAnswerError(string text)
        {
            return new Packet
            {
                SessionId = _sessionId,
                Position = 0,
                Offset = 0,
                PacketClientCommand = PacketClientCommand.None,
                PacketServerResponse = PacketServerResponse.Error,
                Payload = Encoding.ASCII.GetBytes(text),
            };
        }

        public Packet CreateAnswerSuccessRead(byte[] array, int transmitBufferLength, int arrayLength)
        {
            return new Packet
            {
                SessionId = _sessionId,
                PacketClientCommand = PacketClientCommand.None,
                PacketServerResponse = PacketServerResponse.Answer,
                Payload = array,
                Offset = arrayLength,
                Position = transmitBufferLength
            };
        }

        public Packet CreateAnswerExecuteSuccessPayload(byte[] responseBytes, int responseBytesLength)
        {
            return new Packet
            {
                Offset = responseBytesLength,
                PacketServerResponse = PacketServerResponse.Answer,
                PacketClientCommand = PacketClientCommand.None,
                Payload = responseBytes,
                Position = 0,
                SessionId = _sessionId
            };
        }

        public Packet CreateAnswerExecuteSuccessBuffer(int transmitBufferLength)
        {
            return new Packet
            {
                Position = 0,
                Offset = transmitBufferLength,
                PacketClientCommand = PacketClientCommand.None,
                PacketServerResponse = PacketServerResponse.ResultInBuffer,
                SessionId = _sessionId,
                Payload = null,
            };
        }

        public Packet CreateClosePacket()
        {
            return new Packet
            {
                Offset = 0,
                Position = 0,
                PacketServerResponse = PacketServerResponse.Answer,
                PacketClientCommand = PacketClientCommand.Close,
                Payload = null,
                SessionId = _sessionId
            };
        }

        public Packet CreateExecuteBuffer(int requestByteLength)
        {
            return new Packet
            {
                Position = 0,
                Offset = requestByteLength,
                SessionId = _sessionId,
                PacketServerResponse = PacketServerResponse.Answer,
                PacketClientCommand = PacketClientCommand.ExecuteBuffer,
                Payload = null
            };
        }

        public Packet CreateExecutePayload(byte[] requestBytes)
        {
            return new Packet
            {
                Offset = requestBytes.Length,
                Position = 0,
                PacketServerResponse = PacketServerResponse.Answer,
                PacketClientCommand = PacketClientCommand.ExecutePayload,
                Payload = requestBytes,
                SessionId = _sessionId
            };
        }
    }
}
