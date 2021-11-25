using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Packet.Builder
{
    public interface IChatPacket
    {
        IBuffer CreateChannel(string sender, string message);
        IBuffer CreateRoom(byte playerSlot, string sender, string message);
        IBuffer CreateWhisper(string sender, string message);

        IBuffer CreateDirect(string sender, string receiver, string message,
            ChatDirectResponseType responseType);

        IBuffer CreateGm(string message);
    }
}