using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Packet.Builder
{
    public interface IShopPacket
    {
        IBuffer CreatePurchasePacket(Item item, Character character, ShopPurchaseItemMessageType messageType);
        EzPacket CreateNewGiftNotification();
        EzPacket CreateSendGift(Character sender, GiftItem gift, string receiverCharacterName);
        EzPacket CreateSendGift(SendGiftResponseType responseType);
    }
}