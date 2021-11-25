using System;

namespace Arrowgene.Ez2Off.Common.Models
{
    [Serializable]
    public enum FriendAddMessageType
    {
        Success = 1,
        MissingName = 100,
        FriendDoesNotExist = 101,
        CanNotAddMoreFriends = 102,
        AlreadyFriend = 103
    }
}