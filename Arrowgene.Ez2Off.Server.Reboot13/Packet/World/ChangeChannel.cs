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

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.World
{
    public class ChangeChannel : Handler<EzServer>
    {
        public ChangeChannel(EzServer server) : base(server)
        {
        }

        public override int Id => 3; //로비에서 채널 선택

        public override void Handle(EzClient client, EzPacket packet)
        {
            IBuffer response1 = EzServer.Buffer.Provide();
            response1.WriteByte(0x0A); //Number of channels Max 0A(10)
            response1.WriteByte(0);
            response1.WriteByte(0x05); //channels 1-1 Current Users
            response1.WriteByte(0);
            response1.WriteByte(0x15); //channels 1-2 Current Users
            response1.WriteByte(0);
            response1.WriteByte(0x20); //channels 1-3 Current Users
            response1.WriteByte(0);
            response1.WriteByte(0x25); //channels 1-4 Current Users
            response1.WriteByte(0);
            response1.WriteByte(0x30); //channels 1-5 Current Users
            response1.WriteByte(0);
            response1.WriteByte(0x35); //channels 1-6 Current Users
            response1.WriteByte(0);
            response1.WriteByte(0x40); //channels 1-7 Current Users
            response1.WriteByte(0);
            response1.WriteByte(0x45); //channels 1-8 Current Users
            response1.WriteByte(0);
            response1.WriteByte(0x50); //channels 1-9 Current Users
            response1.WriteByte(0);
            response1.WriteByte(0x55); //channels 1-10 Current Users
            response1.WriteByte(0);
            Router.Send(client, 0, response1);
        }
    }
}