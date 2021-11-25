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
    /// Packet Ids the login server sends to the client.
    /// 로그인 서버가 클라이언트에 보내는 패킷 ID입니다.
    /// </summary>
    public enum LoginResponseId
    {
        LoginRequest = 0, // 0x00 - 로그인 응답
        ExistingCharacterInfo = 1, // 0x01 - 기존 캐릭터
        NewCharacterInfo = 2, //  0x02 - 새로운 성격
        SelectChannel = 7, // 0x07 -
        SelectServer = 8, // 0x08 -
        SelectMode = 10, // 0x0A -
    }
}