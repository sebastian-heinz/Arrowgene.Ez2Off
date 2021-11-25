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

using Arrowgene.Ez2Off.Server.Model;

namespace Arrowgene.Ez2Off.Server.Trait
{
    public abstract class RoomTrait : EzTrait
    {
        public RoomTrait(EzServer server) : base(server)
        {
        }

        public abstract void ClientLeave(Room room, EzClient client);
        public abstract void ClientJoin(Room room, EzClient client);
        public abstract void NewMaster(Room room, EzClient client);
        public abstract void Kick(Room room, EzClient client, byte playerSlot);
        public abstract void GameStarting(Room room);
        public abstract void GameStart(Room room);
        public abstract void GameResult(Room room);
        public abstract void ClientFinish(Room room, EzClient client);
        public abstract void GameFinish(Room room);
        public abstract void LoadingFinish(Room room);
        public abstract void NextRadiomixSong(Room room, EzClient client);

        public abstract void ChangeGameSetting(Room room, short playerSlot, RoomOptionType roomOption, int valueA = 0,
            int valueB = 0);
    }
}