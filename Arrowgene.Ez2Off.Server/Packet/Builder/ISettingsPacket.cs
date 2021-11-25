using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Packet.Builder
{
    public interface ISettingsPacket
    {
        IBuffer Create(Setting setting, ModeType modeType);
        void Write(IBuffer buffer, Setting setting, ModeType modeType);
    }
}