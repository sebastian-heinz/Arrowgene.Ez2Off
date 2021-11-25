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
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Packet.Builder;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.World
{
    public class SaveSettings : Handler<EzServer>
    {
        public SaveSettings(EzServer server) : base(server)
        {
        }

        public override int Id => 0x18;//24

        public override void Handle(EzClient client, EzPacket packet)
        {
            KeySettings keySettings = client.Setting.GetKeySettings(client.Mode);
            keySettings.KeyOn1 = packet.Data.ReadByte();
            keySettings.KeyOn2 = packet.Data.ReadByte();
            keySettings.KeyOn3 = packet.Data.ReadByte();
            keySettings.KeyOn4 = packet.Data.ReadByte();
            keySettings.KeyOn5 = packet.Data.ReadByte();
            keySettings.KeyOn6 = packet.Data.ReadByte();
            keySettings.KeyAc1 = packet.Data.ReadByte();
            keySettings.KeyAc2 = packet.Data.ReadByte();
            keySettings.KeyAc3 = packet.Data.ReadByte();
            keySettings.KeyAc4 = packet.Data.ReadByte();
            keySettings.KeyAc5 = packet.Data.ReadByte();
            keySettings.KeyAc6 = packet.Data.ReadByte();
            client.Setting.VolumeMenuMusic = packet.Data.ReadByte();
            client.Setting.VolumeMenuSfx = packet.Data.ReadByte();
            client.Setting.VolumeGameMusic = packet.Data.ReadByte();
            client.Setting.VolumeGameSfx = packet.Data.ReadByte();
            client.Setting.BgaSettings = new BgaSettings(packet.Data.ReadByte());
            client.Setting.SkinPosition = packet.Data.ReadByte();

            IBuffer settings = SettingsPacket.Create(client.Setting, client.Mode);
            Router.Send(client, 0x2D, settings); //45
        }
    }
}