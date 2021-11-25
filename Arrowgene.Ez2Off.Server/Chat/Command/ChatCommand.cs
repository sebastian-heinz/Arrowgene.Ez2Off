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
using Arrowgene.Ez2Off.Server.Chat.Command.Commands;
using Arrowgene.Ez2Off.Server.Chat.Messages;
using Arrowgene.Ez2Off.Server.Model;

namespace Arrowgene.Ez2Off.Server.Chat.Command
{
    /// <summary>
    /// Parses chat commands starting with the following style '/cmd param1 param2'
    /// You can use a forward slash ('/') in parameters to escape censorship.
    /// Example: '/pm name [/G/M]Nick' will be interpreted as '/pm name [GM]Nick'
    /// </summary>
    public class ChatCommand : IChatMiddleware
    {
        private readonly Dictionary<string, BaseChatCommand> _commands;

        public ChatCommand(EzServer server)
        {
            _commands = new Dictionary<string, BaseChatCommand>();
            AddCommand(new AdminCmd());
            AddCommand(new NoticeCmd(server));
        }

        public void AddCommand(BaseChatCommand command)
        {
            _commands.Add(command.Key, command);
        }

        public void HandleMessage(ChatMessage message, List<ChatMessage> messages)
        {
            PlayerChatMessage playerMessage = message as PlayerChatMessage;
            if (playerMessage == null)
            {
                return;
            }

            if (playerMessage.Message.Length <= 1 || playerMessage.Message[0] != '/')
            {
                return;
            }

            string commandMessage = playerMessage.Message.Substring(1);
            string[] command = commandMessage.Split(' ');
            if (command.Length <= 0)
            {
                return;
            }

            if (!_commands.ContainsKey(command[0]))
            {
                return;
            }

            BaseChatCommand bcc = _commands[command[0]];

            EzClient client = playerMessage.Sender;
            if (client == null || client.Account.State < bcc.State)
            {
                return;
            }

            int cmdLength = command.Length - 1;
            string[] cmd;
            if (cmdLength > 0)
            {
                cmd = new string[cmdLength];
                Array.Copy(command, 1, cmd, 0, cmdLength);
                for (int i = 0; i < cmd.Length; i++)
                {
                    cmd[i] = cmd[i].Replace("/", "");
                }
            }
            else
            {
                cmd = new string[0];
            }

            playerMessage.Deliver = false;
            _commands[command[0]].Execute(cmd, playerMessage, messages);
        }
    }
}