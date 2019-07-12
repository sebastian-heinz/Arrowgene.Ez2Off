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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Ez2Off.Server.Database.SQLite;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Services.Logging;
using Xunit;

namespace Arrowgene.Ez2Off.Test.Server.Database
{
    public class DatabaseTest
    {
        public DatabaseTest()
        {
            LogProvider.GlobalLogWrite += (sender, args) => { Debug.WriteLine(args.Log); };
        }

        [Fact]
        public void TestSQLiteDatabase()
        {
            File.Delete("ez2of-test.sqlite");
            TestDatabase(new SQLiteDb("ez2of-test.sqlite"));
        }

        private void TestDatabase(IDatabase database)
        {
            TestAccount(database);
            TestCharacter(database);
            TestSetting(database);
            TestItem(database);
            TestInventoryItem(database);
            TestQuest(database);
            TestSong(database);
            TestGame(database);
            TestScore(database);
            TestMeta(database);
        }

        private void TestAccount(IDatabase database)
        {
            Account createdAccount = database.CreateAccount("Account", "Hash");
            Assert.NotNull(createdAccount);
            Assert.Equal("Account", createdAccount.Name);
            Assert.Equal("Hash", createdAccount.Hash);
            Assert.Equal(AccountState.Player, createdAccount.State);

            Account selectedAccount = database.SelectAccount("Account");
            Assert.NotNull(selectedAccount);
            Assert.Equal(createdAccount.Name, selectedAccount.Name);
            Assert.Equal(createdAccount.Hash, selectedAccount.Hash);
            Assert.Equal(createdAccount.State, selectedAccount.State);
            Assert.Equal(createdAccount.Id, selectedAccount.Id);

            createdAccount.Hash = "HashUpdate";
            createdAccount.Name = "AccountUpdate";
            createdAccount.State = AccountState.Banned;
            bool success = database.UpdateAccount(createdAccount);
            Assert.True(success);

            Account updatedAccount = database.SelectAccount("AccountUpdate");
            Assert.NotNull(updatedAccount);
            Assert.Equal("AccountUpdate", updatedAccount.Name);
            Assert.Equal("HashUpdate", updatedAccount.Hash);
            Assert.Equal(AccountState.Banned, updatedAccount.State);
            Assert.Equal(createdAccount.Id, updatedAccount.Id);
        }

        private void TestCharacter(IDatabase database)
        {
            Account createdAccount = database.CreateAccount("CharacterAccount", "CharacterHash");
            Assert.NotNull(createdAccount);

            Character character = new Character("CharacterName");

            bool success = database.UpsertCharacter(character, createdAccount.Id);
            Assert.True(success);
        }

