/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2020 Sebastian Heinz
 *
 * Github: https://github.com/Arrowgene/Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ez2Off is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ez2Off. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Arrowgene.Ez2Off.Server.Logs;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Arrowgene.Networking.Tcp.Consumer;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using Arrowgene.Services;

namespace Arrowgene.Ez2Off.Server
{
    public class EzQueueConsumer : IConsumer
    {
        public const int NoExpectedSize = -1;

        private readonly BlockingCollection<ClientEvent>[] _queues;
        private readonly Thread[] _threads;
        private readonly Dictionary<int, IHandler> _handlers;
        private readonly Dictionary<ITcpSocket, EzClient> _clients;
        private readonly IPacketFactoryProvider _provider;
        private readonly EzLogger _logger;
        private readonly object _lock;
        private readonly int _maxUnitOfOrder;
        private string _identity;
        private volatile bool _isRunning;

        private CancellationTokenSource _cancellationTokenSource;

        public int HandlersCount => _handlers.Count;

        public Action<EzClient> ClientDisconnected;
        public Action<EzClient> ClientConnected;
        public Action Started;
        public Action Stopped;

        public void SetIdentity(string identity)
        {
            if (!string.IsNullOrEmpty(identity))
            {
                _identity = $"[{identity}] ";
            }
        }

        public EzQueueConsumer(IPacketFactoryProvider provider, int maxUnitOfOrder)
        {
            _logger = LogProvider.Logger<EzLogger>(this);
            _maxUnitOfOrder = maxUnitOfOrder;
            _queues = new BlockingCollection<ClientEvent>[_maxUnitOfOrder];
            _threads = new Thread[_maxUnitOfOrder];
            _lock = new object();
            _provider = provider;
            _handlers = new Dictionary<int, IHandler>();
            _clients = new Dictionary<ITcpSocket, EzClient>();
            _identity = "";
        }

        public void Clear()
        {
            _handlers.Clear();
        }

        public void AddHandler(IHandler handler, bool overwrite = false)
        {
            if (overwrite)
            {
                if (_handlers.ContainsKey(handler.Id))
                {
                    _handlers[handler.Id] = handler;
                }
                else
                {
                    _handlers.Add(handler.Id, handler);
                }

                return;
            }

            if (_handlers.ContainsKey(handler.Id))
            {
                _logger.Error($"{_identity}HandlerId: {handler.Id} already exists");
            }
            else
            {
                _handlers.Add(handler.Id, handler);
            }
        }

        private void HandleReceived(ITcpSocket socket, byte[] data)
        {
            if (!socket.IsAlive)
            {
                return;
            }

            EzClient client;
            lock (_lock)
            {
                if (!_clients.ContainsKey(socket))
                {
                    _logger.Error(socket, $"{_identity}Client does not exist in lookup");
                    return;
                }

                client = _clients[socket];
            }

            List<EzPacket> packets = client.Receive(data);
            foreach (EzPacket packet in packets)
            {
                if (_handlers.ContainsKey(packet.Id))
                {
                    IHandler handler = _handlers[packet.Id];
                    if (handler.ExpectedSize != NoExpectedSize && packet.Data.Size < handler.ExpectedSize)
                    {
                        _logger.Error(client,
                            $"{_identity}Ignoring Packed (Id:{packet.Id}) is smaller ({packet.Data.Size}) than expected ({handler.ExpectedSize})");
                        continue;
                    }

                    _logger.LogIncomingPacket(client, packet);
                    packet.Data.SetPositionStart();
                    try
                    {
                        handler.Handle(client, packet);
                    }
                    catch (Exception ex)
                    {
                        _logger.Exception(client, ex);
                    }
                }
                else
                {
                    _logger.LogUnknownIncomingPacket(client, packet);
                }
            }
        }

