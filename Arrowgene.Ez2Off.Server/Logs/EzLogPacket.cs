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

namespace Arrowgene.Ez2Off.Server.Logs
{
    public class EzLogPacket : EzPacket
    {
        public EzLogPacket(EzClient client, EzPacket ezPacket, EzLogPacketType packetType) : base(ezPacket.Id,
            ezPacket.Data.Clone())
        {
            PacketType = packetType;
            TimeStamp = DateTime.UtcNow;
            Client = client;
            Encrypted = ezPacket.Encrypted;
        }

        public EzClient Client { get; }
        public EzLogPacketType PacketType { get; }
        public DateTime TimeStamp { get; }
        public string Hex => Data.ToHexString("-");
        public string Ascii => Data.ToAsciiString(" ");
        public string EncryptedHex => Encrypted == null ? "[No Encrypted Data Logged]" : Encrypted.ToHexString("-");

        public string ToLogText()
        {
            String log = $"{Client.Identity} Packet Log";
            log += Environment.NewLine;
            log += "----------";
            log += Environment.NewLine;
            log += $"[{TimeStamp:HH:mm:ss}][Typ:{PacketType}][Id:{Id}][Len:{Data.Size}]";
            if (Encrypted != null)
            {
                log += $"[EncLen: {Encrypted.Size}]";
            }

            log += Environment.NewLine;
            log += "ASCII:";
            log += Environment.NewLine;
            log += Ascii;
            log += Environment.NewLine;
            log += "HEX:";
            log += Environment.NewLine;
            log += Hex;
            if (Encrypted != null)
            {
                log += Environment.NewLine;
                log += "Encrypted HEX:";
                log += Environment.NewLine;
                log += EncryptedHex;
            }

            log += Environment.NewLine;
            log += "----------";
            return log;
        }
    }
}