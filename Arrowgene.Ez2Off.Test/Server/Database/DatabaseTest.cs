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
using System.Diagnostics;
using System.IO;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Ez2Off.Server.Database.Sql;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Arrowgene.Ez2Off.Test.Server.Database
{
    public class DatabaseTest
    {
        public DatabaseTest(ITestOutputHelper output)
        {
            LogProvider.GlobalLogWrite += (sender, args) =>
            {
                Debug.WriteLine(args.Log);
                output.WriteLine(args.Log.ToString());
                Console.WriteLine(args.Log);
            };
        }
        
        [Fact]
        public void TestChar()
        {
            string file = "ez2of-test.sqlite";
            File.Delete(file);
            
            SqLiteDb database = new SqLiteDb(file);

            ScriptRunner scriptRunner = new ScriptRunner(database);
            scriptRunner.Run(Path.Combine(Utils.RelativeCommonDirectory(), "Database/sqlite_ez2on_structure.sql"));
            
            Account createdAccount = database.CreateAccount("Account", "Hash");
            Character character = new Character();
            character.Id = createdAccount.Id;
            character.Name = "CharacterName";
            
            Account createdAccount1 = database.CreateAccount("Account1", "Hash1");
            Character character1 = new Character();
            character.Id = createdAccount1.Id;
            character.Name = "CharacterName1";
            
            bool success = database.UpsertCharacter(character);
            Assert.True(success);
            
            bool success1 = database.UpsertCharacter(character);
            Assert.True(success1);   
            
            bool success3 = database.UpsertCharacter(character1);
            Assert.True(success3);
        }

        [Fact]
        public void TestDatabase()
        {
            TestSQLite();
            //TestMariaDB();
        }

        [Fact]
        public void TestSQLite()
        {
            string file = "ez2of-test.sqlite";
            File.Delete(file);
            RunDatabaseTest(new SqLiteDb(file));
        }

        [Fact]
        public void TestMariaDB()
        {
            string host = "localhost";
            short port = 3306;
            string user = "root";
            string password = "";
            string database = "arrowgene_test";
            MariaDb.Drop(host, port, user, password, database);
            IDatabase db = new MariaDb(host, port, user, password, database);
            RunDatabaseTest(db);
        }

        private void RunDatabaseTest(IDatabase database)
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
            Assert.Equal(AccountState.User, createdAccount.State);

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

            Character character = new Character();
            character.Id = createdAccount.Id;
            character.Name = "CharacterName";

            bool success = database.UpsertCharacter(character);
            Assert.True(success);
        }

        private void TestSetting(IDatabase database)
        {
            Setting setting = new Setting();
            Account createdAccount = database.CreateAccount("SettingAccount", "SettingHash");

            Character character = new Character();
            character.Id = createdAccount.Id;
            character.Name = "SettingCharacterName";

            bool success = database.UpsertCharacter(character);
            Assert.True(success);

            setting.CharacterId = character.Id;
            database.UpsertSetting(setting);

            Setting selectSetting = database.SelectSetting(createdAccount.Id);
            Assert.NotNull(selectSetting);
            Assert.Equal(setting.BgaSettings.Animation, selectSetting.BgaSettings.Animation);
            Assert.Equal(setting.BgaSettings.Battle, selectSetting.BgaSettings.Battle);
            Assert.Equal(setting.SkinPosition, selectSetting.SkinPosition);
            Assert.Equal(setting.VolumeGameMusic, selectSetting.VolumeGameMusic);
            Assert.Equal(setting.VolumeGameSfx, selectSetting.VolumeGameSfx);
            Assert.Equal(setting.VolumeMenuMusic, selectSetting.VolumeMenuMusic);
            Assert.Equal(setting.VolumeMenuSfx, selectSetting.VolumeMenuSfx);
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
            setting.VolumeGameSfx = 20;
            setting.VolumeMenuMusic = 30;
            setting.VolumeMenuSfx = 40;
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
            success = database.UpsertSetting(setting);
            Assert.True(success);

            Setting updatedSetting = database.SelectSetting(createdAccount.Id);
            Assert.NotNull(selectSetting);
            Assert.Equal(setting.BgaSettings.Animation, updatedSetting.BgaSettings.Animation);
            Assert.Equal(setting.BgaSettings.Battle, updatedSetting.BgaSettings.Battle);
            Assert.Equal(setting.SkinPosition, updatedSetting.SkinPosition);
            Assert.Equal(setting.VolumeGameMusic, updatedSetting.VolumeGameMusic);
            Assert.Equal(setting.VolumeGameSfx, updatedSetting.VolumeGameSfx);
            Assert.Equal(setting.VolumeMenuMusic, updatedSetting.VolumeMenuMusic);
            Assert.Equal(setting.VolumeMenuSfx, updatedSetting.VolumeMenuSfx);
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
            item.CoinPlus = 3;
            item.Price = 4;
            item.DefensePlus = 5;
            item.Duration = 6;
            item.Effect = "a";
            item.ExpPlus = 7;
            item.HpPlus = 8;
            item.Image = "b";
            item.Level = 11;
            item.Name = "c";
            item.ResiliencePlus = 15;
            item.Type = ItemType.Avatar;
            bool success = database.UpsertItem(item);
            Assert.True(success);

            Item selectedItem = database.SelectItem(item.Id);
            Assert.NotNull(selectedItem);
            Assert.Equal(item.CoinPlus, selectedItem.CoinPlus);
            Assert.Equal(item.Price, selectedItem.Price);
            Assert.Equal(item.DefensePlus, selectedItem.DefensePlus);
            Assert.Equal(item.Duration, selectedItem.Duration);
            Assert.Equal(item.Effect, selectedItem.Effect);
            Assert.Equal(item.ExpPlus, selectedItem.ExpPlus);
            Assert.Equal(item.HpPlus, selectedItem.HpPlus);
            Assert.Equal(item.Image, selectedItem.Image);
            Assert.Equal(item.Level, selectedItem.Level);
            Assert.Equal(item.Name, selectedItem.Name);
            Assert.Equal(item.ResiliencePlus, selectedItem.ResiliencePlus);
            Assert.Equal(item.Type, selectedItem.Type);
        }

        private void TestInventoryItem(IDatabase database)
        {
            Account createdAccount = database.CreateAccount("InventoryAccount", "InventoryHash");

            Character character = new Character();
            character.Id = createdAccount.Id;
            character.Name = "InventoryCharacter";

            bool success = database.UpsertCharacter(character);
            Assert.True(success);

            Item item = new Item();
            item.Id = 2;
            item.Effect = "a";
            item.Image = "b";
            item.Name = "c";
            success = database.UpsertItem(item);
            Assert.True(success);

            DateTime purchaseDate = DateTime.Now;
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.CharacterId = character.Id;
            inventoryItem.PurchaseDate = purchaseDate;
            inventoryItem.Equipped = Inventory.InvalidSlot;
            inventoryItem.Item = item;
            inventoryItem.Slot = Inventory.InvalidSlot;

            bool inserSuccess = database.InsertInventoryItem(inventoryItem);
            Assert.True(inserSuccess);

            InventoryItem selectedInventoryItem = database.SelectInventoryItem(inventoryItem.Id);
            Assert.NotNull(selectedInventoryItem);
            Assert.Equal(inventoryItem.CharacterId, selectedInventoryItem.CharacterId);
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
            Assert.Equal(inventoryItem.CharacterId, updatedInventoryItem.CharacterId);
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
            quest.Title = "QuestTitle";
            quest.Mission = "QuestMission";
            bool success = database.UpsertQuest(quest);
            Assert.True(success);

            Quest selectedQuest = database.SelectQuest(quest.Id);
            Assert.NotNull(selectedQuest);
            Assert.Equal(quest.Id, selectedQuest.Id);
            Assert.Equal(quest.Title, selectedQuest.Title);
            Assert.Equal(quest.Mission, selectedQuest.Mission);
        }

        private void TestSong(IDatabase database)
        {
            Song song = new Song();
            song.Name = "SongName";
            song.FileName = "FileName";
            song.Duration = "1:40";
            song.Id = 1;
            song.Bpm = 3;
            song.RubyEzExr = 4;
            song.RubyEzNotes = 5;
            song.RubyShdExr = 6;
            song.RubyShdNotes = 7;
            song.ClubHdExr = 8;
            song.ClubHdNotes = 9;
            song.ClubShdNotes = 10;


            bool insertSuccess = database.UpsertSong(song);
            Assert.True(insertSuccess);

            Song selectedSong = database.SelectSong(song.Id);
            Assert.NotNull(selectedSong);
            Assert.Equal(song.Name, selectedSong.Name);
            Assert.Equal(song.Duration, selectedSong.Duration);
            Assert.Equal(song.Id, selectedSong.Id);
            Assert.Equal(song.Bpm, selectedSong.Bpm);
            Assert.Equal(song.RubyEzExr, selectedSong.RubyEzExr);
            Assert.Equal(song.RubyEzNotes, selectedSong.RubyEzNotes);
            Assert.Equal(song.RubyShdExr, selectedSong.RubyShdExr);
            Assert.Equal(song.RubyShdNotes, selectedSong.RubyShdNotes);
            Assert.Equal(song.ClubHdExr, selectedSong.ClubHdExr);
            Assert.Equal(song.ClubHdNotes, selectedSong.ClubHdNotes);
            Assert.Equal(song.ClubShdNotes, selectedSong.ClubShdNotes);
            Assert.Equal(song.FileName, selectedSong.FileName);
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
            Account createdAccount = database.CreateAccount("ScoreAccount", "ScoreHash");
            Assert.NotNull(createdAccount);

            Character character = new Character();
            character.Id = createdAccount.Id;
            character.Name = "ScoreCharacterName";
            bool success = database.UpsertCharacter(character);
            Assert.True(success);

            Game game = new Game();
            game.Name = "ScoreGame";
            success = database.InsertGame(game);
            Assert.True(success);

            Song song = new Song();
            song.Id = 100;
            song.Name = "ScoreSong";
            song.Duration = "1:40";
            //  song.Category = "b";
            song.FileName = "e";
            success = database.UpsertSong(song);
            Assert.True(success);

            DateTime created = DateTime.Now;

            Score score = new Score();
            score.Id = 1;
            // score.GameId = game.Id;
            // score.CharacterId = createdAccount.Id;
            // score.SongId = song.Id;
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
            // score.Team = TeamType.Blue;
            //  score.Slot = 1;
            score.Created = created;

            success = database.InsertScore(score);
            Assert.True(success);

            List<Score> scores = database.SelectBestScores(song.Id, score.Mode, score.Difficulty, 1);
            Assert.NotNull(scores);
            Assert.True(scores.Count == 1);
            Score selectedScore = scores[0];
            Assert.NotNull(selectedScore);

            Assert.Equal(score.Id, selectedScore.Id);
            //  Assert.Equal(score.GameId, selectedScore.GameId);
            //  Assert.Equal(score.CharacterId, selectedScore.CharacterId);
            //  Assert.Equal(score.SongId, selectedScore.SongId);
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
            //   Assert.Equal(score.Team, selectedScore.Team);
            //   Assert.Equal(score.Slot, selectedScore.Slot);
            Assert.Equal(score.Created.ToString("yyyy-MM-dd HH:mm:ss"),
                selectedScore.Created.ToString("yyyy-MM-dd HH:mm:ss"));
            //    Assert.Equal(character.Name, selectedScore.CharacterName);

            // TODO change character name, select score again, check if score has updated name.
        }

        private void TestMeta(IDatabase database)
        {
            bool success = database.SetSetting("key", "value");
            Assert.True(success);

            string value = database.GetSetting("key");
            Assert.NotNull(value);
            Assert.True(value == "value");
        }
    }
}