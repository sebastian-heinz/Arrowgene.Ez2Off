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

using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Packet;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.World
{
    public class GameBattleMeter : Handler<WorldServer>
    {
        public GameBattleMeter(WorldServer server) : base(server)
        {
        }

        public override int Id => 17;


        //HEX:11-00-0D-00-00-00-00-  01-16-00-43-01-00-00-00-00-00-00-00-40
        //HEX:11-00-0D-00-00-00-00-  00-15-00-46-01-00-00-00-00-00-00-00-3B
        //HEX:11-00-0D-00-00-00-00-  02-17-00-4A-01-00-00-00-00-00-00-00-36

        public override void Handle(EzClient client, EzPacket received)
        {
            byte a = received.Data.ReadByte();
            byte b = received.Data.ReadByte();
            byte c = received.Data.ReadByte();
            byte d = received.Data.ReadByte();
            byte e = received.Data.ReadByte();
            byte f = received.Data.ReadByte();
            byte g = received.Data.ReadByte();
            byte h = received.Data.ReadByte();
            byte i = received.Data.ReadByte();
            byte j = received.Data.ReadByte();
            byte k = received.Data.ReadByte();
            byte l = received.Data.ReadByte();
            byte m = received.Data.ReadByte();

            _logger.Debug("a:{0} b:{1} c:{2} d:{3} e:{4} f:{5} g:{6} h:{7} i:{8} j:{9} k:{10} l:{11} m:{12}",
                a, b, c, d, e, f, g, h, i, j, k, l, m);
        }
    }
}