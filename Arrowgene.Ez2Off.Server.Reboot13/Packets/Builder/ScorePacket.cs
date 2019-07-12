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

using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder
{
    public class ScorePacket
    {
        public static IBuffer Create(List<EzClient> clients)
        {
            short count = (short) clients.Count;

            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt16((short) clients.Count, Endianness.Big);
            for (short i = 0; i < count; i++)
            {
                EzClient client = clients[i];
                Score score = client.Score;
                buffer.WriteInt16(i, Endianness.Big);
                buffer.WriteByte(score.StageClear ? (byte) 0 : (byte) 1);
                buffer.WriteInt16((short) score.MaxCombo, Endianness.Big);
                buffer.WriteInt16((short) score.Kool, Endianness.Big);
                buffer.WriteInt16((short) score.Cool, Endianness.Big);
                buffer.WriteInt16((short) score.Good, Endianness.Big);
                buffer.WriteInt16((short) score.Miss, Endianness.Big);
                buffer.WriteInt16((short) score.Fail, Endianness.Big);
                buffer.WriteByte(0);
                buffer.WriteByte((byte) score.ComboType);
                buffer.WriteInt32(score.TotalScore, Endianness.Big);
                buffer.WriteInt16(0, Endianness.Big); //+ EXP
                buffer.WriteByte((byte) score.Rank);
                buffer.WriteInt16(0, Endianness.Big); // + Coin Increase [MAX:9999]
                buffer.WriteByte(0); // 1 = Level Up [HP Points +1 / DJ Points +1] increase
                buffer.WriteInt16((short) score.TotalNotes, Endianness.Big);
                buffer.WriteByte(0);
                buffer.WriteByte(5);
                buffer.WriteInt16(0); //EXP +%
                buffer.WriteInt16(0); //Coin +%
                buffer.WriteByte(client.Character.Level);
                buffer.WriteInt32(client.Character.Exp);
                buffer.WriteInt32(Character.ExpForNextLevel(client.Character));
                buffer.WriteInt32(score.TotalScore, Endianness.Big); // Best Score
                buffer.WriteByte(1); //Song completion EXR increase
                buffer.WriteByte((byte) client.Character.GetExr(client.Mode));
            }

            return buffer;
        }
    }
}