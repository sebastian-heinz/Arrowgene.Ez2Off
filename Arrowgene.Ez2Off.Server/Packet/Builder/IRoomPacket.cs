using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Packet.Builder
{
    public interface IRoomPacket
    {
        IBuffer CreateJoinErrorPacket();
        IBuffer CreateOpenRoomPacket(Room room, EzClient client);
        IBuffer CreateJoinRoomPacket(Room room, EzClient client);
        IBuffer KickPlayer(byte playerSlot);
        IBuffer LeavePlayer(byte playerSlot);
        IBuffer NewMaster(byte playerSlot);
        IBuffer AnnounceJoin(EzClient client);
        IBuffer CreateCharacterPacket(Room room);
        void WriteCharacter(IBuffer buffer, int index, EzClient client);
        IBuffer RoomClosed(Room room);
        IBuffer UpdateRoomStatus(Room room);

        /// <summary>
        /// 룸 리스트 생성
        /// </summary>
        IBuffer RoomList(Channel channel);

        IBuffer ItemUpdate(byte slot, Inventory inventory);
        IBuffer WaitingList(Channel channel);
        void WriteWaitingList(IBuffer buffer, Character character);
    }
}