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


using System;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.World
{
    public class ShopSendGift : Handler<EzServer>
    {
        public ShopSendGift(EzServer server) : base(server)
        {
        }

        public override int Id => 35;

        public override void Handle(EzClient client, EzPacket packet)
        {
            //HEX:97-01-
            //00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-
            //44-69-65-74-65-72-00-00-00-00-00-00-00-00-00-00-00-00-
            //01-00

            short itemId = packet.Data.ReadInt16();
            packet.Data.ReadBytes(20);
            string receiverCharacterName = packet.Data.ReadFixedString(18, Utils.KoreanEncoding);

            Item item = Database.SelectItem(itemId);

            if (item == null)
            {
                Logger.Error(client, $"ItemId: {itemId} not found in database");
                return;
            }

            if (client.Character.Coin < item.Price)
            {
                Logger.Debug(client, $"Price: {client.Character.Coin} are to less to buy for Price: {item.Price}");
                return;
            }


            Logger.Debug(client, $"Sending ItemId: {itemId} to Character: {receiverCharacterName}");

            GiftItem gift = new GiftItem();
            gift.ItemId = item.Id;
            gift.SenderId = client.Character.Id;
            gift.SenderName = client.Character.Name;
            gift.SendAt = DateTime.Now.AddDays(10);


            EzClient receiverClient = client.Channel.GetClient(receiverCharacterName);
            if (receiverClient != null)
            {
                gift.ReceiverId = receiverClient.Character.Id;
                receiverClient.Inventory.AddGiftItem(gift);
                Logger.Info(client, "Added gift to gift box");
            }
            else
            {
                Character receiverCharacter = Database.SelectCharacter(receiverCharacterName);
                if (receiverCharacter == null)
                {
                    Logger.Error(client, $"Receiver Character: {receiverCharacterName} doesn't exist");
                    return;
                }

                Logger.Info(client, $"Receiver Character: {receiverCharacterName} offline, saved to database");
                gift.ReceiverId = receiverCharacter.Id;
            }

            //  if (!Database.InsertGift(gift))
            //  {
            //      Logger.Error("Could not save gift to database");
            //  }

            IBuffer buffer = EzServer.Buffer.Provide();
            buffer.WriteInt32(1); // message Type
            /*  1 = success send gift item 
                100 = not valid item code
                101 = not found reciver
                102 = not valid item number
                103 = not found reciver(not friend to gift??)
                104 = don't send this item! (unique item?)
                105 = not enough coin
                106 = not enough cash
                999 = db error message
                */
            buffer.WriteInt32(item.Id); // item_number

            buffer.WriteFixedString(item.Name, 18, Utils.KoreanEncoding); // name
            buffer.WriteInt16(0); // padding
            buffer.WriteInt32(client.Character.Coin); //coin
            buffer.WriteInt32(client.Character.Cash); //cash
            Router.Send(client, 49, buffer);


            // IBuffer buffer = EzServer.Buffer.Provide();
            //  Router.Send(client, 50, buffer); 
        }
    }
}