        private void HandleDisconnected(ITcpSocket socket)
        {
            EzClient client;
            lock (_lock)
            {
                if (!_clients.ContainsKey(socket))
                {
                    _logger.Error(socket, $"{_identity}Disconnected client does not exist in lookup");
                    return;
                }

                client = _clients[socket];
                _clients.Remove(socket);
                _logger.Debug($"{_identity}Clients Count: {_clients.Count}");
            }

            Action<EzClient> onClientDisconnected = ClientDisconnected;
            if (onClientDisconnected != null)
            {
                try
                {
                    onClientDisconnected.Invoke(client);
                }
                catch (Exception ex)
                {
                    _logger.Exception(client, ex);
                }
            }

            _logger.Info(client, $"{_identity}Client disconnected");
        }

        private void HandleConnected(ITcpSocket socket)
        {
            EzClient client = new EzClient(socket, _provider.Provide());
            lock (_lock)
            {
                _clients.Add(socket, client);
                _logger.Debug($"{_identity}Clients Count: {_clients.Count}");
            }

            Action<EzClient> onClientConnected = ClientConnected;
            if (onClientConnected != null)
            {
                try
                {
                    onClientConnected.Invoke(client);
                }
                catch (Exception ex)
                {
                    _logger.Exception(client, ex);
                }
            }

            _logger.Info(client, $"{_identity}Client connected");
        }

        private void Consume(int unitOfOrder)
        {
            while (_isRunning)
            {
                ClientEvent clientEvent;
                try
                {
                    clientEvent = _queues[unitOfOrder].Take(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                switch (clientEvent.ClientEventType)
                {
                    case ClientEventType.ReceivedData:
                        HandleReceived(clientEvent.Socket, clientEvent.Data);
                        break;
                    case ClientEventType.Connected:
                        HandleConnected(clientEvent.Socket);
                        break;
                    case ClientEventType.Disconnected:
                        HandleDisconnected(clientEvent.Socket);
                        break;
                }
            }
        }

        void IConsumer.OnStart()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _isRunning = true;
            for (int i = 0; i < _maxUnitOfOrder; i++)
            {
                int uuo = i;
                _queues[i] = new BlockingCollection<ClientEvent>();
                _threads[i] = new Thread(() => Consume(uuo));
                _threads[i].Name = $"{_identity}Consumer: {i}";
                _logger.Info($"{_identity}Starting Consumer: {i}");
                _threads[i].Start();
            }
        }

        public void LogStatus(string name)
        {
            lock (_lock)
            {
                for (int i = 0; i < _maxUnitOfOrder; i++)
                {
                    _logger.Info($"{name} :: _threads[{i}].IsAlive:{_threads[i].IsAlive}");
                    _logger.Info($"{name} :: _threads[{i}].ThreadState:{_threads[i].ThreadState}");
                    _logger.Info($"{name} :: _queues[{i}].Count :{_queues[i].Count}");
                }

                _logger.Info($"{name} :: _isRunning :{_isRunning}");
                _logger.Info($"{name} :: _clients.Count :{_clients.Count}");
            }
        }

        public void OnStarted()
        {
            Action started = Started;
            if (started != null)
            {
                started.Invoke();
            }
        }

        void IConsumer.OnReceivedData(ITcpSocket socket, byte[] data)
        {
            _queues[socket.UnitOfOrder].Add(new ClientEvent(socket, ClientEventType.ReceivedData, data));
        }

        void IConsumer.OnClientDisconnected(ITcpSocket socket)
        {
            _queues[socket.UnitOfOrder].Add(new ClientEvent(socket, ClientEventType.Disconnected));
        }

        void IConsumer.OnClientConnected(ITcpSocket socket)
        {
            _queues[socket.UnitOfOrder].Add(new ClientEvent(socket, ClientEventType.Connected));
        }

        void IConsumer.OnStop()
        {
            _isRunning = false;
            _cancellationTokenSource.Cancel();
            for (int i = 0; i < _maxUnitOfOrder; i++)
            {
                Thread consumerThread = _threads[i];
                _logger.Info($"{_identity}Shutting Consumer: {i} down...");
                Service.JoinThread(consumerThread, 10000, _logger);
                _logger.Info($"{_identity}Consumer: {i} ended.");
                _threads[i] = null;
            }
        }

        public void OnStopped()
        {
            Action stopped = Stopped;
            if (stopped != null)
            {
                stopped.Invoke();
            }
        }
    }
}