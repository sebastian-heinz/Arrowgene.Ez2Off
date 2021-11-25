using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Packet.Builder
{
    public interface ISongPacket
    {
        IBuffer CreateDjPointsPacket();
    }
}