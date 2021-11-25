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
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class RoomInvite : Handler<EzServer>
    {
        public RoomInvite(EzServer server) : base(server)
        {
        }

        public override int Id => 28;

        public override int ExpectedSize => 18;

        public override void Handle(EzClient client, EzPacket packet)
        {
            byte action = packet.Data.ReadByte();
            string characterName = packet.Data.ReadFixedString(18, Utils.KoreanEncoding);

            EzClient player;
            if (action == 1)
            {
                // Invite Random
                List<EzClient> clients = client.Channel.GetLobbyClients();
                int count = clients.Count;
                if (count <= 0)
                {
                    Logger.Debug(client, "Nobody in channel to invite");
                    IBuffer buffer1 = EzServer.Buffer.Provide();
                    buffer1.WriteByte(0);
                    Router.Send(client, 40, buffer1);
                    return;
                }

                if (count == 1)
                {
                    player = clients[0];
                }
                else
                {
                    int randomIndex = Utils.Random.Next(0, count);
                    player = clients[randomIndex];
                }
            }
            else if (action == 2)
            {
                // Invite by Name
                player = client.Channel.GetClient(characterName);
            }
            else
            {
                Logger.Error(client, $"Invalid Action: {action})");
                return;
            }

            if (player == null)
            {
                Logger.Error(client, $"Character: {characterName} not found");
                return;
            }

            Logger.Debug(client, $"Invited Character: {player.Character.Name})");
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteByte((byte) client.Player.Slot);
            buffer.WriteInt16(client.Room.Number, Endianness.Big);
            buffer.WriteFixedString(client.Room.Password, 14, Utils.KoreanEncoding);
            buffer.WriteFixedString(client.Character.Name, 18, Utils.KoreanEncoding);
            buffer.WriteFixedString(player.Character.Name, 18, Utils.KoreanEncoding);
            Router.Send(client, 39, buffer);
            Router.Send(player, 39, buffer);
        }
    }
}