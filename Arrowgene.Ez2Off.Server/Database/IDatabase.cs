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

using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;

namespace Arrowgene.Ez2Off.Server.Database
{
    public interface IDatabase
    {
        Account CreateAccount(string name, string hash);
        bool UpdateAccount(Account account);
        Account SelectAccount(string accountName);
        Setting SelectSetting(int accountId);
        bool UpsertSetting(Setting setting, int accountId);
        Character SelectCharacter(int accountId);
        bool UpsertCharacter(Character character, int accountId);
        Character SelectCharacter(string characterName);
        Item SelectItem(int itemId);
        bool UpsertItem(Item item);
        List<Item> SelectItems();
        InventoryItem SelectInventoryItem(int inventoryItemId);
        bool InsertInventoryItem(InventoryItem inventoryItem);
        bool DeleteInventoryItem(int inventoryItemId);
        List<InventoryItem> SelectInventoryItems(int accountId);
        bool UpdateInventoryItem(InventoryItem inventoryItem);
        Quest SelectQuest(int questId);
        bool UpsertQuest(Quest quest);
        DatabaseMeta SelectMeta(int version);
        bool UpsertMeta(DatabaseMeta meta);
        Song SelectSong(int songId);
        bool UpsertSong(Song song);
        List<Score> SelectBestScores(int songId, DifficultyType difficulty, int scoreCount = -1);
        Score SelectBestScore(int accountId, int songId, DifficultyType difficulty);
        bool InsertScore(Score score);
        bool InsertGame(Game game);
    }
}