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

using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class MusicBuyLicense : Handler<EzServer>
    {
        public MusicBuyLicense(EzServer server) : base(server)
        {
        }

        public override int Id => 31;

        public override void Handle(EzClient client, EzPacket packet)
        {
            int songid = packet.Data.ReadInt32();

            // TODO

            //IBuffer response = EzServer.Buffer.Provide();
            //response.WriteByte(0);
            //Router.Send(client, ?, response);

            /* Music Lisence Gain Update
            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32(0);// 0=Success 3=Inventory full
            buffer.WriteInt32(songid);//songid
            buffer.WriteInt32(character.Coin);//coin
            Router.Send(client, 43, buffer);
            */
        }
    }
}