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
    /// Packet Ids the client sends to the login server.
    /// 클라이언트가 로그인 서버에 보내는 패킷 ID입니다.
    /// </summary>
    public enum LoginRequestId
    {
        LoginRequest = 0, // 0x00 - 로그인 요청
        CreateCharacter = 2, // 0x02 -
        ExitGame = 4, // 0x04 - 게임종료
        SelectChannel = 6, // 0x06 -
        SelectServer = 7, // 0x07 -
        SelectMode = 9, // 0x09 -
    }
}