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
using System.Collections.Generic;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder;
using Arrowgene.Ez2Off.Server.Trait;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Trait
{
    public class R14RoomTrait : RoomTrait
    {
        public R14RoomTrait(EzServer server) : base(server)
        {
        }

        public override void ClientLeave(Room room, EzClient client)
        {
            Router.Send(client, 5,
                PacketBuilder.LobbyPacket.CreateGoToLobby(client.ChannelIndex, (byte) client.Session.ChannelId));
            Router.Send(room, 12, PacketBuilder.RoomPacket.LeavePlayer((byte) client.Player.Slot));
            Router.Send(client, 13, PacketBuilder.RoomPacket.RoomList(client.Channel));
            Router.Send(client.Channel.GetLobbyClients(), 14, PacketBuilder.RoomPacket.UpdateRoomStatus(room), client);

            Router.Send(client, 2, PacketBuilder.LobbyPacket.CreateCharacterList(client.Channel));
            IBuffer characterListAdd = PacketBuilder.LobbyPacket.CreateCharacterListAdd(client);
            Router.Send(client.Channel.GetLobbyClients(), 3, characterListAdd, client);
        }

        public override void ClientJoin(Room room, EzClient client)
        {
            Router.Send(client, 8, PacketBuilder.RoomPacket.CreateJoinRoomPacket(room, client));
            Router.Send(room, 11, PacketBuilder.RoomPacket.AnnounceJoin(client), client);
            Router.Send(client, 10, PacketBuilder.RoomPacket.CreateCharacterPacket(room));
            Router.Send(client.Channel.GetLobbyClients(), 14, PacketBuilder.RoomPacket.UpdateRoomStatus(client.Room));

            IBuffer characterListRemove = PacketBuilder.LobbyPacket.CreateCharacterListRemove(client);
            Router.Send(client.Channel.GetLobbyClients(), 4, characterListRemove);
        }

        public override void NewMaster(Room room, EzClient client)
        {
            Router.Send(room, 15, PacketBuilder.RoomPacket.NewMaster((byte) room.Master.Player.Slot));
        }

        public override void Kick(Room room, EzClient client, byte playerSlot)
        {
            Router.Send(room, 22, PacketBuilder.RoomPacket.KickPlayer(playerSlot));
        }


        public override void GameStarting(Room room)
        {
        }

        public override void GameStart(Room room)
        {
            if (room.Game == null || room.Game.Song == null)
            {
                Router.Send(room, 23, PacketBuilder.GamePacket.CreateGameStart(null, room.Mode, room.Difficulty));
                return;
            }

            foreach (EzClient client in room.GetClients())
            {
                IBuffer skinTypeSave = EzServer.Buffer.Provide();
                skinTypeSave.WriteInt32(client.Player.Slot);
                skinTypeSave.WriteByte(client.Setting.SkinType);
                Router.Send(room, 70, skinTypeSave);
            }

            if (room.Game is RadiomixGame radiomixGame)
            {
                if (radiomixGame.Index == 0)
                {
                    Router.Send(room.Channel.GetLobbyClients(), 14, PacketBuilder.RoomPacket.UpdateRoomStatus(room));
                }

                Router.Send(room, 23,
                    PacketBuilder.GamePacket.CreateGameStart(radiomixGame.CurrentSong, room.Mode, room.Difficulty));
            }
            else
            {
                Router.Send(room.Channel.GetLobbyClients(), 14, PacketBuilder.RoomPacket.UpdateRoomStatus(room));
                Router.Send(room, 23,
                    PacketBuilder.GamePacket.CreateGameStart(room.Game.Song, room.Mode, room.Difficulty));
            }
        }

        public override void NextRadiomixSong(Room room, EzClient client)
        {
            if (room.Game is RadiomixGame radiomixGame)
            {
                IBuffer buffer = EzServer.Buffer.Provide();
                buffer.WriteInt32(client.Player.Slot);
                buffer.WriteInt32(0);
                buffer.WriteInt32(radiomixGame.NextSongId());
                Router.Send(room, 75, buffer);
            }
        }

        public override void GameResult(Room room)
        {
            List<EzClient> clients = room.GetClients();
            foreach (EzClient client in clients)
            {
                Score score = client.Score;
                if (score != null)
                {
                    score.Incident = client.Audit.Incident;
                }
            }

            IBuffer scorePacket = PacketBuilder.GamePacket.CreateScore(room);
            Router.Send(room, 27, scorePacket);

            if (!Server.Database.InsertGame(room.Game))
            {
                Logger.Error("Could not save game");
            }

            foreach (EzClient client in clients)
            {
                Score score = client.Score;
                if (score != null && client.Rank != null)
                {
                    if (!Server.Database.InsertScore(score))
                    {
                        Logger.Error(client, "Couldn't save score");
                        continue;
                    }

                    if (!Server.Database.InsertRank(client.Rank))
                    {
                        Logger.Error(client, "Couldn't save rank");
                    }

                    if (client.Audit.Incident)
                    {
                        // Incidents
                        if (client.Audit.ScoreIncident != null)
                        {
                            if (!Server.Database.InsertScoreIncident(client.Audit.ScoreIncident, score.Id))
                            {
                                Logger.Error(client, "Score Manipulation: Failed to insert incident.");
                            }
                        }

                        if (client.Audit.ComboIncident != null)
                        {
                            if (!Server.Database.InsertScoreIncident(client.Audit.ComboIncident, score.Id))
                            {
                                Logger.Error(client, "Combo Manipulation: Failed to insert incident.");
                            }
                        }

                        if (client.Audit.AllComboIncident != null)
                        {
                            if (!Server.Database.InsertScoreIncident(client.Audit.AllComboIncident, score.Id))
                            {
                                Logger.Error(client, "AllCombo Manipulation: Failed to insert incident.");
                            }
                        }

                        client.Audit.Reset();
                    }
                }
            }
        }
        public override void ClientFinish(Room room, EzClient client)
        {
            Score score = client.Score;
            NoteHistory noteHistory = client.NoteHistory;
            Game game = room.Game;
            if (!score.StageClear)
            {
                Router.Send(client.Room,
                    26,
                    PacketBuilder.GamePacket.CreateGameOver((byte) client.Player.Slot),
                    client
                );
            }

            noteHistory.Calculate();

            int scoreTolerance = 5000;
            if (!Utils.InRange(
                noteHistory.Score - scoreTolerance,
                noteHistory.Score + scoreTolerance,
                score.RawScore)
            )

            {
                Logger.Error(client,
                    $"Score Manipulation: SongId: {score.Song.Id} Calculated: (Score: {noteHistory.Score} Combo:{noteHistory.MaxCombo} Notes: {noteHistory.NoteCount()}) " +
                    $"Actual: (Score: {score.RawScore} Combo: {score.MaxCombo} Notes: {score.TotalNotes})"
                );
                Incident scoreIncident = new Incident();
                scoreIncident.Type = 1;
                scoreIncident.Created = DateTime.Now;
                scoreIncident.Description = "Score Manipulation";
                scoreIncident.AccountId = client.Account.Id;
                client.Audit.ScoreIncident = scoreIncident;
                client.Audit.Incident = true;
            } else if (noteHistory.Score != score.RawScore)
            {
                Logger.Debug(client,
                    $"Score Calculation Issue - SongId: {score.Song.Id} Calculated: (Score: {noteHistory.Score} Combo:{noteHistory.MaxCombo} Notes: {noteHistory.NoteCount()}) " +
                    $"Actual: (Score: {score.RawScore} Combo: {score.MaxCombo} Notes: {score.TotalNotes})");
            }
            
            if (noteHistory.MaxCombo != score.MaxCombo)
            {
                Logger.Error(client,
                    $"Combo Manipulation: SongId: {score.Song.Id} Calculated: (Score: {noteHistory.Score} Combo:{noteHistory.MaxCombo} Notes: {noteHistory.NoteCount()}) " +
                    $"Actual: (Score: {score.RawScore} Combo: {score.MaxCombo} Notes: {score.TotalNotes})"
                );
                Incident comboIncident = new Incident();
                comboIncident.Type = 2;
                comboIncident.Created = DateTime.Now;
                comboIncident.Description = "Combo Manipulation";
                comboIncident.AccountId = client.Account.Id;
                client.Audit.ComboIncident = comboIncident;
                client.Audit.Incident = true;
            }

            SongDetail songDetail = score.Song.GetSongDetail(score.Mode, score.Difficulty);
            if (!(game is RadiomixGame)
                && score.ComboType > ComboType.None
                && noteHistory.MaxCombo != songDetail.Notes
            )
            {
                Logger.Error(client,
                    $"AllCombo Manipulation: ComboType: {score.ComboType}  History Combo: {noteHistory.MaxCombo} Song Notes: {songDetail.Notes}");
                //Incident allComboIncident = new Incident();
                //allComboIncident.Type = 3;
                //allComboIncident.Created = DateTime.Now;
                //allComboIncident.Description = "AllCombo Manipulation";
                //allComboIncident.AccountId = client.Account.Id;
                //client.Audit.AllComboIncident = allComboIncident;
                //client.Audit.Incident = true;
            }

            if (!(game is RadiomixGame))
            {
               NoteHistory maxScore = NoteHistory.MaxScoreHistory(songDetail.Notes);
               if (score.RawScore > maxScore.Score)
               {
                   Logger.Error(client,
                       $"Score Manipulation: RawScore: {score.RawScore} is higher than maximum possible calculated Score: {maxScore.Score}");
                  //Incident scoreIncident = new Incident();
                  //scoreIncident.Type = 8;
                  //scoreIncident.Created = DateTime.Now;
                  //scoreIncident.Description = "Score Manipulation - higher than possible";
                  //scoreIncident.AccountId = client.Account.Id;
                  //client.Audit.ScoreIncident = scoreIncident;
                  //client.Audit.Incident = true;
               }
            }
        }
        public override void GameFinish(Room room)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            Router.Send(room, 28, buffer);
            Router.Send(room.Channel.GetLobbyClients(), 14, PacketBuilder.RoomPacket.UpdateRoomStatus(room));

            foreach (EzClient client in room.GetClients())
            {
                if (client != null && client.Inventory.ItemsChanged)
                {
                    IBuffer showInventoryPacket =
                        PacketBuilder.InventoryPacket.ShowInventoryPacket(client.Inventory);
                    Router.Send(client, 30, showInventoryPacket);
                    if (client.Room != null)
                    {
                        Router.Send(client.Room, 31,
                            PacketBuilder.RoomPacket.ItemUpdate((byte) client.Player.Slot,
                                client.Inventory));
                    }

                    client.Inventory.ItemsChanged = false;
                }
            }
        }

        public override void LoadingFinish(Room room)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(0);
            Router.Send(room, 24, buffer);
        }

        public override void ChangeGameSetting(Room room, short playerSlot, RoomOptionType roomOption, int valueA = 0,
            int valueB = 0)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt16(playerSlot, Endianness.Big);
            buffer.WriteInt32((int) roomOption);
            buffer.WriteInt32(valueA);
            buffer.WriteInt32(valueB);

            Router.Send(room, 16, buffer);
            Router.Send(room.Channel.GetLobbyClients(), 14, PacketBuilder.RoomPacket.UpdateRoomStatus(room));
        }
    }
}