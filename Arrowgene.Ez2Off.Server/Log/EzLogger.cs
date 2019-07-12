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
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Services.Logging;

namespace Arrowgene.Ez2Off.Server.Log
{
    public class EzLogger : Logger
    {
        private static bool _logUnknownIncomingPackets;
        private static bool _logOutgoingPackets;
        private static bool _logIncomingPackets;

        public event EventHandler<EzPacketLoggedEventArgs> EzPacketLogged;

        public EzLogger() : this(null)
        {
        }

        public EzLogger(string identity, string zone = null) : base(identity, zone)
        {
        }

        public void LogIncomingPacket(EzClient client, EzPacket packet)
        {
            if (_logIncomingPackets)
            {
                EzLogPacket logPacket = new EzLogPacket(client, packet, EzLogPacketType.In);
                Packet(logPacket);
            }
        }

        public void LogUnknownIncommingPacket(EzClient client, EzPacket packet)
        {
            if (_logUnknownIncomingPackets)
            {
                EzLogPacket logPacket = new EzLogPacket(client, packet, EzLogPacketType.Unhandeled);
                Packet(logPacket);
            }
        }

        public void LogOutgoingPacket(EzClient client, EzPacket packet)
        {
            if (_logOutgoingPackets)
            {
                EzLogPacket logPacket = new EzLogPacket(client, packet, EzLogPacketType.Out);
                Packet(logPacket);
            }
        }

        public void Packet(EzLogPacket packet)
        {
            Write(LogLevel.Info, packet.PacketType, packet.ToLogText());
            OnEzPacketLogged(packet);
        }

        protected override void Configure(object configuration)
        {
            EzServerSettings serverConfig = configuration as EzServerSettings;
            if (serverConfig != null)
            {
                _logUnknownIncomingPackets = serverConfig.LogUnknownIncomingPackets;
                _logOutgoingPackets = serverConfig.LogOutgoingPackets;
                _logIncomingPackets = serverConfig.LogIncomingPackets;
            }

            base.Configure(configuration);
        }

        private void OnEzPacketLogged(EzLogPacket logPacket)
        {
            EventHandler<EzPacketLoggedEventArgs> ezPacketLogged = EzPacketLogged;
            if (ezPacketLogged != null)
            {
                EzPacketLoggedEventArgs ezPacketLoggedEventArgs = new EzPacketLoggedEventArgs(logPacket);
                ezPacketLogged(this, ezPacketLoggedEventArgs);
            }
        }
    }
}