        private void TestSetting(IDatabase database)
        {
            Setting setting = new Setting();
            Account createdAccount = database.CreateAccount("SettingAccount", "SettingHash");
            database.UpsertSetting(setting, createdAccount.Id);

            Setting selectSetting = database.SelectSetting(createdAccount.Id);
            Assert.NotNull(selectSetting);
            Assert.Equal(setting.BgaSettings.Animation, selectSetting.BgaSettings.Animation);
            Assert.Equal(setting.BgaSettings.Battle, selectSetting.BgaSettings.Battle);
            Assert.Equal(setting.SkinPosition, selectSetting.SkinPosition);
            Assert.Equal(setting.VolumeGameMusic, selectSetting.VolumeGameMusic);
            Assert.Equal(setting.VolumeGameSFX, selectSetting.VolumeGameSFX);
            Assert.Equal(setting.VolumeMenuMusic, selectSetting.VolumeMenuMusic);
            Assert.Equal(setting.VolumeMenuSFX, selectSetting.VolumeMenuSFX);
            Assert.Equal(setting.KeySettingsRuby.KeyAc1, selectSetting.KeySettingsRuby.KeyAc1);
            Assert.Equal(setting.KeySettingsRuby.KeyAc2, selectSetting.KeySettingsRuby.KeyAc2);
            Assert.Equal(setting.KeySettingsRuby.KeyAc3, selectSetting.KeySettingsRuby.KeyAc3);
            Assert.Equal(setting.KeySettingsRuby.KeyAc4, selectSetting.KeySettingsRuby.KeyAc4);
            Assert.Equal(setting.KeySettingsRuby.KeyAc5, selectSetting.KeySettingsRuby.KeyAc5);
            Assert.Equal(setting.KeySettingsRuby.KeyAc6, selectSetting.KeySettingsRuby.KeyAc6);
            Assert.Equal(setting.KeySettingsRuby.KeyOn1, selectSetting.KeySettingsRuby.KeyOn1);
            Assert.Equal(setting.KeySettingsRuby.KeyOn2, selectSetting.KeySettingsRuby.KeyOn2);
            Assert.Equal(setting.KeySettingsRuby.KeyOn3, selectSetting.KeySettingsRuby.KeyOn3);
            Assert.Equal(setting.KeySettingsRuby.KeyOn4, selectSetting.KeySettingsRuby.KeyOn4);
            Assert.Equal(setting.KeySettingsRuby.KeyOn5, selectSetting.KeySettingsRuby.KeyOn5);
            Assert.Equal(setting.KeySettingsRuby.KeyOn6, selectSetting.KeySettingsRuby.KeyOn6);
            Assert.Equal(setting.KeySettingsStreet.KeyAc1, selectSetting.KeySettingsStreet.KeyAc1);
            Assert.Equal(setting.KeySettingsStreet.KeyAc2, selectSetting.KeySettingsStreet.KeyAc2);
            Assert.Equal(setting.KeySettingsStreet.KeyAc3, selectSetting.KeySettingsStreet.KeyAc3);
            Assert.Equal(setting.KeySettingsStreet.KeyAc4, selectSetting.KeySettingsStreet.KeyAc4);
            Assert.Equal(setting.KeySettingsStreet.KeyAc5, selectSetting.KeySettingsStreet.KeyAc5);
            Assert.Equal(setting.KeySettingsStreet.KeyAc6, selectSetting.KeySettingsStreet.KeyAc6);
            Assert.Equal(setting.KeySettingsStreet.KeyOn1, selectSetting.KeySettingsStreet.KeyOn1);
            Assert.Equal(setting.KeySettingsStreet.KeyOn2, selectSetting.KeySettingsStreet.KeyOn2);
            Assert.Equal(setting.KeySettingsStreet.KeyOn3, selectSetting.KeySettingsStreet.KeyOn3);
            Assert.Equal(setting.KeySettingsStreet.KeyOn4, selectSetting.KeySettingsStreet.KeyOn4);
            Assert.Equal(setting.KeySettingsStreet.KeyOn5, selectSetting.KeySettingsStreet.KeyOn5);
            Assert.Equal(setting.KeySettingsStreet.KeyOn6, selectSetting.KeySettingsStreet.KeyOn6);
            Assert.Equal(setting.KeySettingsClub.KeyAc1, selectSetting.KeySettingsClub.KeyAc1);
            Assert.Equal(setting.KeySettingsClub.KeyAc2, selectSetting.KeySettingsClub.KeyAc2);
            Assert.Equal(setting.KeySettingsClub.KeyAc3, selectSetting.KeySettingsClub.KeyAc3);
            Assert.Equal(setting.KeySettingsClub.KeyAc4, selectSetting.KeySettingsClub.KeyAc4);
            Assert.Equal(setting.KeySettingsClub.KeyAc5, selectSetting.KeySettingsClub.KeyAc5);
            Assert.Equal(setting.KeySettingsClub.KeyAc6, selectSetting.KeySettingsClub.KeyAc6);
            Assert.Equal(setting.KeySettingsClub.KeyOn1, selectSetting.KeySettingsClub.KeyOn1);
            Assert.Equal(setting.KeySettingsClub.KeyOn2, selectSetting.KeySettingsClub.KeyOn2);
            Assert.Equal(setting.KeySettingsClub.KeyOn3, selectSetting.KeySettingsClub.KeyOn3);
            Assert.Equal(setting.KeySettingsClub.KeyOn4, selectSetting.KeySettingsClub.KeyOn4);
            Assert.Equal(setting.KeySettingsClub.KeyOn5, selectSetting.KeySettingsClub.KeyOn5);
            Assert.Equal(setting.KeySettingsClub.KeyOn6, selectSetting.KeySettingsClub.KeyOn6);

            setting.BgaSettings.Animation = true;
            setting.BgaSettings.Battle = true;
            setting.SkinPosition = 0xFF;
            setting.VolumeGameMusic = 10;
            setting.VolumeGameSFX = 20;
            setting.VolumeMenuMusic = 30;
            setting.VolumeMenuSFX = 40;
            setting.KeySettingsRuby.KeyOn1 = 51;
            setting.KeySettingsRuby.KeyOn2 = 52;
            setting.KeySettingsRuby.KeyOn3 = 53;
            setting.KeySettingsRuby.KeyOn4 = 54;
            setting.KeySettingsStreet.KeyOn1 = 61;
            setting.KeySettingsStreet.KeyOn2 = 62;
            setting.KeySettingsStreet.KeyOn3 = 63;
            setting.KeySettingsStreet.KeyOn4 = 64;
            setting.KeySettingsStreet.KeyOn5 = 65;
            setting.KeySettingsClub.KeyOn1 = 71;
            setting.KeySettingsClub.KeyOn2 = 72;
            setting.KeySettingsClub.KeyOn3 = 73;
            setting.KeySettingsClub.KeyOn4 = 74;
            setting.KeySettingsClub.KeyOn5 = 75;
            setting.KeySettingsClub.KeyOn6 = 76;
            
            setting.KeySettingsRuby.KeyAc1 = 151;
            setting.KeySettingsRuby.KeyAc2 = 152;
            setting.KeySettingsRuby.KeyAc3 = 153;
            setting.KeySettingsRuby.KeyAc4 = 154;
            setting.KeySettingsStreet.KeyAc1 = 161;
            setting.KeySettingsStreet.KeyAc2 = 162;
            setting.KeySettingsStreet.KeyAc3 = 163;
            setting.KeySettingsStreet.KeyAc4 = 164;
            setting.KeySettingsStreet.KeyAc5 = 165;
            setting.KeySettingsClub.KeyAc1 = 171;
            setting.KeySettingsClub.KeyAc2 = 172;
            setting.KeySettingsClub.KeyAc3 = 173;
            setting.KeySettingsClub.KeyAc4 = 174;
            setting.KeySettingsClub.KeyAc5 = 175;
            setting.KeySettingsClub.KeyAc6 = 176;
            bool success = database.UpsertSetting(setting, createdAccount.Id);
            Assert.True(success);

            Setting updatedSetting = database.SelectSetting(createdAccount.Id);
            Assert.NotNull(selectSetting);
            Assert.Equal(setting.BgaSettings.Animation, updatedSetting.BgaSettings.Animation);
            Assert.Equal(setting.BgaSettings.Battle, updatedSetting.BgaSettings.Battle);
            Assert.Equal(setting.SkinPosition, updatedSetting.SkinPosition);
            Assert.Equal(setting.VolumeGameMusic, updatedSetting.VolumeGameMusic);
            Assert.Equal(setting.VolumeGameSFX, updatedSetting.VolumeGameSFX);
            Assert.Equal(setting.VolumeMenuMusic, updatedSetting.VolumeMenuMusic);
            Assert.Equal(setting.VolumeMenuSFX, updatedSetting.VolumeMenuSFX);
            Assert.Equal(setting.KeySettingsRuby.KeyAc1, updatedSetting.KeySettingsRuby.KeyAc1);
            Assert.Equal(setting.KeySettingsRuby.KeyAc2, updatedSetting.KeySettingsRuby.KeyAc2);
            Assert.Equal(setting.KeySettingsRuby.KeyAc3, updatedSetting.KeySettingsRuby.KeyAc3);
            Assert.Equal(setting.KeySettingsRuby.KeyAc4, updatedSetting.KeySettingsRuby.KeyAc4);
            Assert.Equal(setting.KeySettingsRuby.KeyAc5, updatedSetting.KeySettingsRuby.KeyAc5);
            Assert.Equal(setting.KeySettingsRuby.KeyAc6, updatedSetting.KeySettingsRuby.KeyAc6);
            Assert.Equal(setting.KeySettingsRuby.KeyOn1, updatedSetting.KeySettingsRuby.KeyOn1);
            Assert.Equal(setting.KeySettingsRuby.KeyOn2, updatedSetting.KeySettingsRuby.KeyOn2);
            Assert.Equal(setting.KeySettingsRuby.KeyOn3, updatedSetting.KeySettingsRuby.KeyOn3);
            Assert.Equal(setting.KeySettingsRuby.KeyOn4, updatedSetting.KeySettingsRuby.KeyOn4);
            Assert.Equal(setting.KeySettingsRuby.KeyOn5, updatedSetting.KeySettingsRuby.KeyOn5);
            Assert.Equal(setting.KeySettingsRuby.KeyOn6, updatedSetting.KeySettingsRuby.KeyOn6);
            Assert.Equal(setting.KeySettingsStreet.KeyAc1, updatedSetting.KeySettingsStreet.KeyAc1);
            Assert.Equal(setting.KeySettingsStreet.KeyAc2, updatedSetting.KeySettingsStreet.KeyAc2);
            Assert.Equal(setting.KeySettingsStreet.KeyAc3, updatedSetting.KeySettingsStreet.KeyAc3);
            Assert.Equal(setting.KeySettingsStreet.KeyAc4, updatedSetting.KeySettingsStreet.KeyAc4);
            Assert.Equal(setting.KeySettingsStreet.KeyAc5, updatedSetting.KeySettingsStreet.KeyAc5);
            Assert.Equal(setting.KeySettingsStreet.KeyAc6, updatedSetting.KeySettingsStreet.KeyAc6);
            Assert.Equal(setting.KeySettingsStreet.KeyOn1, updatedSetting.KeySettingsStreet.KeyOn1);
            Assert.Equal(setting.KeySettingsStreet.KeyOn2, updatedSetting.KeySettingsStreet.KeyOn2);
            Assert.Equal(setting.KeySettingsStreet.KeyOn3, updatedSetting.KeySettingsStreet.KeyOn3);
            Assert.Equal(setting.KeySettingsStreet.KeyOn4, updatedSetting.KeySettingsStreet.KeyOn4);
            Assert.Equal(setting.KeySettingsStreet.KeyOn5, updatedSetting.KeySettingsStreet.KeyOn5);
            Assert.Equal(setting.KeySettingsStreet.KeyOn6, updatedSetting.KeySettingsStreet.KeyOn6);
            Assert.Equal(setting.KeySettingsClub.KeyAc1, updatedSetting.KeySettingsClub.KeyAc1);
            Assert.Equal(setting.KeySettingsClub.KeyAc2, updatedSetting.KeySettingsClub.KeyAc2);
            Assert.Equal(setting.KeySettingsClub.KeyAc3, updatedSetting.KeySettingsClub.KeyAc3);
            Assert.Equal(setting.KeySettingsClub.KeyAc4, updatedSetting.KeySettingsClub.KeyAc4);
            Assert.Equal(setting.KeySettingsClub.KeyAc5, updatedSetting.KeySettingsClub.KeyAc5);
            Assert.Equal(setting.KeySettingsClub.KeyAc6, updatedSetting.KeySettingsClub.KeyAc6);
            Assert.Equal(setting.KeySettingsClub.KeyOn1, updatedSetting.KeySettingsClub.KeyOn1);
            Assert.Equal(setting.KeySettingsClub.KeyOn2, updatedSetting.KeySettingsClub.KeyOn2);
            Assert.Equal(setting.KeySettingsClub.KeyOn3, updatedSetting.KeySettingsClub.KeyOn3);
            Assert.Equal(setting.KeySettingsClub.KeyOn4, updatedSetting.KeySettingsClub.KeyOn4);
            Assert.Equal(setting.KeySettingsClub.KeyOn5, updatedSetting.KeySettingsClub.KeyOn5);
            Assert.Equal(setting.KeySettingsClub.KeyOn6, updatedSetting.KeySettingsClub.KeyOn6);
        }

