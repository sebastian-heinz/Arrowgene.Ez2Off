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
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Networking;

namespace Arrowgene.Ez2Off.Server.Model
{
    [Serializable]
    public class ServerPoint
    {
        public const int MaxLoad = 1000;

        private object _lock;

        public int Id { get; set; }

        public int TotalConnections { get; private set; }

        /// <summary>
        /// 0 - 1000
        /// </summary>
        public short RubyLoad { get; private set; }

        /// <summary>
        /// 0 - 1000
        /// </summary>
        public short StreetLoad { get; private set; }

        /// <summary>
        /// 0 - 1000
        /// </summary>
        public short ClubLoad { get; private set; }

        public ServerPoint()
        {
            _lock = new object();
            Id = 0;
            RubyLoad = 0;
            StreetLoad = 0;
            ClubLoad = 0;
        }

        public NetworkPoint Public { get; set; }

        public short GetLoad(ModeType mode)
        {
            switch (mode)
            {
                case ModeType.RubyMix: return RubyLoad;
                case ModeType.StreetMix: return StreetLoad;
                case ModeType.ClubMix: return ClubLoad;
            }

            return 0;
        }

        public void ChangeLoad(ModeType mode, short amount)
        {
            lock (_lock)
            {
                TotalConnections += amount;
                switch (mode)
                {
                    case ModeType.RubyMix:
                        RubyLoad += amount;
                        break;
                    case ModeType.StreetMix:
                        StreetLoad += amount;
                        break;
                    case ModeType.ClubMix:
                        ClubLoad += amount;
                        break;
                }
            }
        }
    }
}