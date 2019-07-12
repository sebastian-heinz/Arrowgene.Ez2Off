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
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Log
{
    public class EzLogPacket : EzPacket
    {
        public EzLogPacket(EzClient client, EzPacket ezPacket, EzLogPacketType packetType) : base(ezPacket.Id, EzServer.Buffer.Provide(ezPacket.Data.GetAllBytes()))
        {
            PacketType = packetType;
            TimeStamp = DateTime.UtcNow;
            Buffer = ezPacket.ToData();
        }


        public EzLogPacketType PacketType { get; private set; }
        public DateTime TimeStamp { get; private set; }

        public string Hex
        {
            
            get { return Buffer.ToHexString('-'); }
        }

        public string ASCII
        {
            get { return Buffer.ToAsciiString(true); }
        }

        public IBuffer Buffer { get; private set; }

        public string ToLogText()
        {
            String log = "Packet Log";
            log += Environment.NewLine;
            log +=   "----------";
            log += Environment.NewLine;
            log += string.Format("[{0:HH:mm:ss}][Typ:{1}][Id:{2}][Len:{3}]", TimeStamp, PacketType, Id, Buffer.Size);
            log += Environment.NewLine;
            log += "ASCII:" + ASCII;
            log += Environment.NewLine;
            log += "HEX:" + Hex;
            log += Environment.NewLine;
            log += "----------";
            return log;
        }
    }
}