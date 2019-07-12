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

using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.Login
{
    public class CreateCharacter : Handler<LoginServer>
    {
        public CreateCharacter(LoginServer server) : base(server)
        {
        }

        public override int Id => 2;

        public override void Handle(EzClient client, EzPacket packet)
        {
            string characterName = packet.Data.ReadCString(Utils.KoreanEncoding);
            _logger.Debug("Character Name: {0}", characterName);

            Character existing = Server.Database.SelectCharacter(characterName);
            if (existing == null)
            {
                client.Session.Character = new Character(characterName);
                if (!Server.Database.UpsertCharacter(client.Session.Character, client.Account.Id))
                {
                    _logger.Error("Failed to save newly created character.");
                }

                IBuffer characterPacket = CharacterPacket.Create(client);
                Send(client, 2, characterPacket);
            }
            else
            {
                _logger.Error("Character with name ({0}) already exists", characterName);
                IBuffer error = EzServer.Buffer.Provide();
                error.WriteByte(1);
                Send(client, 2, error);
            }
        }
    }
}