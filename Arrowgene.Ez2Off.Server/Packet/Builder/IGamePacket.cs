using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Packet.Builder
{
    public interface IGamePacket
    {
        IBuffer CreateScore(Room room);
        IBuffer CreateGameOver(byte slot);
        IBuffer CreateGameStart(Song song, ModeType mode, DifficultyType difficulty);
    }
}