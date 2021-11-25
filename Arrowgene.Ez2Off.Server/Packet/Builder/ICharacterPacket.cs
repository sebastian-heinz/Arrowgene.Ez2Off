using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Packet.Builder
{
    public interface ICharacterPacket
    {
        IBuffer Create(EzClient client);
        void Write(IBuffer buffer, EzClient client);
    }
}