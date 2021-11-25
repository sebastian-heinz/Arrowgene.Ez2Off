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

using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet
{
    public class R14PacketFactory : PacketFactory
    {
        protected override int HeaderSize => 3;
        protected override Endianness SizeEndianness => Endianness.Little;

        private static readonly byte[] Key = {86, 120, 25, 72};

        public R14PacketFactory(EzSettings settings) : base(settings)
        {
        }

        public override PacketFactory Provide()
        {
            return new R14PacketFactory(_settings);
        }

        public override IBuffer CreateBuffer(EzPacket packet, EzClient client)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(packet.Id);
            buffer.WriteInt16((short) packet.Data.Size);
            byte[] packetData = packet.Data.GetAllBytes();
            Xor(packetData);
            buffer.WriteBytes(packetData);
            return buffer;
        }

        protected override EzPacket CreatePacket(byte id, byte[] packetData, EzClient client)
        {
            Xor(packetData);
            IBuffer packetBuffer = EzServer.Buffer.Provide(packetData);
            EzPacket packet = new EzPacket(id, packetBuffer);
            return packet;
        }

        private void Xor(byte[] data)
        {
            int keyIndex = 0;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte) (data[i] ^ Key[keyIndex]);
                keyIndex++;
                if (keyIndex >= Key.Length)
                {
                    keyIndex = 0;
                }
            }
        }
    }
}