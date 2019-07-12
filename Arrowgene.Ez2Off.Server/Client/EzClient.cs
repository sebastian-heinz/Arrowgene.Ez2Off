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

using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Services.Networking.Tcp;
using Arrowgene.Services.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.Ez2Off.Server.Client
{
    public class EzClient
    {
        private readonly PacketBuilder _packetBuilder;

        public EzClient(ITcpSocket clientSocket)
        {
            _packetBuilder = new PacketBuilder();

            if (Socket is AsyncEventClient)
            {
                AsyncEventClient client = (AsyncEventClient) Socket;
                Identity = client.Socket.RemoteEndPoint.ToString();
            }
            else
            {
                Identity = GetHashCode().ToString();
            }

            Socket = clientSocket;
            Player = new Player();
        }

        public ITcpSocket Socket { get; }
        public string Identity { get; }
        public Session Session { get; set; }
        public Account Account => Session.Account;
        public Character Character => Session.Character;
        public Setting Setting => Session.Setting;
        public ModeType Mode => Session.Mode;
        public Inventory Inventory => Session.Inventory;
        public ServerPoint WorldServer { get; set; }
        public Channel Channel { get; set; }
        public Room Room { get; set; }
        public Player Player { get; set; }
        public Score Score { get; set; }
        public Game Game { get; set; }

        public EzPacket Read(byte[] data)
        {
            return _packetBuilder.Read(data);
        }

        public void Send(EzPacket packet)
        {
            byte[] data = packet.ToData().GetAllBytes();
            Socket.Send(data);
        }
    }
}