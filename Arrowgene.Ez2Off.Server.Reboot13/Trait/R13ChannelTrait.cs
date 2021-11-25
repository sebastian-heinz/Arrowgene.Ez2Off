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

using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Reboot13.Packet.Builder;
using Arrowgene.Ez2Off.Server.Trait;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Trait
{
    public class R13ChannelTrait : ChannelTrait
    {
        public R13ChannelTrait(EzServer server) : base(server)
        {
        }

        public override void ClientJoin(Channel channel, EzClient client)
        {
            IBuffer response = EzServer.Buffer.Provide();
            response.WriteByte(1);
            response.WriteByte(0);
            response.WriteInt16(client.ChannelIndex); //0A?
            response.WriteByte((byte) client.Mode); // Mode 0=RubyMix  1=STREET MIX  2=CLUB MIX
            response.WriteByte(0);
            response.WriteByte((byte) client.Session.ChannelId); //1-xCH / 0=1ch 1=2ch 2=3ch 3=4ch
            response.WriteByte(0);
            Router.Send(client, 1, response);

            IBuffer characterList = LobbyPacket.CreateCharacterList(channel);
            Router.Send(client, 2, characterList);

            IBuffer characterListAdd = LobbyPacket.CreateCharacterListAdd(client);
            Router.Send(channel.GetLobbyClients(), 3, characterListAdd, client);

            Router.Send(client, 13, RoomPacket.RoomList(channel));

            IBuffer applyInventoryPacket = InventoryPacket.ShowInventoryPacket(client.Inventory);
            Router.Send(client, 33, applyInventoryPacket);

            IBuffer djPointsPacket = SongPacket.CreateDjPointsPacket();
            Router.Send(client, 43, djPointsPacket);

            IBuffer settings = SettingsPacket.Create(client.Setting, client.Mode);
            Router.Send(client, 45, settings);

            Router.Send(client, 53, LobbyPacket.CreateFriendList(client.Friends.GetAll(), Clients));

            List<EzClient> friendedMe = Clients.GetFriendedMe(client);
            foreach (EzClient friendMeClient in friendedMe)
            {
                IBuffer friendUpdate = EzServer.Buffer.Provide();
                Friend friend = friendMeClient.Friends.Get(client.Character.Id);
                LobbyPacket.WriteFriend(friendUpdate, friend, Clients, client);
                Router.Send(friendMeClient, 57, friendUpdate);
            }
        }

        public override void ClientLeave(Channel channel, EzClient client)
        {
            IBuffer characterListRemove = LobbyPacket.CreateCharacterListRemove(client);
            Router.Send(channel.GetLobbyClients(), 4, characterListRemove);
        }

        public override void CloseRoom(Channel channel, Room room)
        {
            Router.Send(channel.GetLobbyClients(), 14, RoomPacket.RoomClosed(room));
        }

        public override void CreateRoom(Channel channel, Room room, EzClient client)
        {
            Router.Send(client, 9, RoomPacket.CreateOpenRoomPacket(client.Room, client));
            //Router.Send(client.Channel.GetLobbyClients(), 2, LobbyPacket.CreateCharacterList(client.Channel));
            IBuffer characterListRemove = LobbyPacket.CreateCharacterListRemove(client);
            Router.Send(client.Channel.GetLobbyClients(), 4, characterListRemove);
            Router.Send(client, 10, RoomPacket.CreateCharacterPacket(client.Room));
            Router.Send(client.Channel.GetLobbyClients(), 14, RoomPacket.UpdateRoomStatus(client.Room));
        }
    }
}
