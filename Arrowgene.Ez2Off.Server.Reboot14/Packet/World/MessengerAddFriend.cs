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

using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Id;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class MessengerAddFriend : Handler<EzServer>
    {
        public MessengerAddFriend(EzServer server) : base(server)
        {
        }

        public override int Id => (int) WorldRequestId.FriendAdd;

        public override void Handle(EzClient client, EzPacket packet)
        {
            string characterName = packet.Data.ReadFixedString(18, Utils.KoreanEncoding);
            Logger.Debug(client, $"Add CharacterName: {characterName}");

            if (client.Friends.Count > MessageBox.MaxFriends)
            {
                Logger.Error(client, $"CharacterName: {characterName} can not be added, to many friends");
                Router.Send(
                    client,
                    (byte) WorldResponseId.FriendAdd,
                    PacketBuilder.MessagePacket.AddFriend(characterName, FriendAddMessageType.CanNotAddMoreFriends)
                );
                return;
            }

            Character character = Server.GetCharacter(characterName);
            if (character == null)
            {
                Logger.Error(client, $"CharacterName: {characterName} doesn't exist");
                Router.Send(
                    client,
                    (byte) WorldResponseId.FriendAdd,
                    PacketBuilder.MessagePacket.AddFriend(characterName, FriendAddMessageType.FriendDoesNotExist)
                );
                return;
            }

            if (client.Friends.Get(character.Id) != null)
            {
                Logger.Error(client, $"CharacterName: {characterName} is already your friend");
                Router.Send(
                    client,
                    (byte) WorldResponseId.FriendAdd,
                    PacketBuilder.MessagePacket.AddFriend(characterName, FriendAddMessageType.AlreadyFriend)
                );
                return;
            }

            Friend friend = new Friend();
            friend.CharacerId = client.Character.Id;
            friend.FriendCharacterName = character.Name;
            friend.FriendCharacterId = character.Id;

            if (!Database.InsertFriend(friend))
            {
                Logger.Error(client, $"Couldn't save CharacterName: {characterName} as friend");
            }

            client.Friends.Add(friend);

            Router.Send(
                client,
                (byte) WorldResponseId.FriendAdd,
                PacketBuilder.MessagePacket.AddFriend(characterName, FriendAddMessageType.Success)
            );
            Router.Send(client,
                (byte) WorldResponseId.FriendListShow,
                PacketBuilder.LobbyPacket.CreateFriendList(client.Friends.GetAll(), Server.Clients)
            );
        }
    }
}