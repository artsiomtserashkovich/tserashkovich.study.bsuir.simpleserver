﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.Acceptors;
using NetworkSocketServer.NetworkLayer.TransportHandler.Factories;

namespace NetworkSocketServer.NetworkLayer.ConnectionDispatcher
{
    internal class SingleThreadAcceptorDispatcher : IConnectionDispatcher
    {
        private readonly INewTransportHandler _newTransportHandler;
        private readonly ITransportHandlerFactory _transportHandlerFactory;
        private readonly IList<INetworkAcceptor> _acceptors;

        public SingleThreadAcceptorDispatcher(
            INewTransportHandler newTransportHandler,
            ITransportHandlerFactory transportHandlerFactory)
        {
            _newTransportHandler = newTransportHandler;
            _transportHandlerFactory = transportHandlerFactory;
            _acceptors = new List<INetworkAcceptor>();
        }

        public void RegisterAcceptor(INetworkAcceptor acceptor)
        {
            _acceptors.Add(acceptor);
        }

        public void StartListen()
        {
            InternalStart().Wait();
        }

        private async Task InternalStart()
        {
            while (true)
            {
                foreach (var acceptor in _acceptors)
                {
                    if (!acceptor.IsHaveNewConnection()) continue;

                    try
                    {
                        using var transportHandler = _transportHandlerFactory.CreateTransportHandler();

                        await acceptor.AcceptConnection(transportHandler);

                        await _newTransportHandler.HandleNewConnection(transportHandler);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Error happend:" + exception.Message);
                    }
                }
            }
        }

        public void StopListen()
        {
            throw new NotSupportedException();
        }
    }
}