        private void TestItem(IDatabase database)
        {
            Item item = new Item();
            item.Id = 1;
            item.a = 1;
            item.b = 2;
            item.CoinPlus = 3;
            item.Coins = 4;
            item.DefensePlus = 5;
            item.Duration = 6;
            item.Effect = "a";
            item.ExpPlus = 7;
            item.HpPlus = 8;
            item.Image = "b";
            item.k = 9;
            item.l = 10;
            item.Level = 11;
            item.n = 12;
            item.Name = "c";
            item.o = 13;
            item.q = 14;
            item.ResiliencePlus = 15;
            item.s = 16;
            item.t = 17;
            item.Type = ItemType.Avatar;
            item.u = 18;
            bool success = database.UpsertItem(item);
            Assert.True(success);

            Item selectedItem = database.SelectItem(item.Id);
            Assert.NotNull(selectedItem);
            Assert.Equal(item.a, selectedItem.a);
            Assert.Equal(item.b, selectedItem.b);
            Assert.Equal(item.CoinPlus, selectedItem.CoinPlus);
            Assert.Equal(item.Coins, selectedItem.Coins);
            Assert.Equal(item.DefensePlus, selectedItem.DefensePlus);
            Assert.Equal(item.Duration, selectedItem.Duration);
            Assert.Equal(item.Effect, selectedItem.Effect);
            Assert.Equal(item.ExpPlus, selectedItem.ExpPlus);
            Assert.Equal(item.HpPlus, selectedItem.HpPlus);
            Assert.Equal(item.Image, selectedItem.Image);
            Assert.Equal(item.k, selectedItem.k);
            Assert.Equal(item.l, selectedItem.l);
            Assert.Equal(item.Level, selectedItem.Level);
            Assert.Equal(item.n, selectedItem.n);
            Assert.Equal(item.Name, selectedItem.Name);
            Assert.Equal(item.o, selectedItem.o);
            Assert.Equal(item.q, selectedItem.q);
            Assert.Equal(item.ResiliencePlus, selectedItem.ResiliencePlus);
            Assert.Equal(item.s, selectedItem.s);
            Assert.Equal(item.t, selectedItem.t);
            Assert.Equal(item.Type, selectedItem.Type);
            Assert.Equal(item.u, selectedItem.u);
        }

