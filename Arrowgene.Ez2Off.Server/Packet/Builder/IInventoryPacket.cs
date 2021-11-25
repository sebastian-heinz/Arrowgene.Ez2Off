using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Packet.Builder
{
    public interface IInventoryPacket
    {
        EzPacket ShowGiftsPacket(GiftItem[] gifts);
        IBuffer ShowInventoryPacket(Inventory inventory);
        void WriteEquipSlots(Inventory inventory, IBuffer buffer);
        void WriteEquipSlot(int slot, Inventory inventory, IBuffer buffer);
        void WritePremiumSlot(Inventory inventory, IBuffer buffer);
        void WriteInventorySlot(int slot, Inventory inventory, IBuffer buffer);
        EzPacket CreateAcceptGift(AcceptGiftResponseType responseType);
    }
}