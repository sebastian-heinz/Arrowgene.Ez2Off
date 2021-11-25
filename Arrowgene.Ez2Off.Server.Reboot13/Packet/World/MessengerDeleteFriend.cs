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
using Arrowgene.Ez2Off.Server.Reboot13.Packet.Builder;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.World
{
    public class MessengerDeleteFriend : Handler<EzServer>
    {
        public MessengerDeleteFriend(EzServer server) : base(server)
        {
        }

        public override int Id => 39;

        public override void Handle(EzClient client, EzPacket packet)
        {
            string characterName = packet.Data.ReadFixedString(18, Utils.KoreanEncoding);
            Logger.Debug(client, $"Delete Character: {characterName}");

            Character character = Server.GetCharacter(characterName);
            if (character == null)
            {
                Logger.Error(client, $"Character: {characterName} doesn't exist");
                return;
            }

            Friend friend = client.Friends.Get(characterName);
            if (friend == null)
            {
                Logger.Error(client, $"Friend Character: {characterName} doesn't exist");
                return;
            }

            if (!Database.DeleteFriend(friend.Id))
            {
                Logger.Error(client, $"Couldn't delete FriendId: {friend.Id} Character: {characterName}");
            }

            client.Friends.Remove(friend);

            // TODO find single update id ?
            Router.Send(client, 53, LobbyPacket.CreateFriendList(client.Friends.GetAll(), Server.Clients));
        }
    }
}