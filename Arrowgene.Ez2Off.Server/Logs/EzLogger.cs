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
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ez2Off.Server.Logs
{
    public class EzLogger : Logger
    {
        private static bool _logUnknownIncomingPackets;
        private static bool _logOutgoingPackets;
        private static bool _logIncomingPackets;

        public override void Initialize(string identity, string zone, object configuration)
        {
            base.Initialize(identity, zone, configuration);
            EzSettings settings = configuration as EzSettings;
            if (settings != null)
            {
                _logUnknownIncomingPackets = settings.LogUnknownIncomingPackets;
                _logOutgoingPackets = settings.LogOutgoingPackets;
                _logIncomingPackets = settings.LogIncomingPackets;
            }
            else
            {
                Error("Couldn't apply EzLogger configuration");
            }
        }

        public void Info(EzClient socket, string message)
        {
            Info($"[{socket.Identity}] {message}");
        }

        public void Debug(EzClient socket, string message)
        {
            Debug($"[{socket.Identity}] {message}");
        }

        public void Error(EzClient socket, string message)
        {
            Error($"[{socket.Identity}] {message}");
        }

        public void Exception(EzClient socket, Exception exception)
        {
            if (exception == null)
            {
                Write(LogLevel.Error, $"[{socket.Identity}] Exception was null", null);
            }
            else
            {
                Write(LogLevel.Error, $"[{socket.Identity}] {exception}", exception);
            }
        }

        public void Info(ITcpSocket socket, string message)
        {
            Info($"[{socket.Identity}] {message}");
        }

        public void Debug(ITcpSocket socket, string message)
        {
            Debug($"[{socket.Identity}] {message}");
        }

        public void Error(ITcpSocket socket, string message)
        {
            Error($"[{socket.Identity}] {message}");
        }

        public void Exception(ITcpSocket socket, Exception exception)
        {
            if (exception == null)
            {
                Write(LogLevel.Error, $"[{socket.Identity}] Exception was null", null);
            }
            else
            {
                Write(LogLevel.Error, $"[{socket.Identity}] {exception}", exception);
            }
        }

        public void LogIncomingPacket(EzClient client, EzPacket packet)
        {
            if (_logIncomingPackets)
            {
                EzLogPacket logPacket = new EzLogPacket(client, packet, EzLogPacketType.In);
                Packet(logPacket);
            }
        }

        public void LogUnknownIncomingPacket(EzClient client, EzPacket packet)
        {
            if (_logUnknownIncomingPackets)
            {
                EzLogPacket logPacket = new EzLogPacket(client, packet, EzLogPacketType.Unhandled);
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
            Write(LogLevel.Info, packet.ToLogText(), packet.PacketType);
        }
    }
}