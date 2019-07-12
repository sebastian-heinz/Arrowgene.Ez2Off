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

using System.Threading;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.World
{
    public class GameStart : Handler<WorldServer>
    {
        public GameStart(WorldServer server) : base(server)
        {
        }

        public override int Id => 0x0F;//15
        

        public override void Handle(EzClient client, EzPacket packet)
        {

            
            Game game = new Game();
            game.Name = client.Room.Info.Name;
            game.GroupType = client.Room.Info.GameGroupType;
            game.Type = client.Room.Info.GameType;
            if (!Database.InsertGame(game))
            {
                // Oh uh
            }
            
            
            foreach (EzClient c in client.Room.GetClients())
            {
                c.Player.Playing = true;
                c.Game = game;
            }

            
            //Song info
            IBuffer buffer = EzServer.Buffer.Provide();
            //General BYTE / Float
            buffer.WriteByte(1);//Level
            buffer.WriteFloat(1.6f);//MeasureScale
            //JudgmentDelta BYTE
            buffer.WriteByte(8);//Kool
            buffer.WriteByte(24);//Cool
            buffer.WriteByte(60);//Good
            buffer.WriteByte(76);//Miss
            //GaugeUpDownRate Float
            buffer.WriteFloat(0.2f);//Cool
            buffer.WriteFloat(0.1f);//Good
            buffer.WriteFloat(-1.8f);//Miss
            buffer.WriteFloat(-4.8f);//Fail
            buffer.WriteByte(0);
            //
            buffer.WriteByte((byte)client.Room.Info.SelectedSong);//Disc Num 205
            buffer.WriteByte((byte)client.Room.Info.Difficulty);//Select difficulty  0=EZ 1=NM 2=HD 3=SHD
            buffer.WriteByte(1);
            Send(client.Room, 0x17, buffer);//23

        }
    }
}