﻿using System;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.SocketOptionsAccessor.KeepAlive;
using NetworkSocketServer.Client.Inputs;
using NetworkSocketServer.TransportLayer.Client.ClientManager;

namespace NetworkSocketServer.Client
{
    public class Client
    {
        private readonly InputManager _inputManager;
        private readonly CommandExecutor _commandExecutor;

        public Client(IClientConnectionManagerFactory factory, SocketKeepAliveOptions keepAliveOptions)
        {
            _inputManager = new InputManager(new CommandParser());

            _commandExecutor = new CommandExecutor(factory.Create(keepAliveOptions));
        }

        public async Task Run()
        {

            while (true)
            {
                try
                {
                    var command = _inputManager.GetCommand();

                    await command.Execute(_commandExecutor);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Client Error" + ex.Message);
                }
            }
        }
    }
}
