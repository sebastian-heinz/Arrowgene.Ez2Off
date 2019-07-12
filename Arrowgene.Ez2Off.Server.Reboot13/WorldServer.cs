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

using System.Reflection;
using Arrowgene.Ez2Off.Server.Bridge;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.World;
using Arrowgene.Ez2Off.Server.Scripting;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13
{
    public class WorldServer : EzWorldServer
    {
        public WorldServer(SettingsContainer settingsContainer, WorldServerSettings settings) : base(settingsContainer,
            settings)
        {
            EzScriptEngine.Instance.AddReference(Assembly.GetAssembly(typeof(WorldServer)));
        }

        public override string Name => "Reboot13 WorldServer";

        protected override void OnClientDisconnected(EzClient client)
        {
            Database.UpsertCharacter(client.Character, client.Account.Id);
            Database.UpsertSetting(client.Setting, client.Account.Id);

            if (client.Room != null)
            {
                Room room = client.Room;
                room.Leave(client);

                IBuffer roomCharacterPacket = RoomPacket.CreateCharacterPacket(room);
                Send(room, 10, roomCharacterPacket);

                IBuffer announceRoomPacket = RoomPacket.CreateAnnounceRoomPacket(client.Channel);
                Send(client.Channel.GetLobbyClients(), 13, announceRoomPacket);
            }

            if (client.Channel != null)
            {
                Channel channel = client.Channel;
                channel.Leave(client);

                IBuffer characterList = LobbyCharacterListPacket.Create(channel);
                Send(channel.GetLobbyClients(), 2, characterList);
            }
        }

        protected override void LoadHandles()
        {
            AddHandler(new RoomEntry(this));
            AddHandler(new InventoryDeleteItem(this));
            AddHandler(new PurchaseDjPoint(this));
            AddHandler(new LobbyEnter(this));
            AddHandler(new InventoryShow(this));
            AddHandler(new InventoryApplyItem(this));
            AddHandler(new BackButton(this));
            AddHandler(new RoomCreate(this));
            AddHandler(new RoomChangeOptions(this));
            AddHandler(new RoomSelectSong(this));
            AddHandler(new GameStart(this));
            AddHandler(new GameSongScores(this));
            AddHandler(new GameLoading(this));
            AddHandler(new GameResult(this));
            AddHandler(new ChangeChannel(this));
            AddHandler(new ChangeChannelSelect(this));
            AddHandler(new SaveSettings(this));
            AddHandler(new LobbyChat(this));
            AddHandler(new PrivateChat(this));
            AddHandler(new Mission(this));
            AddHandler(new PurchaseItem(this));
            AddHandler(new InventoryMoveItem(this));
            AddHandler(new GameBattleMeter(this));
            AddHandler(new Ranking(this));
            AddHandler(new MessengerAddFriend(this));
            AddHandler(new MessengerBox(this));
            AddHandler(new MessengerOption(this));
            Bridge.AddHandler(new ChannelInfoHandler(this));
        }
    }
}