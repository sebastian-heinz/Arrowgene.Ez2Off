/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2018 Sebastian Heinz
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
    public class KeySettings
    {
        public byte KeyAc1 { get; set; }
        public byte KeyAc2 { get; set; }
        public byte KeyAc3 { get; set; }
        public byte KeyAc4 { get; set; }
        public byte KeyAc5 { get; set; }
        public byte KeyAc6 { get; set; }

        public byte KeyOn1 { get; set; }
        public byte KeyOn2 { get; set; }
        public byte KeyOn3 { get; set; }
        public byte KeyOn4 { get; set; }
        public byte KeyOn5 { get; set; }
        public byte KeyOn6 { get; set; }
        
        public KeySettings(ModeType modeType)
        {
            switch (modeType)
            {
                case ModeType.RubyMix:
                    KeyOn1 = 0x0F;
                    KeyOn2 = 0x10;
                    KeyOn3 = 0x13;
                    KeyOn4 = 0x14;
                    KeyOn5 = 0xFF;
                    KeyOn6 = 0xFF;
                    
                    KeyAc1 = 0x22;
                    KeyAc2 = 0x18;
                    KeyAc3 = 0x0E;
                    KeyAc4 = 0x19;
                    KeyAc5 = 0xFF;
                    KeyAc6 = 0xFF;
                    break;
                case ModeType.StreetMix:
                    KeyOn1 = 0x0F;
                    KeyOn2 = 0x10;
                    KeyOn3 = 0x25;
                    KeyOn4 = 0x13;
                    KeyOn5 = 0x14;
                    KeyOn6 = 0xFF;
                    
                    KeyAc1 = 0x18;
                    KeyAc2 = 0x0E;
                    KeyAc3 = 0x19;
                    KeyAc4 = 0x0F;
                    KeyAc5 = 0x1A;
                    KeyAc6 = 0xFF;
                    break;
                case ModeType.ClubMix:
                    KeyOn1 = 0x0E;
                    KeyOn2 = 0x0F;
                    KeyOn3 = 0x10;
                    KeyOn4 = 0x13;
                    KeyOn5 = 0x14;
                    KeyOn6 = 0x15;
                    
                    KeyAc1 = 0x22;
                    KeyAc2 = 0x18;
                    KeyAc3 = 0x0E;
                    KeyAc4 = 0x19;
                    KeyAc5 = 0x0F;
                    KeyAc6 = 0x1A;
                    break;
            }
        }
    }
}