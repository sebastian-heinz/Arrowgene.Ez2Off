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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.World
{
    public class GameResult : Handler<WorldServer>
    {
        public GameResult(WorldServer server) : base(server)
        {
        }

        public override int Id => 0x12; //18

        public override void Handle(EzClient client, EzPacket packet)
        {
            Score score = new Score();
            score.AccountId = client.Account.Id;
            score.GameId = client.Game.Id;
            score.SongId = client.Room.Info.SelectedSong;
            score.Created = DateTime.Now;
            score.Difficulty = client.Room.Info.Difficulty;
            score.FadeEffect = client.Room.Info.FadeEffect;
            score.NoteEffect = client.Room.Info.NoteEffect;
            score.Slot = client.Player.Slot;
            score.Team = client.Player.Team;

            byte unknown0 = packet.Data.ReadByte();
            score.StageClear = packet.Data.ReadByte() == 0;
            short unknown1 = packet.Data.ReadInt16(Endianness.Big);
            score.MaxCombo = packet.Data.ReadInt16(Endianness.Big);
            score.Kool = packet.Data.ReadInt16(Endianness.Big);
            score.Cool = packet.Data.ReadInt16(Endianness.Big);
            score.Good = packet.Data.ReadInt16(Endianness.Big);
            score.Miss = packet.Data.ReadInt16(Endianness.Big);
            score.Fail = packet.Data.ReadInt16(Endianness.Big);
            short unknown2 = packet.Data.ReadInt16(Endianness.Big);
            score.RawScore = packet.Data.ReadInt32(Endianness.Big);
            score.TotalNotes = packet.Data.ReadInt16(Endianness.Big);
            score.Rank = (ScoreRankType) packet.Data.ReadByte();
            byte unknown3 = packet.Data.ReadByte();
            score.ComboType = Score.GetComboType(score);

            _logger.Debug("StageClear: {0}", score.StageClear);
            _logger.Debug("MaxCombo: {0}", score.MaxCombo);
            _logger.Debug("Kool: {0}", score.Kool);
            _logger.Debug("Cool: {0}", score.Cool);
            _logger.Debug("Good: {0}", score.Good);
            _logger.Debug("Miss: {0}", score.Miss);
            _logger.Debug("Fail: {0}", score.Fail);
            _logger.Debug("RawScore: {0}", score.RawScore);
            _logger.Debug("TotalScore: {0}", score.TotalScore);
            _logger.Debug("Rank: {0}", score.Rank);
            _logger.Debug("Total Notes: {0}", score.TotalNotes);
            _logger.Debug("Unknown0: {0}", unknown0);
            _logger.Debug("Unknown1: {0}", unknown1);
            _logger.Debug("Unknown2: {0}", unknown2);
            _logger.Debug("Unknown3: {0}", unknown3);

            client.Player.Playing = false;
            client.Score = score;
/*
            //Play check
            IBuffer player4 = EzServer.Buffer.Provide();
            player4.WriteByte(0);

            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(100);

            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(6); //MAX COMBO

            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(7); //SCORE

            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(0); //?

            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(0); //?

            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(0); //?

            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(0);
            player4.WriteByte(0); //?
            Send(client, 0x19, player4);
            */

            if (!Database.InsertScore(score))
            {
                _logger.Error("Could't save score for: {0}", client.Character.Name);
            }

            if (!client.Room.Finished())
            {
                // Last player finish will be responsible for going back to room.
                // TODO let the server check periodically incase the last person disconnectes.
                return;
            }

            List<EzClient> clients = client.Room.GetClients();
            IBuffer scorePacket = ScorePacket.Create(clients);
            Send(client.Room, 0x1B, scorePacket); //27

            Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith(t =>
            {
                // Display Room after 10 seconds
                IBuffer buffer = EzServer.Buffer.Provide();
                buffer.WriteByte(0);
                Send(client.Room, 0x1C, buffer); //28
            });
        }
    }
}