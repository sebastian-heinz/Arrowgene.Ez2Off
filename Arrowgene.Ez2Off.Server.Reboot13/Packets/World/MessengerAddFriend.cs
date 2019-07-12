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

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.World
{
    public class MessengerAddFriend : Handler<WorldServer>
    {
        public MessengerAddFriend(WorldServer server) : base(server)
        {
        }

        public override int Id => 0x26;

        public override void Handle(EzClient client, EzPacket packet)
        {
            //
            //  ASCII:.  .  .  .  .  .  .  a  s  d  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .
            //    HEX:26-00-28-00-00-00-00- 61-73-64-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00

            string characterName = packet.Data.ReadFixedString(14, Utils.KoreanEncoding);
            
            _logger.Debug("Add Character: {0}", characterName);


            IBuffer buffer = EzServer.Buffer.Provide();

            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            buffer.WriteByte(1);
            
            
        //    Send(client, 0x26, buffer);

        //    Send(client, 0x30, buffer);
            
       //     Send(client, 0x31, buffer);
            
      // discon      Send(client, 0x32, buffer);
            
    // Send(client, 0x33, buffer);
    
     Send(client, 0x30, buffer);

        }
    }
}