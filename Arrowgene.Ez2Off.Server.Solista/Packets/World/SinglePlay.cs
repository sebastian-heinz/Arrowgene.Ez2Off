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
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Solista.Packets.World
{
    public class SinglePlay : Handler<WorldServer>
    {
        public SinglePlay(WorldServer server) : base(server)
        {
        }

        public override int Id => 5;

        public override void Handle(EzClient client, EzPacket packet)
        {
         IBuffer response = EzServer.Buffer.Provide();
            response.WriteByte(1);//1
            response.WriteByte(0);
            response.WriteByte(0x6a);//6a
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0xff);//ff
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);//1
            response.WriteByte(0);//1
            response.WriteByte(8);//8
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(1);//1
            response.WriteByte(1);//1
            response.WriteByte(0XB0);
            response.WriteByte(0XB0);
            response.WriteByte(0XC0);
            response.WriteByte(0XCC);
            response.WriteByte(0X20);
            response.WriteByte(0XC7);
            response.WriteByte(0XD2);
            response.WriteByte(0X20);
            response.WriteByte(0XBB);
            response.WriteByte(0XE7);
            response.WriteByte(0XB6);
            response.WriteByte(0XF7);
            response.WriteByte(0X21);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0);
             Send(client, 9, response);
             
            IBuffer response1 = EzServer.Buffer.Provide();
            response1.WriteByte(0);//0
            response1.WriteByte(1);//1
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0X30);//30
            response1.WriteByte(0X32);//32
            response1.WriteByte(0X2D);//2d
            response1.WriteByte(0X31);//31
            response1.WriteByte(0X31);//31
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0X69);//69
            response1.WriteByte(0X65);//65
            response1.WriteByte(0X6E);//6e
            response1.WriteByte(0);
            response1.WriteByte(0X72);//72
            response1.WriteByte(0X47);//47
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(1);//1
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(5);//5
            response1.WriteByte(0);
            response1.WriteByte(0X7C);//7c
            response1.WriteByte(0);
            response1.WriteByte(4);//4
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(3);//3
            response1.WriteByte(0X35);//35
            response1.WriteByte(0XA5);//a5
            response1.WriteByte(0);
            response1.WriteByte(2);//2
            response1.WriteByte(0X2A);//2a
            response1.WriteByte(0X0A);//0a
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteByte(2);//2
            response1.WriteByte(0xd0);//d0
            response1.WriteByte(0);
            response1.WriteByte(0);
            Send(client, 0x0A, response1);
        }
    }
}