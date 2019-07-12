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

using System.Collections.Generic;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Server.Bridge;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Networking.ServerBridge.Messages;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.World
{
    public class LobbyEnter : Handler<WorldServer>
    {
        public LobbyEnter(WorldServer server) : base(server)
        {
        }

        public override int Id => 1;

        public override void Handle(EzClient client, EzPacket packet)
        {
            byte[] paramOne = packet.Data.ReadBytes(17);
            byte[] paramOneDecrypt = Utils.DecryptParameter(paramOne, Utils.KeyFirstParameter);
            string one = Utils.ParameterToString(paramOneDecrypt);

            // TODO check if session exist locally
            _logger.Debug("Requestion Session from Login Server for Client ({0}) with SessionKey: {1})",
                client.Identity, one);
            Bridge.Request<Session, EzClient, EzPacket>(Server.LoginServerPoint.Bridge.ToIpEndPoint(),
                new Request<string>(SessionHandler.Id, one), OnSessionResponse, client, packet);
        }

        private void OnSessionResponse(Response<Session> result, EzClient client, EzPacket packet)
        {
            _logger.Debug("Client ({0}) entered world (SessionKey: {1})", client.Identity, result.Result.Key);


            client.Session = result.Result;

            Channel channel = Server.GetChannel(client.Mode, client.Session.ChannelId);
            channel.Join(client);

            IBuffer response = EzServer.Buffer.Provide();
            response.WriteByte(1);
            response.WriteByte(0);
            response.WriteByte(7); //0A?
            response.WriteByte(0);
            response.WriteByte((byte) client.Mode); // Mode 0=RubyMix  1=STREET MIX  2=CLUB MIX
            response.WriteByte(0);
            response.WriteByte((byte) client.Session.ChannelId); //1-xCH / 0=1ch 1=2ch 2=3ch 3=4ch
            response.WriteByte(0);
            Send(client, 1, response);

            IBuffer characterList = LobbyCharacterListPacket.Create(channel);
            Send(channel.GetLobbyClients(), 2, characterList);

            IBuffer announceRoomPacket = RoomPacket.CreateAnnounceRoomPacket(channel);
            Send(client.Channel.GetLobbyClients(), 13, announceRoomPacket);

            IBuffer applyInventoryPacket = InventoryPacket.ShowInventoryPacket(client.Inventory);
            Send(client, 0x21, applyInventoryPacket);

            IBuffer djPointsPacket = SongPacket.CreateDjPointsPacket();
            Send(client, 0x2B, djPointsPacket); //43

            IBuffer settings = SettingsPacket.Create(client.Setting, client.Mode);
            Send(client, 0x2D, settings); //45
        }
    }
}