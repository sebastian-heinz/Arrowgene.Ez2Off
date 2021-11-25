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

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.Id
{
    /// <summary>
    /// Packet Ids the world server sends to the client.
    /// 서버가 클라이언트로 보내는 패킷 ID
    /// </summary>
    public enum WorldResponseId
    {
        ChangeChannel = 0, // 0x00 - 로비에서 채널 선택
        LobbyEnter = 1, // 0x01 -
        CharacterList = 2, // 0x02 -
        CharacterListAdd = 3, // 0x03 -
        CharacterListRemove = 4, // 0x04 -
        CreateRoom = 9, // 0x09 -
        RoomCharacter = 10, // 0x0A -
        RoomJoin = 11, // 0x0B
        RoomLeave = 12, // 0x0C
        RoomMaster = 15, // 0x0F
        SelectSong = 16, // 0x10 -
        CreateRoomOptionApply = 17, // 0x11
        Chat = 18, // 0x12 -
        ChatPrivate = 20, // 0x14
        ChatGm = 21, // 0x15
        GameStart = 23, // 0x17 - Start Game
        GameLoading = 24, // 0x18 - Game Loading
        GameResult = 27, // 0x1B - Game Score
        GameCloseResult = 28, // 0x1C - Close Score Screen
        PurchaseItem = 29, // 0x1D -
        InventoryApplyItem = 30, // 0x1E - Equip/Delete/Move Item
        ApplyInventory = 33, // 0x21
        InventoryShow = 36, // 0x24 - Inventory
        Ranking = 37, // 0x25 - Show Ranking Screen - 랭킹 화면 표시
        CharacterInformation = 38, // 0x26
        GameScores = 39, // 0x27 - Scores Loading Screen
        InviteToRoom = 40, // 0x28
        PurchaseDjPoint = 44, // 0x2C
        SaveSettings = 45, // 0x2D -
        NotClientPacket46 = 46, // NotClientPacket
        NotClientPacket47 = 47, // NotClientPacket
        MessageNewItemGift = 48, //0x30
        GiftInventory = 50, // 0x32
        FriendList = 53, // 0x35
        UpdateFriend = 57, // 0x39
        MessageBox = 62 // 0x3E
    }
}