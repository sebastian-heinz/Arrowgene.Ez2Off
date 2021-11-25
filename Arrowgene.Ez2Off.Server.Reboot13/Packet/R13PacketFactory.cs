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

using System.IO;
using System.Security.Cryptography;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet
{
    public class R13PacketFactory : PacketFactory
    {
        protected override int HeaderSize => 7;
        protected override Endianness SizeEndianness => Endianness.Big;

        public R13PacketFactory(EzSettings settings) : base(settings)
        {
        }

        public override PacketFactory Provide()
        {
            return new R13PacketFactory(_settings);
        }
        protected override EzPacket CreatePacket(byte id, byte[] packetData, EzClient client)
        {
            IBuffer packetBuffer = EzServer.Buffer.Provide(packetData);
            EzPacket packet = new EzPacket(id, packetBuffer);
            return packet;
        }

        public override IBuffer CreateBuffer(EzPacket packet, EzClient client)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(packet.Id);
            buffer.WriteInt16((short) packet.Data.Size, Endianness.Big);
            buffer.WriteInt32(0);
            buffer.WriteBuffer(packet.Data);
            return buffer;
        }
    }
}