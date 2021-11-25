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
using System.Collections.Generic;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Id;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class ShopSendGift : Handler<EzServer>
    {
        public ShopSendGift(EzServer server) : base(server)
        {
        }

        public override int Id => (int) WorldRequestId.ShopSendGift;

        public override void Handle(EzClient client, EzPacket packet)
        {
            //HEX:97-01-
            //00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-
            //44-69-65-74-65-72-00-00-00-00-00-00-00-00-00-00-00-00-
            //01-00

            short itemId = packet.Data.ReadInt16();
            packet.Data.ReadBytes(20);
            string receiverCharacterName = packet.Data.ReadFixedString(18, Utils.KoreanEncoding);
            packet.Data.ReadBytes(1); //01
            packet.Data.ReadBytes(1); //00

            Item item = Database.SelectItem(itemId);

            if (item == null)
            {
                Logger.Error(client, $"ItemId: {itemId} not found in database");
                Router.Send(client, PacketBuilder.ShopPacket.CreateSendGift(SendGiftResponseType.DatabaseError));
                return;
            }

            if (item.Currency == ItemCurrencyType.Cash)
            {
                if (client.Character.Cash < item.Price)
                {
                    Logger.Debug(client, $"Cash: {client.Character.Cash} to less to buy for Cash: {item.Price}");
                    Router.Send(client, PacketBuilder.ShopPacket.CreateSendGift(SendGiftResponseType.NotEnoughCash));
                    return;
                }
            }
            else
            {
                if (client.Character.Coin < item.Price)
                {
                    Logger.Debug(client, $"Coin: {client.Character.Coin} to less to buy for Coin: {item.Price}");
                    Router.Send(client, PacketBuilder.ShopPacket.CreateSendGift(SendGiftResponseType.NotEnoughCoin));
                    return;
                }
            }


            Logger.Debug(client, $"Sending ItemId: {itemId} to Character: {receiverCharacterName}");

            GiftItem gift = new GiftItem();
            gift.ItemId = item.Id;
            gift.SenderId = client.Character.Id;
            gift.SenderName = client.Character.Name;
            gift.SendAt = DateTime.Now;
            gift.ExpireDate = DateTime.Now.AddDays(GiftItem.ExpireDays);


            EzClient receiverClient = Server.Clients.GetClient(receiverCharacterName);
            if (receiverClient != null)
            {
                gift.ReceiverId = receiverClient.Character.Id;

                if (!receiverClient.Inventory.AddGiftItem(gift))
                {
                    Logger.Error(client, $"Gifts full");
                    // TODO client can not receive more than 15 gifts
                    Router.Send(client, PacketBuilder.ShopPacket.CreateSendGift(SendGiftResponseType.InvalidItemNumber));
                    return;
                }
            }
            else
            {
                Character receiverCharacter = Database.SelectCharacter(receiverCharacterName);
                if (receiverCharacter == null)
                {
                    Logger.Error(client, $"Receiver Character: {receiverCharacterName} doesn't exist");
                    Router.Send(client, PacketBuilder.ShopPacket.CreateSendGift(SendGiftResponseType.ReceiverNotFound));
                    return;
                }

                List<GiftItem> giftItems = Database.SelectGiftItems(receiverCharacter.Id);
                if (giftItems.Count >= Inventory.MaxGifts)
                {
                    Logger.Error(client, $"Gifts full");
                    // TODO client can not receive more than 15 gifts
                    Router.Send(client, PacketBuilder.ShopPacket.CreateSendGift(SendGiftResponseType.InvalidItemNumber));
                    return;
                }

                gift.ReceiverId = receiverCharacter.Id;
            }

            if (!Database.InsertGiftItem(gift))
            {
                Logger.Error(client, $"Could not save gift");
                Router.Send(client, PacketBuilder.ShopPacket.CreateSendGift(SendGiftResponseType.DatabaseError));
            }

            if (item.Currency == ItemCurrencyType.Cash)
            {
                client.Character.Cash -= item.Price;
            }
            else
            {
                client.Character.Coin -= item.Price;
            }

            if (receiverClient != null)
            {
                Router.Send(receiverClient, PacketBuilder.ShopPacket.CreateNewGiftNotification());
            }

            Router.Send(client, PacketBuilder.ShopPacket.CreateSendGift(
                client.Character, gift, receiverCharacterName
            ));
        }
    }
}