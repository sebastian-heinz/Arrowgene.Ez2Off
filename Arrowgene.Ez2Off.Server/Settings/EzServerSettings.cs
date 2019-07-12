/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2018 Sebastian Heinz
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
using System.Net;
using System.Runtime.Serialization;
using Arrowgene.Services.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.Ez2Off.Server.Settings
{
    [DataContract]
    public class EzServerSettings : ICloneable
    {
        public EzServerSettings(IPAddress publicIpAddress, IPAddress listenIpAddress, ushort port, ushort bridgePort)
        {
            Active = true;
            ListenIpAddress = listenIpAddress;
            PublicIpAddress = publicIpAddress;
            Port = port;
            BridgePort = bridgePort;
            LoadHandlerScripts = false;
            HandlerScriptsPath = String.Empty;
            LogUnknownIncomingPackets = true;
            LogOutgoingPackets = true;
            LogIncomingPackets = true;
            DatabaseSettings = new DatabaseSettings();
            ServerSettings = new AsyncEventSettings();
            BridgeServerSettings = new AsyncEventSettings();
        }

        public EzServerSettings(EzServerSettings settings)
        {
            Active = settings.Active;
            ListenIpAddress = settings.ListenIpAddress;
            PublicIpAddress = settings.PublicIpAddress;
            Port = settings.Port;
            BridgePort = settings.BridgePort;
            LoadHandlerScripts = settings.LoadHandlerScripts;
            HandlerScriptsPath = settings.HandlerScriptsPath;
            LogUnknownIncomingPackets = settings.LogUnknownIncomingPackets;
            LogOutgoingPackets = settings.LogOutgoingPackets;
            LogIncomingPackets = settings.LogIncomingPackets;
            DatabaseSettings = new DatabaseSettings(settings.DatabaseSettings);
            ServerSettings = new AsyncEventSettings(settings.ServerSettings);
            BridgeServerSettings = new AsyncEventSettings(settings.BridgeServerSettings);
        }

        [DataMember(Order = 0)]
        public bool Active { get; set; }

        [DataMember(Name = "ListenIpAddress", Order = 1)]
        public string DataListenIpAddress
        {
            get => ListenIpAddress.ToString();
            set => ListenIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Name = "PublicIpAddress", Order = 2)]
        public string DataPublicIpAddress
        {
            get => PublicIpAddress.ToString();
            set => PublicIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 3)]
        public ushort Port { get; set; }

        [DataMember(Order = 4)]
        public ushort BridgePort { get; set; }

        [DataMember(Order = 5)]
        public bool LoadHandlerScripts { get; set; }

        /// <summary>
        /// Path to scripts to load for this server.
        /// </summary>
        [DataMember(Order = 6)]
        public string HandlerScriptsPath { get; set; }

        [DataMember(Order = 7)]
        public bool LogUnknownIncomingPackets { get; set; }

        [DataMember(Order = 8)]
        public bool LogOutgoingPackets { get; set; }

        [DataMember(Order = 9)]
        public bool LogIncomingPackets { get; set; }

        [DataMember(Order = 10)]
        public DatabaseSettings DatabaseSettings { get; set; }

        [DataMember(Order = 11)]
        public AsyncEventSettings ServerSettings { get; set; }

        [DataMember(Order = 12)]
        public AsyncEventSettings BridgeServerSettings { get; set; }

        [IgnoreDataMember]
        public int Id { get; set; }

        /// <summary>
        /// Public PublicIpAddress which clients use to connect.
        /// </summary>
        [IgnoreDataMember]
        public IPAddress PublicIpAddress { get; set; }

        /// <summary>
        /// Internal PublicIpAddress used for incomming connections.
        /// </summary>
        [IgnoreDataMember]
        public IPAddress ListenIpAddress { get; set; }

        [IgnoreDataMember]
        public IPEndPoint ListenEndPoint => new IPEndPoint(ListenIpAddress, Port);

        [IgnoreDataMember]
        public IPEndPoint PublicEndPoint => new IPEndPoint(PublicIpAddress, Port);

        [IgnoreDataMember]
        public IPEndPoint BridgeListenEndPoint => new IPEndPoint(ListenIpAddress, BridgePort);

        [IgnoreDataMember]
        public IPEndPoint BridgePublicEndPoint => new IPEndPoint(PublicIpAddress, BridgePort);

        public object Clone()
        {
            return new EzServerSettings(this);
        }
    }
}