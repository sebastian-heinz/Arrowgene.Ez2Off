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

using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.Ez2Off.Server.Settings
{
    [DataContract]
    public class EzSettings
    {
        /// <summary>
        /// Warning:
        /// Changing while having existing accounts requires to rehash all passwords.
        /// </summary>
        public const int BCryptWorkFactor = 14;

        [IgnoreDataMember]
        public IPAddress ListenIpAddress { get; set; }

        [DataMember(Name = "ListenIpAddress", Order = 0)]
        public string DataListenIpAddress
        {
            get => ListenIpAddress.ToString();
            set => ListenIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [IgnoreDataMember]
        public IPAddress LoginIpAddress { get; set; }

        [DataMember(Name = "LoginIpAddress", Order = 1)]
        public string DataLoginIpAddress
        {
            get => LoginIpAddress.ToString();
            set => LoginIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 2)]
        public ushort LoginPort { get; set; }

        [IgnoreDataMember]
        public IPAddress GameIpAddress { get; set; }

        [DataMember(Name = "GameIpAddress", Order = 3)]
        public string DataGameIpAddress
        {
            get => GameIpAddress.ToString();
            set => GameIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 4)]
        public ushort GamePort { get; set; }

        [DataMember(Order = 5)]
        public int LogLevel { get; set; }

        [DataMember(Order = 7)]
        public bool AdminOnly { get; set; }

        [DataMember(Order = 10)]
        public bool LogUnknownIncomingPackets { get; set; }

        [DataMember(Order = 11)]
        public bool LogOutgoingPackets { get; set; }

        [DataMember(Order = 12)]
        public bool LogIncomingPackets { get; set; }

        [DataMember(Order = 13)]
        public bool LogEncryptedPackets { get; set; }

        [DataMember(Order = 40)]
        public bool CombineChannel { get; set; }

        [DataMember(Order = 41)]
        public bool NeedRegistration { get; set; }

        [DataMember(Order = 42)]
        public bool NeedCharacter { get; set; }

        [DataMember(Order = 43)]
        public int ChannelLoadMultiplier { get; set; }

        [DataMember(Order = 44)]
        public int ServerLoadMultiplier { get; set; }

        [DataMember(Order = 70)]
        public DatabaseSettings DatabaseSettings { get; set; }

        [DataMember(Order = 100)]
        public AsyncEventSettings LoginSocketServerSettings { get; set; }

        [DataMember(Order = 110)]
        public AsyncEventSettings GameSocketServerSettings { get; set; }

        public EzSettings()
        {
            ListenIpAddress = IPAddress.Any;
            LoginIpAddress = IPAddress.Loopback;
            LoginPort = 9350;
            GameIpAddress = IPAddress.Loopback;
            GamePort = 9360;
            LogLevel = 0;
            AdminOnly = false;
            LogUnknownIncomingPackets = false;
            LogOutgoingPackets = false;
            LogIncomingPackets = false;
            NeedRegistration = false;
            NeedCharacter = false;
            CombineChannel = false;
            ChannelLoadMultiplier = 1;
            ServerLoadMultiplier = 1;
            DatabaseSettings = new DatabaseSettings();
            LoginSocketServerSettings = new AsyncEventSettings {Identity = "Login"};
            GameSocketServerSettings = new AsyncEventSettings {Identity = "Game"};
        }

        public EzSettings(EzSettings settings)
        {
            ListenIpAddress = settings.ListenIpAddress;
            LoginIpAddress = settings.LoginIpAddress;
            LoginPort = settings.LoginPort;
            GameIpAddress = settings.GameIpAddress;
            GamePort = settings.GamePort;
            LogLevel = settings.LogLevel;
            AdminOnly = settings.AdminOnly;
            LogUnknownIncomingPackets = settings.LogUnknownIncomingPackets;
            LogOutgoingPackets = settings.LogOutgoingPackets;
            LogIncomingPackets = settings.LogIncomingPackets;
            NeedRegistration = settings.NeedRegistration;
            NeedCharacter = settings.NeedCharacter;
            CombineChannel = settings.CombineChannel;
            ChannelLoadMultiplier = settings.ChannelLoadMultiplier;
            ServerLoadMultiplier = settings.ServerLoadMultiplier;
            DatabaseSettings = new DatabaseSettings(settings.DatabaseSettings);
            LoginSocketServerSettings = new AsyncEventSettings(settings.LoginSocketServerSettings);
            GameSocketServerSettings = new AsyncEventSettings(settings.GameSocketServerSettings);
        }
    }
}