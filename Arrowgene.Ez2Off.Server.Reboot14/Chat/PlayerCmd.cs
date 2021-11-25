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

using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Chat.Messages;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Chat.Command.Commands
{
    public class PlayerCmd : BaseChatCommand
    {
        private readonly object _lock;
        private readonly EzServer _server;

        public PlayerCmd(EzServer server)
        {
            _lock = new object();
            _server = server;
        }

        public override void Execute(string[] command, PlayerChatMessage message, List<ChatMessage> messages)
        {
            if (message.Type != ChatType.Lobby)
            {
                AddResponse(message, messages, "Please use Lobby Chat");
                return;
            }

            if (command.Length <= 0)
            {
                AddResponse(message, messages, "--- Commands ---");
                AddResponse(message, messages, "Coin->EzCash `/pc buy-cash 1` [1EzCash = 100Coin]");
                AddResponse(message, messages, "--- End ---");
                return;
            }

            lock (_lock)
            {
                EzClient client = message.Sender;
                Character character = client.Character;
                switch (command[0])
                {
                    // case "buy-coin":
                    // if (command.Length <= 1)
                    // {
                    //     AddResponse(message, messages, "To less info use: '/pc buy-coin 1000'");
                    //     return;
                    // }
                    // if (!int.TryParse(command[1], out int coin) || coin <= 0 || coin % 100 != 0)
                    // {
                    //     AddResponse(message, messages, $"'{command[1]}' is not a valid number");
                    //     return;
                    // }
                    // int cashCost = coin / 100;
                    // int newCash = character.Cash - cashCost;
                    // int currentCoin = character.Coin;
                    // AddResponse(message, messages, $"Buy {coin}Coins for {cashCost}EzCash");
                    // if (newCash < 0)
                    // {
                    //     AddResponse(message, messages, "Not enough EzCash");
                    //     return;
                    // }
                    // character.Coin = currentCoin + coin;
                    // character.Cash = newCash;
                    // if (!_server.Database.UpsertCharacter(character))
                    // {
                    //     AddResponse(message, messages, "Error");
                    //     return;
                    // }
                    // AddResponse(message, messages,
                    //     $"OK [EzCash: {character.Cash} (-{cashCost})] [Coin: {character.Coin} (+{coin})]");
                    // break;
                    case "buy-cash":
                        if (command.Length <= 1)
                        {
                            AddResponse(message, messages, "To less info use: '/pc buy-cash 100'");
                            return;
                        }

                        if (!int.TryParse(command[1], out int cash) || cash <= 0)
                        {
                            AddResponse(message, messages, $"'{command[1]}' is not a valid number");
                            return;
                        }

                        int coinCost = cash * 100;
                        int newCoin = character.Coin - coinCost;
                        int currentCash = character.Cash;
                        AddResponse(message, messages, $"Buy {cash}EzCash for {coinCost}Coin");
                        if (newCoin < 0)
                        {
                            AddResponse(message, messages, "Not enough Coin");
                            return;
                        }

                        character.Cash = currentCash + cash;
                        character.Coin = newCoin;
                        if (!_server.Database.UpsertCharacter(character))
                        {
                            AddResponse(message, messages, "Error");
                            return;
                        }

                        AddResponse(message, messages,
                            $"OK [EzCash: {character.Cash} (+{cash})] [Coin: {character.Coin} (-{coinCost})]");
                        
                        // TODO update coins
                        IBuffer buffer = EzServer.Buffer.Provide();
                        buffer.WriteInt32((byte) ShopPurchaseItemMessageType.Success);
                        buffer.WriteByte(0);
                        buffer.WriteInt32(0);
                        buffer.WriteInt32(character.Coin, Endianness.Big);
                        buffer.WriteInt32(character.Cash, Endianness.Big);
                        _server.Router.Send(client, 29, buffer);
                        break;
                }
            }
        }

        private void AddResponse(PlayerChatMessage message, List<ChatMessage> messages, string text)
        {
            messages.Add(new ChatMessage("System", text, ChatType.Whisper, message.Sender));
        }

        public override AccountState State => AccountState.User;
        public override string Key => "pc";
    }
}