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

using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.World
{
    public class Ranking : Handler<WorldServer>
    {
        public override int Id => 0x1A;

        public Ranking(WorldServer server) : base(server)
        {
        }

        public override void Handle(EzClient client, EzPacket packet)
        {
            RankingType rankingType = (RankingType) packet.Data.ReadByte();
            byte page = packet.Data.ReadByte();
            _logger.Debug("Ranking: {0} Page: {1}", rankingType, page);


            IBuffer buffer = EzServer.Buffer.Provide();


            buffer.WriteByte((byte) rankingType);
            buffer.WriteByte(page);

            if (rankingType == RankingType.Level)
            {
                buffer.WriteByte(1); // Entries on Page
                buffer.WriteByte(8); // My Rank +1

                //1st entry
                buffer.WriteByte(7); // Rank +1
                buffer.WriteFixedString("12345678901234567", 17, Utils.KoreanEncoding);
                buffer.WriteInt16(88, Endianness.Big); //Level
                buffer.WriteByte(50); //EXP?
                buffer.WriteInt32(100, Endianness.Big); //MaxScore
                buffer.WriteInt16(1337, Endianness.Big); //MaxCombo
                buffer.WriteByte(1); //sex 0 = male
                buffer.WriteByte(10); //exr
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }
            else if (rankingType == RankingType.Battle)
            {
                buffer.WriteByte(1); // Entries on Page
                buffer.WriteByte(8); // My Rank +1

                //1st entry
                buffer.WriteByte(7); // Rank +1
                buffer.WriteFixedString("12345678901234567", 17, Utils.KoreanEncoding);
                buffer.WriteInt16(88, Endianness.Big); //Level
                buffer.WriteInt32(100, Endianness.Big); //WIN
                buffer.WriteInt32(60, Endianness.Big); //LOOSE
                buffer.WriteByte(0); //sex
                buffer.WriteByte(14); //exr
                buffer.WriteByte(0);
                buffer.WriteByte(0);
            }

            Send(client, 0x25, buffer);
        }
    }
}