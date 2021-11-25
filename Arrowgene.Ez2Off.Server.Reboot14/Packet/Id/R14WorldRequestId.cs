/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2020 Sebastian Heinz
 *
 * Github: https://github.com/Arrowgene/Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ez2Off is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ez2Off. If not, see <https://www.gnu.org/licenses/>.
 */

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.Id
{
    /// <summary>
    /// Packet Ids the client sends to the world server.
    /// 클라이언트가 서버로 보내는 패킷 ID
    /// </summary>
    public enum WorldRequestId
    {
        LobbyEnter = 1,
        ChangeChannel = 3,
        ChangeChannelSelect = 4,
        RoomCreate = 5,
        RoomEntry = 6,
        BackButton = 8,
        RoomSelectSong = 9,
        RoomChangeOptions = 10,
        Chat = 11,
        ChatWhisper = 12,
        RoomKick = 14,
        GameStart = 15,
        GameLoading = 16,
        GameBattleMeter = 17,
        GameResult = 18,
        ShopPurchaseItem = 20,
        InventoryApplyItem = 21,
        InventoryMoveItem = 22,
        InventoryDeleteItem = 23,
        SaveSettings = 24,
        InventoryShow = 25,
        CharacterInformation = 26,
        GameSongBestScore = 27,
        RoomInvite = 28,
        RoomViewLobbyMembers = 29,
        MusicBuyLicense = 31,
        ShopSendGift = 34,
        InventoryGifts = 35,
        InventoryAcceptGift = 36,
        FriendAdd = 37, 
        FriendDelete = 38,
        FriendChat = 39,
        MessengerOption = 40,
        InventoryUseItem = 42,
        MessengerBox = 43,
        Mission = 44,
        ChangeMode = 48,
        GameCheck = 51,
        ShowCaseMovieStart = 53,
        ShowCaseMovieBack = 54
    }
}