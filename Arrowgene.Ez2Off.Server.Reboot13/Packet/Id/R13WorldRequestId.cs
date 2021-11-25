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
    /// Packet Ids the client sends to the world server.
    /// 클라이언트가 서버로 보내는 패킷 ID
    /// </summary>
    public enum WorldRequestId
    {
        LobbyEnter = 1, // 0x01 -
        ChangeChannel = 3, // 0x03 - 로비에서 채널 선택
        ChangeChannelSelect = 4, // 0x04 - 로비에서 채널 선택
        CreateRoom = 5, // 0x05 -
        BackButton = 8, // 0x08 - 뒤로 가기
        SelectSong = 9, // 0x09 - 
        CreateRoomOptionApply = 10, // 0x0A
        Chat = 11, // 0x0B -
        ChatPrivate = 12, // 0x0C -
        KickPlayer = 14, // 0x0E
        GameSongInfo = 15, // 0x0F - 
        GameLoading = 16, // 0x10 -
        CharacterInformation = 17, //
        GameResult = 18, // 0x12 -
        PurchaseItem = 20, // 0x14 -
        InventoryApplyItem = 21, // 0x15 - 
        InventoryMoveItem = 22, // 0x16 - 
        InventoryDeleteItem = 23, // 0x17 -
        SaveSettings = 24, // 0x18 -
        InventoryShow = 25, // 0x19 - 내가방 버튼 
        Ranking = 26, // 0x1A - Open Ranking Screen - 랭킹 스크린 열기
        GameSongScores = 28, // 0x1C -
        InviteToRoom = 29, // 0x1D
        PurchaseDjPoint = 32, // 0x20
        ShopSendGift = 35, // 0x23
        GiftInventory = 36, // 0x24
        ChatDirect = 40, // 0x28
        MessageBox = 43, // 0x2B
        Mission = 44, // 0x2C - 
    }
}