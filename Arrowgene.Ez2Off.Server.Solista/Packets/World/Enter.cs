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

namespace Arrowgene.Ez2Off.Server.Solista.Packets.World
{
    /// <summary>
    /// A client connected to the world server
    /// </summary>
    public class Enter : Handler<WorldServer>
    {
        public Enter(WorldServer server) : base(server)
        {
        }

        public override int Id => 1;

        public override void Handle(EzClient client, EzPacket packet)
        {
            byte[] paramOne = packet.Data.ReadBytes(17);
            byte[] paramOneDecrypt = Utils.DecryptParameter(paramOne, Utils.KeyFirstParameter);
            string one = Utils.ParameterToString(paramOneDecrypt);

            _logger.Debug("Client {0} Entered World (SessionKey:{1})", client.Identity, one);

            //01 00 08 00 00 00 00 // ID = 1 Size =0x08 = 8
            //
            //01 00 10 00 01 00 07 00  Data size = 8
            IBuffer response = EzServer.Buffer.Provide();
            response.WriteByte(1);
            response.WriteByte(0);
            response.WriteByte(0x10);
            response.WriteByte(0);
            response.WriteByte(1);
            response.WriteByte(0);
            response.WriteByte(7);
            response.WriteByte(0);
            Send(client, 1, response);
            
            
            //21 00 01 00 00 00 00  // ID = 0x21 = 33; Size = 1;
            //
            //00  Data size =1
            IBuffer response1 = EzServer.Buffer.Provide();
            response.WriteByte(0);
            Send(client, 33, response1);

            IBuffer response2 = EzServer.Buffer.Provide();
            response2.WriteByte(0); 
            response2.WriteByte(1);

            response2.WriteByte(0);
            response2.WriteByte(3);
            response2.WriteByte(0X65);
            response2.WriteByte(0X7A);
            response2.WriteByte(0X63);
            response2.WriteByte(0X72);
            response2.WriteByte(0X62);
            response2.WriteByte(0X65);
            response2.WriteByte(0X65);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(0);

            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(1);
            response2.WriteByte(1);
            response2.WriteByte(0X0D);
            response2.WriteByte(0);
            response2.WriteByte(0X10);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(7);
            response2.WriteByte(0);
            response2.WriteByte(0);

            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(1);
            response2.WriteByte(1);
            response2.WriteByte(1);
            response2.WriteByte(1);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(0);
            response2.WriteByte(7);
            response2.WriteByte(2);
            Send(client, 2, response2);
        }
    }
}