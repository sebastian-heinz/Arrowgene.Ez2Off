using Arrowgene.Ez2Off.Server.Packet.Builder;

namespace Arrowgene.Ez2Off.Server.Packet
{
    public class PacketBuilder
    {
        public static ICharacterPacket CharacterPacket { get; set; }
        public static IChatPacket ChatPacket { get; set; }
        public static IGamePacket GamePacket { get; set; }
        public static IInventoryPacket InventoryPacket { get; set; }
        public static ILobbyPacket LobbyPacket { get; set; }
        public static IMessagePacket MessagePacket { get; set; }
        public static IRoomPacket RoomPacket { get; set; }
        public static ISettingsPacket SettingsPacket { get; set; }
        public static IShopPacket ShopPacket { get; set; }
        public static ISongPacket SongPacket { get; set; }
    }
}