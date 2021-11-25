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
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Chat.Messages;
using Arrowgene.Ez2Off.Server.Model;

namespace Arrowgene.Ez2Off.Server.Chat.Command.Commands
{
    public class NoticeCmd : BaseChatCommand
    {
        private readonly EzServer _server;

        public NoticeCmd(EzServer server)
        {
            _server = server;
        }

        public override void Execute(string[] command, PlayerChatMessage message, List<ChatMessage> messages)
        {
            if (command.Length <= 0)
            {
                return;
            }

            String msg = String.Join(" ", command);
            if (String.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            EzClient client = message.Sender;
            ChatMessage gmNotice =
                new ChatMessage(client.Character.Name, msg, ChatType.Gm, _server.Clients.GetAllClients());

            messages.Add(gmNotice);
        }

        public override AccountState State => AccountState.Admin;
        public override string Key => "n";
    }
}