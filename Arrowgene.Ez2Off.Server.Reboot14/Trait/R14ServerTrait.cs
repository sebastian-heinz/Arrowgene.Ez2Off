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
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder;
using Arrowgene.Ez2Off.Server.Trait;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Trait
{
    public class R14ServerTrait : ServerTrait
    {
        public R14ServerTrait(EzServer server) : base(server)
        {
        }

        public override void ClientDisconnected(EzClient client)
        {
            List<EzClient> friendedMe = Clients.GetFriendedMe(client);
            foreach (EzClient friendMeClient in friendedMe)
            {
                IBuffer friendUpdate = EzServer.Buffer.Provide();
                Friend friend = friendMeClient.Friends.Get(client.Character.Id);

                PacketBuilder.LobbyPacket.WriteFriend(friendUpdate, friend.FriendCharacterName,
                    (short) friend.Mode, friend.ChannelId,
                    false);
                Router.Send(friendMeClient, 56, friendUpdate);
            }
        }
    }
}