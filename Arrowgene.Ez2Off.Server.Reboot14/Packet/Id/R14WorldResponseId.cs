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
    /// Packet Ids the world server sends to the client.
    /// 서버가 클라이언트로 보내는 패킷 ID
    /// </summary>
    public enum WorldResponseId : byte
    {
        ChangeChannel = 0, //
        CharacterList = 2, //
        CharacterListAdd = 3, //
        CharacterListRemove = 4, //
        ChannelFull = 5, // Shows Message FULL
        Unknown1 = 6, // If you send it, the client will send ID 2
        RoomFull = 8, // You Can not enter room
        CreateRoom = 9, //
        RoomCharacter = 10, //
        RoomJoin = 11, //
        RoomLeave = 12, //
        RoomMaster = 15, //
        SelectSong = 16, //
        CreateRoomOptionApply = 17, //
        Chat = 18, //
        ChatPrivate = 20, //
        ChatGm = 21, //
        GameStart = 23, // Start Game
        GameLoading = 24, // Game Loading
        GameResult = 27, // Game Score
        GameCloseResult = 28, // Close Score Screen
        PurchaseItem = 29, //
        InventoryApplyItem = 30, // Equip/Delete/Move Item
        ApplyInventory = 33,
        InventoryShow = 36, // Inventory
        CharacterInformation = 38,
        GameScores = 39, // Scores Loading Screen
        InviteToRoom = 40,
        SaveSettings = 44, 
        NotClientPacket46 = 46, // NotClientPacket
        NotClientPacket47 = 47, // NotClientPacket
        MessageNewItemGift = 48, //0x30
        GiftInventory = 50, // 0x32
        FriendListShow = 52, // 0x34
        FriendAdd = 53, // 0x35
        FriendDelete = 54, // 0x36
        FriendChat = 55, // 0x37
        UpdateFriend = 57, // 0x39
        MessageBoxListShow = 61, // 0x3E
        SkinTypeSave = 70,
        FinishLoading = 74,
        NextRadiomixSong = 75
    }
}