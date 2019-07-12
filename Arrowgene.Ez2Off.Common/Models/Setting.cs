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
    public class Setting
    {
        public byte VolumeMenuMusic { get; set; }
        public byte VolumeMenuSFX { get; set; }
        public byte VolumeGameMusic { get; set; }
        public byte VolumeGameSFX { get; set; }
        public BgaSettings BgaSettings { get; set; }
        public byte SkinPosition { get; set; }
        public KeySettings KeySettingsRuby { get; set; }
        public KeySettings KeySettingsStreet { get; set; }
        public KeySettings KeySettingsClub { get; set; }

        public Setting()
        {
            BgaSettings = new BgaSettings(true, true);
            KeySettingsRuby = new KeySettings(ModeType.RubyMix);
            KeySettingsStreet = new KeySettings(ModeType.StreetMix);
            KeySettingsClub = new KeySettings(ModeType.ClubMix);
            VolumeMenuMusic = 50;
            VolumeMenuSFX = 50;
            VolumeGameMusic = 50;
            VolumeGameSFX = 50;
            SkinPosition = 0;
        }

        public void SetKeySettings(KeySettings keySettings, ModeType modeType)
        {
            switch (modeType)
            {
                case ModeType.RubyMix:
                    KeySettingsRuby = keySettings;
                    break;
                case ModeType.StreetMix:
                    KeySettingsStreet = keySettings;
                    break;
                case ModeType.ClubMix:
                    KeySettingsClub = keySettings;
                    break;
            }
        }

        public KeySettings GetKeySettings(ModeType modeType)
        {
            switch (modeType)
            {
                case ModeType.RubyMix: return KeySettingsRuby;
                case ModeType.StreetMix: return KeySettingsStreet;
                case ModeType.ClubMix: return KeySettingsClub;
                default: throw new Exception("Invalid ModeType");
            }
        }
    }
}