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
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.Builder
{
    public class LobbyPacket
    {
        public static IBuffer CreateCharacterList(Channel channel)
        {
            List<EzClient> clients = channel.GetLobbyClients();
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt16((short) clients.Count, Endianness.Big);
            for (int i = 0; i < clients.Count; i++)
            {
                EzClient client = clients[i];
                buffer.WriteInt16(client.ChannelIndex, Endianness.Big);
                buffer.WriteFixedString(client.Character.Name, 17, Utils.KoreanEncoding);
                buffer.WriteByte((byte) client.Character.Sex);
                buffer.WriteByte(client.Character.Level);
            }

            return buffer;
        }

        public static IBuffer CreateCharacterListAdd(EzClient client)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32(client.ChannelIndex);
            buffer.WriteFixedString(client.Character.Name, 17, Utils.KoreanEncoding);
            ///////////////padding
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            buffer.WriteByte(0);
            //////////////////
            buffer.WriteInt16((byte) client.Character.Sex);
            buffer.WriteInt16(client.Character.Level);
            return buffer;
        }

        public static IBuffer CreateCharacterListRemove(EzClient client)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt16(client.ChannelIndex, Endianness.Big);
            buffer.WriteFixedString(client.Character.Name, 17, Utils.KoreanEncoding);
            return buffer;
        }

        public static IBuffer CreateGoToLobby(short clientChannelIDX,byte channelId)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte(1);
            buffer.WriteByte(0);
            buffer.WriteInt16(clientChannelIDX);
            buffer.WriteByte(channelId); //1-xCH / 0=1ch 1=2ch 2=3ch 3=4ch
            return buffer;
        }

        public static IBuffer CreateFriendList(List<Friend> friends, ClientLookup lookup)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32(friends.Count);
            foreach (Friend friend in friends)
            {
                EzClient client = lookup.GetClient(friend.FriendCharacterName);
                if (client == null)
                {
                    friend.Online = false;
                }
                else
                {
                    friend.Online = true;
                    friend.Mode = client.Mode;
                    friend.ChannelId = (short) client.Channel.Info.Id;
                }

                WriteFriend(buffer, friend.FriendCharacterName, (short) friend.Sex, (short) friend.Mode,
                    friend.ChannelId,
                    friend.Online);
            }

            return buffer;
        }


        public static void WriteFriend(IBuffer buffer, Friend friend, ClientLookup lookup, EzClient friendClient = null)
        {
            if (friendClient == null)
            {
                friendClient = lookup.GetClient(friend.FriendCharacterName);
            }

            if (friendClient == null)
            {
                friend.Online = false;
            }
            else
            {
                friend.Online = true;
                friend.Mode = friendClient.Mode;
                friend.ChannelId = (short) friendClient.Channel.Info.Id;
            }

            WriteFriend(buffer, friend.FriendCharacterName, (short) friend.Sex, (short) friend.Mode, friend.ChannelId,
                friend.Online);
        }

        public static void WriteFriend(IBuffer buffer, string name, short sex, short mode, short channelId, bool online)
        {
            channelId += 1;
            buffer.WriteFixedString(name, 18, Utils.KoreanEncoding);
            buffer.WriteInt16(sex);
            buffer.WriteInt16(mode);
            buffer.WriteInt16(channelId);
            buffer.WriteInt32(online ? 1 : 0);
        }
    }
}
