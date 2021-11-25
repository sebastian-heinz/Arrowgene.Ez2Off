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

using System;
using System.Collections.Generic;

namespace Arrowgene.Ez2Off.Common.Models
{
    [Serializable]
    public class Player
    {
        public Player()
        {
            Playing = false;
            Ready = ReadyType.NotReady;
            Team = TeamType.None;
            Slot = 0;
            Loading = false;
        }

        public bool Playing { get; set; }
        public ReadyType Ready { get; set; }
        public TeamType Team { get; set; }
        public int Slot { get; set; }
        public bool Loading { get; set; }
    }
}