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

using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Solista.Packets.Login
{
    /// <summary>
    /// A client connected to the login server
    /// </summary>
    public class LoginRequest : Handler<LoginServer>
    {
        public LoginRequest(LoginServer server) : base(server)
        {
        }

        public override int Id => 0;

        public override void Handle(EzClient client, EzPacket packet)
        {
            byte[] paramOne = packet.Data.ReadBytes(17);
            byte[] paramTwo = packet.Data.ReadBytes(17);
            byte[] paramVersion = packet.Data.ReadBytes(4);

            byte[] paramOneDecrypt = Utils.DecryptParameter(paramOne, Utils.KeyFirstParameter);
            byte[] paramTwoDecrypt = Utils.DecryptParameter(paramTwo, Utils.KeySecondParameter);

            string one = Utils.ParameterToString(paramOneDecrypt);
            string two = Utils.ParameterToString(paramTwoDecrypt);
            string version = Utils.ParameterToString(paramVersion);


            _logger.Debug("Client {0} Login (params: one:{1} two:{2}) Version:{3}", client.Identity, one, two, version);

            IBuffer response = EzServer.Buffer.Provide();
            response.WriteByte(1);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0x14);
            Send(client, 0, response);

            IBuffer response1 = EzServer.Buffer.Provide();
            response1.WriteByte(1);
            response1.WriteByte(0x30);
            response1.WriteByte(0x32);
            response1.WriteByte(0x2d);
            response1.WriteByte(0x30);
            response1.WriteByte(0x38);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0x69);
            response1.WriteByte(0x65);
            response1.WriteByte(0x6e);
            response1.WriteByte(0);
            response1.WriteByte(0x72);
            response1.WriteByte(0x47);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(1);
            response1.WriteByte(0);
            response1.WriteByte(0x3d);
            response1.WriteByte(0);
            response1.WriteByte(0x21);
            response1.WriteByte(0);
            response1.WriteByte(0x38);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0x85);
            response1.WriteByte(0);
            response1.WriteByte(0x89);
            response1.WriteByte(0);
            response1.WriteByte(0x8d);
            response1.WriteByte(0);
            response1.WriteByte(0x94);
            response1.WriteByte(1);
            response1.WriteByte(0);
            response1.WriteByte(0x7c);
            response1.WriteByte(0);
            response1.WriteByte(4);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(2);
            response1.WriteByte(0xb5);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(9);
            response1.WriteByte(0xc4);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0x27);
            response1.WriteByte(0x10);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0x27);
            response1.WriteByte(0x10);
            response1.WriteByte(0x0d);
            response1.WriteByte(0x0e);
            response1.WriteByte(0x0f);
            response1.WriteByte(0x10);
            response1.WriteByte(0x13);
            response1.WriteByte(0x14);
            response1.WriteByte(0x15);
            response1.WriteByte(0x16);
            response1.WriteByte(0x24);
            response1.WriteByte(0x0d);
            response1.WriteByte(0x0a);
            response1.WriteByte(0x0c);
            response1.WriteByte(1);
            Send(client, 1, response1);
        }
    }
}