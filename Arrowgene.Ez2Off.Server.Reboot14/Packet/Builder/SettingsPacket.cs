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

using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Packet.Builder;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder
{
    public class SettingsPacket : ISettingsPacket
    {
        public IBuffer Create(Setting setting, ModeType modeType)
        {
            IBuffer buffer = EzServer.Buffer.Provide();
            Write(buffer, setting, modeType);
            return buffer;
        }

        public void Write(IBuffer buffer, Setting setting, ModeType modeType)
        {
            KeySettings keySettings = setting.GetKeySettings(modeType);
            buffer.WriteByte(keySettings.KeyOn1);
            buffer.WriteByte(keySettings.KeyOn2);
            buffer.WriteByte(keySettings.KeyOn3);
            buffer.WriteByte(keySettings.KeyOn4);
            buffer.WriteByte(keySettings.KeyOn5);
            buffer.WriteByte(keySettings.KeyOn6);
            buffer.WriteByte(keySettings.KeyAc1);
            buffer.WriteByte(keySettings.KeyAc2);
            buffer.WriteByte(keySettings.KeyAc3);
            buffer.WriteByte(keySettings.KeyAc4);
            buffer.WriteByte(keySettings.KeyAc5);
            buffer.WriteByte(keySettings.KeyAc6);
            buffer.WriteByte(setting.VolumeMenuMusic);
            buffer.WriteByte(setting.VolumeMenuSfx);
            buffer.WriteByte(setting.VolumeGameMusic);
            buffer.WriteByte(setting.VolumeGameSfx);
            buffer.WriteByte(setting.BgaSettings.ToByte());
            buffer.WriteByte(setting.SkinPosition);
            buffer.WriteByte(setting.SkinType);
        }
    }
}