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


using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.World
{
    public class InventoryGifts : Handler<EzServer>
    {
        public InventoryGifts(EzServer server) : base(server)
        {
        }

        public override int Id => 36; //25 = 내가방 버튼R

        public override void Handle(EzClient client, EzPacket packet)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            GiftItem[] gifts = client.Inventory.GetGiftItems();
            buffer.WriteInt32(Inventory.MaxGifts);
            for (int i = 0; i < Inventory.MaxGifts; i++)
            {
                GiftItem gift = gifts[i];

                if (gift == null)
                {
                    buffer.WriteBytes(new byte[40]);
                }
                else
                {
                    buffer.WriteInt32(0);
                    buffer.WriteFixedString(gift.SenderName, 18, Utils.KoreanEncoding);
                    buffer.WriteInt16((short) gift.ItemId);
                    buffer.WriteInt32(0);
                    buffer.WriteInt32(0);
                    buffer.WriteInt32(0);
                    buffer.WriteInt32(gift.GetExpireDateUnixTime());
                }
            }

            Router.Send(client, 50, buffer); //36 = 가방 열림,목록
        }
    }
}