using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Packet.Builder
{
    public interface ILobbyPacket
    {
        IBuffer CreateCharacterList(Channel channel);
        IBuffer CreateCharacterListAdd(EzClient client);
        IBuffer CreateCharacterListRemove(EzClient client);
        IBuffer CreateGoToLobby(short clientChannelIDX, byte channelId);
        IBuffer CreateFriendList(List<Friend> friends, ClientLookup lookup);
        void WriteFriend(IBuffer buffer, Friend friend, ClientLookup lookup, EzClient friendClient = null);
        void WriteFriend(IBuffer buffer, string name, short mode, short channelId, bool online);
    }
}