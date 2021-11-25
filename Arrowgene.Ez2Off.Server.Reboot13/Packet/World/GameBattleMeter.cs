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
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.World
{
    /// <summary>
    /// Will be send on every 30 Combos (30 / combo == 0) or when Fail
    /// </summary>
    public class GameBattleMeter : Handler<EzServer>
    {
        public GameBattleMeter(EzServer server) : base(server)
        {
        }

        public override int Id => 17;

        public override void Handle(EzClient client, EzPacket received)
        {
            byte notePosition = received.Data.ReadByte();
            // notePosition + 15
            byte notePosition2 = received.Data.ReadByte();
            short progress = received.Data.ReadInt16(Endianness.Big);
            // 0 = Miss, 1 = Fail, 2 = Good, 3 = Cool, 4 = Kool
            byte noteType = received.Data.ReadByte();
            short bestCombo = received.Data.ReadInt16(Endianness.Big);
            int score = received.Data.ReadInt32(Endianness.Big);
            short health = received.Data.ReadInt16(Endianness.Big);

           // Logger.Debug(
           //     "NotePosition:{0} NotePosition2:{1} Progress:{2} NoteType:{3} BestCombo:{4} Score:{5} Health:{6}",
            //    notePosition, notePosition2, progress, noteType, bestCombo, score, health);

            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt16((short) client.Player.Slot, Endianness.Big);
            buffer.WriteByte(notePosition);
            buffer.WriteByte(notePosition2);
            buffer.WriteInt16(progress, Endianness.Big);
            buffer.WriteByte(noteType);
            buffer.WriteInt16(bestCombo, Endianness.Big);
            buffer.WriteInt32(score, Endianness.Big);
            buffer.WriteInt16(health, Endianness.Big);
            Router.Send(client.Room, 25, buffer, client);
        }
    }
}