        private void TestInventoryItem(IDatabase database)
        {
            Account createdAccount = database.CreateAccount("InventoryAccount", "InventoryHash");

            Item item = new Item();
            item.Id = 2;
            item.Effect = "a";
            item.Image = "b";
            item.Name = "c";
            bool success = database.UpsertItem(item);
            Assert.True(success);

            DateTime purchaseDate = DateTime.Now;
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.AccountId = createdAccount.Id;
            inventoryItem.PurchaseDate = purchaseDate;
            inventoryItem.Equipped = Inventory.InvalidSlot;
            inventoryItem.Item = item;
            inventoryItem.Slot = Inventory.InvalidSlot;

            bool inserSuccess = database.InsertInventoryItem(inventoryItem);
            Assert.True(inserSuccess);

            InventoryItem selectedInventoryItem = database.SelectInventoryItem(inventoryItem.Id);
            Assert.NotNull(selectedInventoryItem);
            Assert.Equal(inventoryItem.AccountId, selectedInventoryItem.AccountId);
            Assert.Equal(inventoryItem.PurchaseDate.ToString("yyyy-MM-dd HH:mm:ss"),
                selectedInventoryItem.PurchaseDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Assert.Equal(inventoryItem.Equipped, selectedInventoryItem.Equipped);
            Assert.Equal(inventoryItem.Item.Id, selectedInventoryItem.Item.Id);
            Assert.Equal(inventoryItem.Slot, selectedInventoryItem.Slot);

            purchaseDate = purchaseDate.AddDays(1);
            inventoryItem.PurchaseDate = purchaseDate;
            inventoryItem.Equipped = 1;
            inventoryItem.Slot = 2;
            bool updateSuccess = database.UpdateInventoryItem(inventoryItem);
            Assert.True(updateSuccess);

            InventoryItem updatedInventoryItem = database.SelectInventoryItem(inventoryItem.Id);
            Assert.NotNull(updatedInventoryItem);
            Assert.Equal(inventoryItem.AccountId, updatedInventoryItem.AccountId);
            Assert.Equal(inventoryItem.PurchaseDate.ToString("yyyy-MM-dd HH:mm:ss"),
                updatedInventoryItem.PurchaseDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Assert.Equal(inventoryItem.Equipped, updatedInventoryItem.Equipped);
            Assert.Equal(inventoryItem.Item.Id, updatedInventoryItem.Item.Id);
            Assert.Equal(inventoryItem.Slot, updatedInventoryItem.Slot);

            bool deleteSuccess = database.DeleteInventoryItem(inventoryItem.Id);
            Assert.True(deleteSuccess);

            InventoryItem deletedInventoryItem = database.SelectInventoryItem(inventoryItem.Id);
            Assert.Null(deletedInventoryItem);
        }

        private void TestQuest(IDatabase database)
        {
            Quest quest = new Quest();
            quest.Id = 1;
            quest.a = 3;
            quest.b = 4;
            quest.c = 5;
            quest.d = 6;
            quest.Title = "QuestTitle";
            quest.Mission = "QuestMission";
            quest.g = 7;
            quest.h = 8;
            quest.i = 9;
            quest.j = 10;
            quest.k = 11;
            quest.l = 12;
            quest.m = 13;
            quest.n = 14;
            quest.o = 15;
            quest.p = 16;
            quest.q = 17;
            quest.r = 18;
            quest.s = 19;
            quest.t = 20;
            quest.u = 21;
            quest.v = 22;
            quest.w = 23;
            quest.x = 24;
            quest.y = 25;
            quest.z = 26;
            quest.z1 = 27;
            quest.z2 = 28;
            quest.z3 = 29;
            quest.z4 = 30;
            quest.z5 = 31;
            quest.z6 = 32;
            quest.z7 = 33;
            quest.z8 = 34;
            quest.z9 = 35;
            quest.z10 = 36;
            quest.z11 = 37;
            bool success = database.UpsertQuest(quest);
            Assert.True(success);

            Quest selectedQuest = database.SelectQuest(quest.Id);
            Assert.NotNull(selectedQuest);
            Assert.Equal(quest.Id, selectedQuest.Id);
            Assert.Equal(quest.a, selectedQuest.a);
            Assert.Equal(quest.b, selectedQuest.b);
            Assert.Equal(quest.c, selectedQuest.c);
            Assert.Equal(quest.d, selectedQuest.d);
            Assert.Equal(quest.Title, selectedQuest.Title);
            Assert.Equal(quest.Mission, selectedQuest.Mission);
            Assert.Equal(quest.g, selectedQuest.g);
            Assert.Equal(quest.h, selectedQuest.h);
            Assert.Equal(quest.i, selectedQuest.i);
            Assert.Equal(quest.j, selectedQuest.j);
            Assert.Equal(quest.k, selectedQuest.k);
            Assert.Equal(quest.l, selectedQuest.l);
            Assert.Equal(quest.m, selectedQuest.m);
            Assert.Equal(quest.n, selectedQuest.n);
            Assert.Equal(quest.o, selectedQuest.o);
            Assert.Equal(quest.p, selectedQuest.p);
            Assert.Equal(quest.q, selectedQuest.q);
            Assert.Equal(quest.r, selectedQuest.r);
            Assert.Equal(quest.s, selectedQuest.s);
            Assert.Equal(quest.t, selectedQuest.t);
            Assert.Equal(quest.u, selectedQuest.u);
            Assert.Equal(quest.v, selectedQuest.v);
            Assert.Equal(quest.w, selectedQuest.w);
            Assert.Equal(quest.x, selectedQuest.x);
            Assert.Equal(quest.y, selectedQuest.y);
            Assert.Equal(quest.z, selectedQuest.z);
            Assert.Equal(quest.z1, selectedQuest.z1);
            Assert.Equal(quest.z2, selectedQuest.z2);
            Assert.Equal(quest.z3, selectedQuest.z3);
            Assert.Equal(quest.z4, selectedQuest.z4);
            Assert.Equal(quest.z5, selectedQuest.z5);
            Assert.Equal(quest.z6, selectedQuest.z6);
            Assert.Equal(quest.z7, selectedQuest.z7);
            Assert.Equal(quest.z8, selectedQuest.z8);
            Assert.Equal(quest.z9, selectedQuest.z9);
            Assert.Equal(quest.z10, selectedQuest.z10);
            Assert.Equal(quest.z11, selectedQuest.z11);
        }

        private void TestSong(IDatabase database)
        {
            Song song = new Song();
            song.Name = "SongName";
            song.Duration = "1:40";
            song.Id = 1;
            song.Bpm = 3;
            song.RubyEzDifficulty = 4;
            song.RubyEzNotes = 5;
            song.RubyShdDifficulty = 6;
            song.RubyShdNotes = 7;
            song.ClubHdDifficulty = 8;
            song.ClubHdNotes = 9;
            song.ClubShdNotes = 10;
            song.b = "b";
            song.e = "e";
            song.d2 = 11;
            song.d1 = 12;
            song.d4 = 13;
            song.d6 = 14;
            song.d7 = 15;
            song.d8 = 16;
            song.d9 = 17;
            song.d10 = 18;
            song.d11 = 19;
            song.d12 = 20;
            song.d13 = 21;
            song.d14 = 22;
            song.d15 = 23;
            song.d16 = 24;
            song.d17 = 25;
            song.d19 = 26;
            song.d21 = 27;
            song.d22 = 28;
            song.d23 = 29;
            song.d24 = 30;
            song.d25 = 31;
            song.d26 = 32;
            song.d27 = 33;
            song.d28 = 34;
            song.d29 = 35;
            song.d30 = 36;
            song.d31 = 37;
            song.d32 = 38;
            song.d52 = 39;
            song.d51 = 40;
            song.d50 = 41;
            song.d49 = 42;
            song.d48 = 43;
            song.d47 = 44;
            song.d46 = 45;
            song.d45 = 46;
            song.d44 = 47;
            song.d33 = 48;
            song.d34 = 49;
            song.d35 = 50;
            song.d36 = 51;
            song.d37 = 52;
            song.d38 = 53;
            song.d39 = 54;
            song.d40 = 55;
            song.d41 = 56;
            song.d42 = 57;
            song.d43 = 58;
            song.d58 = 59;
            song.d59 = 60;
            song.d57 = 61;
            song.d56 = 62;
            song.d54 = 63;
            song.d61 = 64;

            bool insertSuccess = database.UpsertSong(song);
            Assert.True(insertSuccess);

            Song selectedSong = database.SelectSong(song.Id);
            Assert.NotNull(selectedSong);
            Assert.Equal(song.Name, selectedSong.Name);
            Assert.Equal(song.Duration, selectedSong.Duration);
            Assert.Equal(song.Id, selectedSong.Id);
            Assert.Equal(song.Bpm, selectedSong.Bpm);
            Assert.Equal(song.RubyEzDifficulty, selectedSong.RubyEzDifficulty);
            Assert.Equal(song.RubyEzNotes, selectedSong.RubyEzNotes);
            Assert.Equal(song.RubyShdDifficulty, selectedSong.RubyShdDifficulty);
            Assert.Equal(song.RubyShdNotes, selectedSong.RubyShdNotes);
            Assert.Equal(song.ClubHdDifficulty, selectedSong.ClubHdDifficulty);
            Assert.Equal(song.ClubHdNotes, selectedSong.ClubHdNotes);
            Assert.Equal(song.ClubShdNotes, selectedSong.ClubShdNotes);
            Assert.Equal(song.b, selectedSong.b);
            Assert.Equal(song.e, selectedSong.e);
            Assert.Equal(song.d2, selectedSong.d2);
            Assert.Equal(song.d1, selectedSong.d1);
            Assert.Equal(song.d4, selectedSong.d4);
            Assert.Equal(song.d6, selectedSong.d6);
            Assert.Equal(song.d7, selectedSong.d7);
            Assert.Equal(song.d8, selectedSong.d8);
            Assert.Equal(song.d9, selectedSong.d9);
            Assert.Equal(song.d10, selectedSong.d10);
            Assert.Equal(song.d11, selectedSong.d11);
            Assert.Equal(song.d12, selectedSong.d12);
            Assert.Equal(song.d13, selectedSong.d13);
            Assert.Equal(song.d14, selectedSong.d14);
            Assert.Equal(song.d15, selectedSong.d15);
            Assert.Equal(song.d16, selectedSong.d16);
            Assert.Equal(song.d17, selectedSong.d17);
            Assert.Equal(song.d19, selectedSong.d19);
            Assert.Equal(song.d21, selectedSong.d21);
            Assert.Equal(song.d22, selectedSong.d22);
            Assert.Equal(song.d23, selectedSong.d23);
            Assert.Equal(song.d24, selectedSong.d24);
            Assert.Equal(song.d25, selectedSong.d25);
            Assert.Equal(song.d26, selectedSong.d26);
            Assert.Equal(song.d27, selectedSong.d27);
            Assert.Equal(song.d28, selectedSong.d28);
            Assert.Equal(song.d29, selectedSong.d29);
            Assert.Equal(song.d30, selectedSong.d30);
            Assert.Equal(song.d31, selectedSong.d31);
            Assert.Equal(song.d32, selectedSong.d32);
            Assert.Equal(song.d52, selectedSong.d52);
            Assert.Equal(song.d51, selectedSong.d51);
            Assert.Equal(song.d50, selectedSong.d50);
            Assert.Equal(song.d49, selectedSong.d49);
            Assert.Equal(song.d48, selectedSong.d48);
            Assert.Equal(song.d47, selectedSong.d47);
            Assert.Equal(song.d46, selectedSong.d46);
            Assert.Equal(song.d45, selectedSong.d45);
            Assert.Equal(song.d44, selectedSong.d44);
            Assert.Equal(song.d33, selectedSong.d33);
            Assert.Equal(song.d34, selectedSong.d34);
            Assert.Equal(song.d35, selectedSong.d35);
            Assert.Equal(song.d36, selectedSong.d36);
            Assert.Equal(song.d37, selectedSong.d37);
            Assert.Equal(song.d38, selectedSong.d38);
            Assert.Equal(song.d39, selectedSong.d39);
            Assert.Equal(song.d40, selectedSong.d40);
            Assert.Equal(song.d41, selectedSong.d41);
            Assert.Equal(song.d42, selectedSong.d42);
            Assert.Equal(song.d43, selectedSong.d43);
            Assert.Equal(song.d58, selectedSong.d58);
            Assert.Equal(song.d59, selectedSong.d59);
            Assert.Equal(song.d57, selectedSong.d57);
            Assert.Equal(song.d56, selectedSong.d56);
            Assert.Equal(song.d54, selectedSong.d54);
            Assert.Equal(song.d61, selectedSong.d61);
        }

        private void TestGame(IDatabase database)
        {
            Game game = new Game();
            game.GroupType = GameGroupType.Individual;
            game.Name = "GameName";
            game.Type = GameType.SinglePlayer;

            bool success = database.InsertGame(game);
            Assert.True(success);
        }

        private void TestScore(IDatabase database)
        {
            Account createdAccount = database.CreateAccount("Account", "Hash");
            Assert.NotNull(createdAccount);

            Character character = new Character("ScoreCharacter");
            bool success = database.UpsertCharacter(character, createdAccount.Id);
            Assert.True(success);

            Game game = new Game();
            game.Name = "ScoreGame";
            success = database.InsertGame(game);
            Assert.True(success);

            Song song = new Song();
            song.Id = 100;
            song.Name = "ScoreSong";
            song.Duration = "1:40";
            song.b = "b";
            song.e = "e";
            success = database.UpsertSong(song);
            Assert.True(success);

            DateTime created = DateTime.Now;

            Score score = new Score();
            score.Id = 1;
            score.GameId = game.Id;
            score.AccountId = createdAccount.Id;
            score.SongId = song.Id;
            score.Difficulty = DifficultyType.EZ;
            score.StageClear = true;
            score.MaxCombo = 1;
            score.Kool = 1;
            score.Cool = 1;
            score.Good = 1;
            score.Miss = 1;
            score.Fail = 1;
            score.RawScore = 1;
            score.Rank = ScoreRankType.A;
            score.TotalNotes = 1;
            score.ComboType = ComboType.AllCombo;
            // score.TotalScore => CalulateTotalScore();
            score.NoteEffect = NoteEffectType.HyperRandom;
            score.FadeEffect = FadeEffectType.Blink;
            score.Team = TeamType.Blue;
            score.Slot = 1;
            score.Created = created;

            success = database.InsertScore(score);
            Assert.True(success);

            List<Score> scores = database.SelectBestScores(song.Id, score.Difficulty, 1);
            Assert.NotNull(scores);
            Assert.True(scores.Count == 1);
            Score selectedScore = scores[0];
            Assert.NotNull(selectedScore);

            Assert.Equal(score.Id, selectedScore.Id);
            Assert.Equal(score.GameId, selectedScore.GameId);
            Assert.Equal(score.AccountId, selectedScore.AccountId);
            Assert.Equal(score.SongId, selectedScore.SongId);
            Assert.Equal(score.Difficulty, selectedScore.Difficulty);
            Assert.Equal(score.StageClear, selectedScore.StageClear);
            Assert.Equal(score.MaxCombo, selectedScore.MaxCombo);
            Assert.Equal(score.Kool, selectedScore.Kool);
            Assert.Equal(score.Cool, selectedScore.Cool);
            Assert.Equal(score.Good, selectedScore.Good);
            Assert.Equal(score.Miss, selectedScore.Miss);
            Assert.Equal(score.Fail, selectedScore.Fail);
            Assert.Equal(score.RawScore, selectedScore.RawScore);
            Assert.Equal(score.Rank, selectedScore.Rank);
            Assert.Equal(score.TotalNotes, selectedScore.TotalNotes);
            Assert.Equal(score.ComboType, selectedScore.ComboType);
            Assert.Equal(score.TotalScore, selectedScore.TotalScore);
            Assert.Equal(score.NoteEffect, selectedScore.NoteEffect);
            Assert.Equal(score.FadeEffect, selectedScore.FadeEffect);
            Assert.Equal(score.Team, selectedScore.Team);
            Assert.Equal(score.Slot, selectedScore.Slot);
            Assert.Equal(score.Created.ToString("yyyy-MM-dd HH:mm:ss"),
                selectedScore.Created.ToString("yyyy-MM-dd HH:mm:ss"));
            Assert.Equal(character.Name, selectedScore.CharacterName);

            // TODO change character name, select score again, check if score has updated name.
        }

        private void TestMeta(IDatabase database)
        {
            DatabaseMeta meta = new DatabaseMeta(1);
            bool success = database.UpsertMeta(meta);
            Assert.True(success);

            DatabaseMeta selectedMeta = database.SelectMeta(1);
            Assert.NotNull(selectedMeta);
        }
    }
}