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

namespace Arrowgene.Ez2Off.Common.Models
{
    [Serializable]
    public class BgaSettings
    {
        public bool Animation { get; set; }
        public bool Battle { get; set; }

        public BgaSettings(byte settings)
        {
            FromByte(settings);
        }

        public BgaSettings(bool animation, bool battle)
        {
            Animation = animation;
            Battle = battle;
        }

        public void FromByte(byte settings)
        {
            switch (settings)
            {
                case 0:
                    Animation = false;
                    Battle = false;
                    break;
                case 1:
                    Animation = true;
                    Battle = false;
                    break;
                case 2:
                    Animation = true;
                    Battle = true;
                    break;
                case 3:
                    Animation = false;
                    Battle = true;
                    break;
                default: throw new Exception("Invalid BGA Setting");
            }
        }

        public byte ToByte()
        {
            if (Battle)
            {
                if (Animation)
                {
                    return 2;
                }

                return 3;
            }

            if (Animation)
            {
                return 1;
            }

            return 0;
        }
    }
}