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

namespace Arrowgene.Ez2Off.Server.Database
{
    public interface IDatabase
    {
        void Execute(string sql);
        string GetSetting(string key);
        bool SetSetting(string key, string value);
        bool DeleteSetting(string key);
        Account CreateAccount(string name, string hash);
        bool UpdateAccount(Account account);
        Account SelectAccount(string accountName);
        Setting SelectSetting(int accountId);
        bool UpsertSetting(Setting setting);
        Character SelectCharacter(int characterId);
        bool UpsertCharacter(Character character);
        Character SelectCharacter(string characterName);
        Item SelectItem(int itemId);
        bool UpsertItem(Item item);
        List<Item> SelectItems();
        InventoryItem SelectInventoryItem(int inventoryItemId);
        bool InsertInventoryItem(InventoryItem inventoryItem);
        bool DeleteInventoryItem(int inventoryItemId);
        bool DeleteInventoryItems(List<int> inventoryItemIds);
        List<InventoryItem> SelectInventoryItems(int characterId);
        List<InventoryItem> SelectExpiredInventoryItems();
        bool UpdateInventoryItem(InventoryItem inventoryItem);
        Quest SelectQuest(int questId);
        bool UpsertQuest(Quest quest);
        Song SelectSong(int songId);
        List<Song> SelectSongs();
        bool UpsertSong(Song song);
        List<Radiomix> SelectRadiomixes();
        bool UpsertRadiomix(Radiomix radiomix);
        Score SelectScore(int scoreId);
        List<Score> SelectBestScores(int songId, ModeType mode, DifficultyType difficulty, int scoreCount = -1);
        Score SelectBestScore(int accountId, int songId, ModeType mode, DifficultyType difficulty);
        Score SelectMaxScore(int characterId, ModeType mode);
        bool InsertScore(Score score);
        Game SelectGame(int gameId);
        bool InsertGame(Game game);
        Rank SelectRank(int rankId);
        bool InsertRank(Rank rank);
        bool UpsertStatus(DateTime status);
        List<Message> SelectMessages(int characterId);
        bool InsertMessage(Message message);
        bool UpdateMessage(Message message);
        bool DeleteMessage(int messageId);
        List<Friend> SelectFriends(int characterId);
        bool InsertFriend(Friend friend);
        bool DeleteFriend(int friendId);
        GiftItem SelectGiftItem(int giftItemId);
        bool InsertGiftItem(GiftItem giftItem);
        bool UpdateGiftItem(GiftItem giftItem);
        bool DeleteGiftItem(int giftItemId);
        List<GiftItem> SelectGiftItems(int characterId);
        List<GiftItem> SelectExpiredGifts();
        bool DeleteGifts(List<int> giftIds);
        bool InsertIncident(Incident incident);
        bool InsertIdentification(Identification identification);
        bool InsertScoreIncident(Incident incident, int scoreId);
        bool InsertScoreIncident(int incidentId, int scoreId);
    }
}