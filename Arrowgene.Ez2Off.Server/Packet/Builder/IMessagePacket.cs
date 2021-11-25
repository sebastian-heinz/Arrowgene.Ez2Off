using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Packet.Builder
{
    public interface IMessagePacket
    {
        IBuffer CreateMessageBox(List<Message> messages);
        void WriteMessage(IBuffer buffer, Message message);
        IBuffer AddFriend(string characterName, FriendAddMessageType messageType);
        IBuffer DeleteFriend(string characterName, FriendDeleteMessageType messageType);
        IBuffer CreateNewMessageNotification();
        IBuffer CreateMessageResponse(MessageResponseType responseType);
    }
}