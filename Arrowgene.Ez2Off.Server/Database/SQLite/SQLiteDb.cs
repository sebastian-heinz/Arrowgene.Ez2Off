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
using System.Data.SQLite;
using System.IO;
using System.Text;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Log;
using Arrowgene.Services.Logging;

namespace Arrowgene.Ez2Off.Server.Database.SQLite
{
    public class SQLiteDb : IDatabase
    {
        public const string MemoryDatabasePath = ":memory:";
        private const string Seperator = " ";

        private string _databasePath;
        private Logger _logger;
        private string _connectionString;

        public SQLiteDb(string databasePath)
        {
            _logger = LogProvider<EzLogger>.GetLogger(this);
            _databasePath = databasePath;
            _logger.Info("Attention: Database requires that all server instances use the same file: {0}",
                _databasePath);
            CreateDatabase();
            CreateTables();
        }

        public Account CreateAccount(string name, string hash)
        {
            string sql = String.Join(Seperator,
                "INSERT INTO `account`",
                "(`name`, `hash`, `state`)",
                "VALUES",
                "(",
                $"'{name}',",
                $"'{hash}',",
                $"{(int) AccountState.Player}",
                ");"
            );
            try
            {
                ExecuteNonQuery(sql, out int id);
                return new Account(id, name, hash, AccountState.Player);
            }
            catch (Exception ex)
            {
                if (ex is SQLiteException sqLiteEx && sqLiteEx.ResultCode == SQLiteErrorCode.Constraint)
                {
                    // Account with this name already exists.
                }
                else
                {
                    _logger.Exception(ex);
                }

                return null;
            }
        }

        public bool UpdateAccount(Account account)
        {
            string sql = String.Join(Seperator,
                "UPDATE `account`",
                "SET",
                $"`name`='{account.Name}',",
                $"`hash`='{account.Hash}',",
                $"`state`={(int) account.State}",
                "WHERE",
                $"`id`={account.Id};"
            );
            try
            {
                ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                return false;
            }

            return true;
        }

        public Account SelectAccount(string accountName)
        {
            string sql = String.Join(Seperator,
                "SELECT `id`, `name`, `hash`, `state` FROM `account`",
                "WHERE",
                $"`name`='{accountName}';"
            );
            Account account = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.HasRows && reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string hash = reader.GetString(2);
                    int state = reader.GetInt32(3);
                    account = new Account((int) id, name, hash, (AccountState) state);
                }
            });
            return account;
        }

        public Setting SelectSetting(int accountId)
        {
            string sql = String.Join(Seperator,
                "SELECT `ruby_key_on_1`, `ruby_key_on_2`, `ruby_key_on_3`, `ruby_key_on_4`, `ruby_key_ac_1`, `ruby_key_ac_2`, `ruby_key_ac_3`, `ruby_key_ac_4`,",
                "`street_key_on_1`, `street_key_on_2`, `street_key_on_3`, `street_key_on_4`, `street_key_on_5`, `street_key_ac_1`, `street_key_ac_2`, `street_key_ac_3`, `street_key_ac_4`, `street_key_ac_5`,",
                "`club_key_on_1`, `club_key_on_2`, `club_key_on_3`, `club_key_on_4`, `club_key_on_5`, `club_key_on_6`, `club_key_ac_1`, `club_key_ac_2`, `club_key_ac_3`, `club_key_ac_4`, `club_key_ac_5`, `club_key_ac_6`,",
                "`volume_menu_music`, `volume_menu_sfx`, `volume_game_music`, `volume_game_sfx`, `bga_settings`, `skin_position`",
                "FROM `setting` WHERE",
                $"`account_id`={accountId};"
            );
            Setting setting = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.HasRows && reader.Read())
                {
                    setting = new Setting();
                    setting.KeySettingsRuby.KeyOn1 = reader.GetByte(0);
                    setting.KeySettingsRuby.KeyOn2 = reader.GetByte(1);
                    setting.KeySettingsRuby.KeyOn3 = reader.GetByte(2);
                    setting.KeySettingsRuby.KeyOn4 = reader.GetByte(3);
                    setting.KeySettingsRuby.KeyAc1 = reader.GetByte(4);
                    setting.KeySettingsRuby.KeyAc2 = reader.GetByte(5);
                    setting.KeySettingsRuby.KeyAc3 = reader.GetByte(6);
                    setting.KeySettingsRuby.KeyAc4 = reader.GetByte(7);
                    setting.KeySettingsStreet.KeyOn1 = reader.GetByte(8);
                    setting.KeySettingsStreet.KeyOn2 = reader.GetByte(9);
                    setting.KeySettingsStreet.KeyOn3 = reader.GetByte(10);
                    setting.KeySettingsStreet.KeyOn4 = reader.GetByte(11);
                    setting.KeySettingsStreet.KeyOn5 = reader.GetByte(12);
                    setting.KeySettingsStreet.KeyAc1 = reader.GetByte(13);
                    setting.KeySettingsStreet.KeyAc2 = reader.GetByte(14);
                    setting.KeySettingsStreet.KeyAc3 = reader.GetByte(15);
                    setting.KeySettingsStreet.KeyAc4 = reader.GetByte(16);
                    setting.KeySettingsStreet.KeyAc5 = reader.GetByte(17);
                    setting.KeySettingsClub.KeyOn1 = reader.GetByte(18);
                    setting.KeySettingsClub.KeyOn2 = reader.GetByte(19);
                    setting.KeySettingsClub.KeyOn3 = reader.GetByte(20);
                    setting.KeySettingsClub.KeyOn4 = reader.GetByte(21);
                    setting.KeySettingsClub.KeyOn5 = reader.GetByte(22);
                    setting.KeySettingsClub.KeyOn6 = reader.GetByte(23);
                    setting.KeySettingsClub.KeyAc1 = reader.GetByte(24);
                    setting.KeySettingsClub.KeyAc2 = reader.GetByte(25);
                    setting.KeySettingsClub.KeyAc3 = reader.GetByte(26);
                    setting.KeySettingsClub.KeyAc4 = reader.GetByte(27);
                    setting.KeySettingsClub.KeyAc5 = reader.GetByte(28);
                    setting.KeySettingsClub.KeyAc6 = reader.GetByte(29);
                    setting.VolumeMenuMusic = reader.GetByte(30);
                    setting.VolumeMenuSFX = reader.GetByte(31);
                    setting.VolumeGameMusic = reader.GetByte(32);
                    setting.VolumeGameSFX = reader.GetByte(33);
                    setting.BgaSettings.FromByte(reader.GetByte(34));
                    setting.SkinPosition = reader.GetByte(35);
                }
            });

            return setting;
        }

        public bool UpsertSetting(Setting setting, int accountId)
        {
            string sqlUpate = String.Join(Seperator,
                "UPDATE `setting`",
                "SET",
                $"`ruby_key_on_1`={setting.KeySettingsRuby.KeyOn1},",
                $"`ruby_key_on_2`={setting.KeySettingsRuby.KeyOn2},",
                $"`ruby_key_on_3`={setting.KeySettingsRuby.KeyOn3},",
                $"`ruby_key_on_4`={setting.KeySettingsRuby.KeyOn4},",
                $"`ruby_key_ac_1`={setting.KeySettingsRuby.KeyAc1},",
                $"`ruby_key_ac_2`={setting.KeySettingsRuby.KeyAc2},",
                $"`ruby_key_ac_3`={setting.KeySettingsRuby.KeyAc3},",
                $"`ruby_key_ac_4`={setting.KeySettingsRuby.KeyAc4},",
                $"`street_key_on_1`={setting.KeySettingsStreet.KeyOn1},",
                $"`street_key_on_2`={setting.KeySettingsStreet.KeyOn2},",
                $"`street_key_on_3`={setting.KeySettingsStreet.KeyOn3},",
                $"`street_key_on_4`={setting.KeySettingsStreet.KeyOn4},",
                $"`street_key_on_5`={setting.KeySettingsStreet.KeyOn5},",
                $"`street_key_ac_1`={setting.KeySettingsStreet.KeyAc1},",
                $"`street_key_ac_2`={setting.KeySettingsStreet.KeyAc2},",
                $"`street_key_ac_3`={setting.KeySettingsStreet.KeyAc3},",
                $"`street_key_ac_4`={setting.KeySettingsStreet.KeyAc4},",
                $"`street_key_ac_5`={setting.KeySettingsStreet.KeyAc5},",
                $"`club_key_on_1`={setting.KeySettingsClub.KeyOn1},",
                $"`club_key_on_2`={setting.KeySettingsClub.KeyOn2},",
                $"`club_key_on_3`={setting.KeySettingsClub.KeyOn3},",
                $"`club_key_on_4`={setting.KeySettingsClub.KeyOn4},",
                $"`club_key_on_5`={setting.KeySettingsClub.KeyOn5},",
                $"`club_key_on_6`={setting.KeySettingsClub.KeyOn6},",
                $"`club_key_ac_1`={setting.KeySettingsClub.KeyAc1},",
                $"`club_key_ac_2`={setting.KeySettingsClub.KeyAc2},",
                $"`club_key_ac_3`={setting.KeySettingsClub.KeyAc3},",
                $"`club_key_ac_4`={setting.KeySettingsClub.KeyAc4},",
                $"`club_key_ac_5`={setting.KeySettingsClub.KeyAc5},",
                $"`club_key_ac_6`={setting.KeySettingsClub.KeyAc6},",
                $"`volume_menu_music`={setting.VolumeMenuMusic},",
                $"`volume_menu_sfx`={setting.VolumeMenuSFX},",
                $"`volume_game_music`={setting.VolumeGameMusic},",
                $"`volume_game_sfx`={setting.VolumeGameSFX},",
                $"`bga_settings`={setting.BgaSettings.ToByte()},",
                $"`skin_position`={setting.SkinPosition}",
                "WHERE",
                $"`account_id`={accountId};"
            );
            int rowsAffected = ExecuteNonQuery(sqlUpate);
            if (rowsAffected == 0)
            {
                string sqlCreate = String.Join(Seperator,
                    "INSERT INTO `setting`",
                    "(`account_id`, `ruby_key_on_1`, `ruby_key_on_2`, `ruby_key_on_3`, `ruby_key_on_4`, `ruby_key_ac_1`, `ruby_key_ac_2`, `ruby_key_ac_3`, `ruby_key_ac_4`,",
                    "`street_key_on_1`, `street_key_on_2`, `street_key_on_3`, `street_key_on_4`, `street_key_on_5`, `street_key_ac_1`, `street_key_ac_2`, `street_key_ac_3`, `street_key_ac_4`, `street_key_ac_5`,",
                    "`club_key_on_1`, `club_key_on_2`, `club_key_on_3`, `club_key_on_4`, `club_key_on_5`, `club_key_on_6`, `club_key_ac_1`, `club_key_ac_2`, `club_key_ac_3`, `club_key_ac_4`, `club_key_ac_5`, `club_key_ac_6`,",
                    "`volume_menu_music`, `volume_menu_sfx`, `volume_game_music`, `volume_game_sfx`, `bga_settings`, `skin_position`)",
                    "VALUES",
                    "(",
                    $"{accountId},",
                    $"{setting.KeySettingsRuby.KeyOn1},",
                    $"{setting.KeySettingsRuby.KeyOn2},",
                    $"{setting.KeySettingsRuby.KeyOn3},",
                    $"{setting.KeySettingsRuby.KeyOn4},",
                    $"{setting.KeySettingsRuby.KeyAc1},",
                    $"{setting.KeySettingsRuby.KeyAc2},",
                    $"{setting.KeySettingsRuby.KeyAc3},",
                    $"{setting.KeySettingsRuby.KeyAc4},",
                    $"{setting.KeySettingsStreet.KeyOn1},",
                    $"{setting.KeySettingsStreet.KeyOn2},",
                    $"{setting.KeySettingsStreet.KeyOn3},",
                    $"{setting.KeySettingsStreet.KeyOn4},",
                    $"{setting.KeySettingsStreet.KeyOn5},",
                    $"{setting.KeySettingsStreet.KeyAc1},",
                    $"{setting.KeySettingsStreet.KeyAc2},",
                    $"{setting.KeySettingsStreet.KeyAc3},",
                    $"{setting.KeySettingsStreet.KeyAc4},",
                    $"{setting.KeySettingsStreet.KeyAc5},",
                    $"{setting.KeySettingsClub.KeyOn1},",
                    $"{setting.KeySettingsClub.KeyOn2},",
                    $"{setting.KeySettingsClub.KeyOn3},",
                    $"{setting.KeySettingsClub.KeyOn4},",
                    $"{setting.KeySettingsClub.KeyOn5},",
                    $"{setting.KeySettingsClub.KeyOn6},",
                    $"{setting.KeySettingsClub.KeyAc1},",
                    $"{setting.KeySettingsClub.KeyAc2},",
                    $"{setting.KeySettingsClub.KeyAc3},",
                    $"{setting.KeySettingsClub.KeyAc4},",
                    $"{setting.KeySettingsClub.KeyAc5},",
                    $"{setting.KeySettingsClub.KeyAc6},",
                    $"{setting.VolumeMenuMusic},",
                    $"{setting.VolumeMenuSFX},",
                    $"{setting.VolumeGameMusic},",
                    $"{setting.VolumeGameSFX},",
                    $"{setting.BgaSettings.ToByte()},",
                    $"{setting.SkinPosition}",
                    ");"
                );
                ExecuteNonQuery(sqlCreate);
                _logger.Debug("Created new setting for accountId: {0}", accountId);
            }

            return true;
        }

        public Character SelectCharacter(int accountId)
        {
            string sql = String.Join(Seperator,
                "SELECT `name`, `sex`, `level`, `ruby_exr`, `street_exr`, `club_exr`, `exp`, `coin`, `cash`,",
                "`max_combo`, `ruby_wins`, `street_wins`, `club_wins`, `ruby_loses`, `street_loses`, `club_loses`,",
                "`premium` FROM `character`",
                "WHERE",
                $"`account_id`={accountId};"
            );
            Character character = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.HasRows && reader.Read())
                {
                    string name = reader.GetString(0);
                    character = new Character(name);
                    character.Sex = (CharacterSex) reader.GetInt32(1);
                    character.Level = reader.GetByte(2);
                    character.RubyExr = reader.GetInt32(3);
                    character.StreetExr = reader.GetInt32(4);
                    character.ClubExr = reader.GetInt32(5);
                    character.Exp = reader.GetInt32(6);
                    character.Coin = reader.GetInt32(7);
                    character.Cash = reader.GetInt32(8);
                    character.MaxCombo = reader.GetInt16(9);
                    character.RubyWins = reader.GetInt32(10);
                    character.StreetWins = reader.GetInt32(11);
                    character.ClubWins = reader.GetInt32(12);
                    character.RubyLoses = reader.GetInt32(13);
                    character.StreetLoses = reader.GetInt32(14);
                    character.ClubLoses = reader.GetInt32(15);
                    character.Premium = reader.GetInt16(16);
                }
            });

            return character;
        }

        public Character SelectCharacter(string characterName)
        {
            string sql = String.Join(Seperator,
                "SELECT `name`, `sex`, `level`, `ruby_exr`, `street_exr`, `club_exr`, `exp`, `coin`, `cash`,",
                "`max_combo`, `ruby_wins`, `street_wins`, `club_wins`, `ruby_loses`, `street_loses`, `club_loses`,",
                "`premium` FROM `character`",
                "WHERE",
                $"`name`='{characterName}';"
            );
            Character character = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.HasRows && reader.Read())
                {
                    string name = reader.GetString(0);
                    character = new Character(name);
                    character.Sex = (CharacterSex) reader.GetInt32(1);
                    character.Level = reader.GetByte(2);
                    character.RubyExr = reader.GetInt32(3);
                    character.StreetExr = reader.GetInt32(4);
                    character.ClubExr = reader.GetInt32(5);
                    character.Exp = reader.GetInt32(6);
                    character.Coin = reader.GetInt32(7);
                    character.Cash = reader.GetInt32(8);
                    character.MaxCombo = reader.GetInt16(9);
                    character.RubyWins = reader.GetInt32(10);
                    character.StreetWins = reader.GetInt32(11);
                    character.ClubWins = reader.GetInt32(12);
                    character.RubyLoses = reader.GetInt32(13);
                    character.StreetLoses = reader.GetInt32(14);
                    character.ClubLoses = reader.GetInt32(15);
                    character.Premium = reader.GetInt16(16);
                }
            });

            return character;
        }

        public bool UpsertCharacter(Character character, int accountId)
        {
            string sqlUpate = String.Join(Seperator,
                "UPDATE `character`",
                "SET",
                $"`name`='{character.Name}',",
                $"`sex`={(int) character.Sex},",
                $"`level`={character.Level},",
                $"`ruby_exr`={character.RubyExr},",
                $"`street_exr`={character.StreetExr},",
                $"`club_exr`={character.ClubExr},",
                $"`exp`={character.Exp},",
                $"`coin`={character.Coin},",
                $"`cash`={character.Cash},",
                $"`max_combo`={character.MaxCombo},",
                $"`ruby_wins`={character.RubyWins},",
                $"`street_wins`={character.StreetWins},",
                $"`club_wins`={character.ClubWins},",
                $"`ruby_loses`={character.RubyLoses},",
                $"`street_loses`={character.StreetLoses},",
                $"`club_loses`={character.ClubLoses},",
                $"`premium`={character.Premium}",
                "WHERE",
                $"`account_id`={accountId};"
            );
            int rowsAffected = ExecuteNonQuery(sqlUpate);
            if (rowsAffected == 0)
            {
                string sqlCreate = String.Join(Seperator,
                    "INSERT INTO `character`",
                    "(`account_id`, `name`, `sex`, `level`, `ruby_exr`, `street_exr`, `club_exr`, `exp`, `coin`, `cash`,",
                    "`max_combo`, `ruby_wins`, `street_wins`, `club_wins`, `ruby_loses`, `street_loses`, `club_loses`, `premium`)",
                    "VALUES",
                    "(",
                    $"{accountId},",
                    $"'{character.Name}',",
                    $"{(int) character.Sex},",
                    $"{character.Level},",
                    $"{character.RubyExr},",
                    $"{character.StreetExr},",
                    $"{character.ClubExr},",
                    $"{character.Exp},",
                    $"{character.Coin},",
                    $"{character.Cash},",
                    $"{character.MaxCombo},",
                    $"{character.RubyWins},",
                    $"{character.StreetWins},",
                    $"{character.ClubWins},",
                    $"{character.RubyLoses},",
                    $"{character.StreetLoses},",
                    $"{character.ClubLoses},",
                    $"{character.Premium}",
                    ");"
                );
                ExecuteNonQuery(sqlCreate);
                _logger.Debug("Created new character for accountId: {0}", accountId);
            }

            return true;
        }

        public List<Item> SelectItems()
        {
            List<Item> items = new List<Item>();
            string sql = String.Join(Seperator,
                "SELECT `id`, `name`, `effect`, `image`, `duration`, `coins`, `level`, `exp_plus`, `coin_plus`, `hp_plus`,",
                "`resilience_plus`, `defense_plus`, `a`, `b`, `k`, `l`, `m`, `n`, `o`, `q`, `type`, `s`, `t`, `u`",
                "FROM `item`;"
            );
            ExecuteReader(sql, reader =>
            {
                while (reader.Read())
                {
                    Item item = new Item();
                    item.Id = reader.GetInt32(0);
                    item.Name = reader.GetString(1);
                    item.Effect = reader.GetString(2);
                    item.Image = reader.GetString(3);
                    item.Duration = reader.GetInt32(4);
                    item.Coins = reader.GetInt32(5);
                    item.Level = reader.GetInt32(6);
                    item.ExpPlus = reader.GetInt32(7);
                    item.CoinPlus = reader.GetInt32(8);
                    item.HpPlus = reader.GetInt32(9);
                    item.ResiliencePlus = reader.GetInt32(10);
                    item.DefensePlus = reader.GetInt32(11);
                    item.a = reader.GetInt32(12);
                    item.b = reader.GetInt32(13);
                    item.k = reader.GetInt32(14);
                    item.l = reader.GetInt32(15);
                    item.m = reader.GetInt32(16);
                    item.n = reader.GetInt32(17);
                    item.o = reader.GetInt32(18);
                    item.q = reader.GetInt32(19);
                    item.Type = (ItemType) reader.GetInt32(20);
                    item.s = reader.GetInt32(21);
                    item.t = reader.GetInt32(22);
                    item.u = reader.GetInt32(23);
                    items.Add(item);
                }
            });
            return items;
        }

        public InventoryItem SelectInventoryItem(int inventoryItemId)
        {
            string sql = String.Join(Seperator,
                "SELECT `item`.`id`, `item`.`name`, `item`.`effect`, `item`.`image`, `item`.`duration`, `item`.`coins`, `item`.`level`, `item`.`exp_plus`, `item`.`coin_plus`, `item`.`hp_plus`,",
                "`item`.`resilience_plus`, `item`.`defense_plus`, `a`, `b`, `k`, `l`, `m`, `n`, `o`, `q`, `type`, `s`, `t`, `u`,",
                "`inventory`.`account_id`, `inventory`.`purchase_date`, `inventory`.`slot`, `inventory`.`equipped`",
                "FROM `inventory`",
                "INNER JOIN `item` on `item`.`id` = `inventory`.`item_id`",
                "WHERE",
                $"`inventory`.`id`={inventoryItemId};"
            );
            InventoryItem inventoryItem = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.HasRows && reader.Read())
                {
                    Item item = new Item();
                    item.Id = reader.GetInt32(0);
                    item.Name = reader.GetString(1);
                    item.Effect = reader.GetString(2);
                    item.Image = reader.GetString(3);
                    item.Duration = reader.GetInt32(4);
                    item.Coins = reader.GetInt32(5);
                    item.Level = reader.GetInt32(6);
                    item.ExpPlus = reader.GetInt32(7);
                    item.CoinPlus = reader.GetInt32(8);
                    item.HpPlus = reader.GetInt32(9);
                    item.ResiliencePlus = reader.GetInt32(10);
                    item.DefensePlus = reader.GetInt32(11);
                    item.a = reader.GetInt32(12);
                    item.b = reader.GetInt32(13);
                    item.k = reader.GetInt32(14);
                    item.l = reader.GetInt32(15);
                    item.m = reader.GetInt32(16);
                    item.n = reader.GetInt32(17);
                    item.o = reader.GetInt32(18);
                    item.q = reader.GetInt32(19);
                    item.Type = (ItemType) reader.GetInt32(20);
                    item.s = reader.GetInt32(21);
                    item.t = reader.GetInt32(22);
                    item.u = reader.GetInt32(23);
                    item.u = reader.GetInt32(23);

                    inventoryItem = new InventoryItem();
                    inventoryItem.Id = inventoryItemId;
                    inventoryItem.AccountId = reader.GetInt32(24);
                    inventoryItem.PurchaseDate = reader.GetDateTime(25);
                    inventoryItem.Slot = reader.GetInt32(26);
                    inventoryItem.Equipped = reader.GetInt32(27);
                    inventoryItem.Item = item;
                }
            });

            return inventoryItem;
        }

        public bool InsertInventoryItem(InventoryItem inventoryItem)
        {
            string sqlCreate = String.Join(Seperator,
                "INSERT INTO `inventory`",
                "(`account_id`, `item_id`, `purchase_date`, `slot`, `equipped`)",
                "VALUES",
                "(",
                $"{inventoryItem.AccountId},",
                $"{inventoryItem.Item.Id},",
                $"'{inventoryItem.PurchaseDate:yyyy-MM-dd HH:mm:ss}',",
                $"{inventoryItem.Slot},",
                $"{inventoryItem.Equipped}",
                ");"
            );
            int affectedRows = ExecuteNonQuery(sqlCreate, out int autoIncrement);
            inventoryItem.Id = autoIncrement;
            return affectedRows > 0;
        }

        public bool DeleteInventoryItem(int inventoryItemId)
        {
            string sqlDelete = String.Join(Seperator,
                "DELETE FROM `inventory` WHERE",
                $"`id`={inventoryItemId}"
            );
            ExecuteNonQuery(sqlDelete);
            return true;
        }

        public List<InventoryItem> SelectInventoryItems(int accountId)
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();
            string sql = String.Join(Seperator,
                "SELECT `item`.`id`, `item`.`name`, `item`.`effect`, `item`.`image`, `item`.`duration`, `item`.`coins`, `item`.`level`, `item`.`exp_plus`, `item`.`coin_plus`, `item`.`hp_plus`,",
                "`item`.`resilience_plus`, `item`.`defense_plus`, `a`, `b`, `k`, `l`, `m`, `n`, `o`, `q`, `type`, `s`, `t`, `u`,",
                "`inventory`.`id`, `inventory`.`account_id`, `inventory`.`purchase_date`, `inventory`.`slot`, `inventory`.`equipped`",
                "FROM `inventory`",
                "INNER JOIN `item` on `item`.`id` = `inventory`.`item_id`",
                "WHERE",
                $"`inventory`.`account_id`={accountId};"
            );
            ExecuteReader(sql, reader =>
            {
                while (reader.Read())
                {
                    Item item = new Item();
                    item.Id = reader.GetInt32(0);
                    item.Name = reader.GetString(1);
                    item.Effect = reader.GetString(2);
                    item.Image = reader.GetString(3);
                    item.Duration = reader.GetInt32(4);
                    item.Coins = reader.GetInt32(5);
                    item.Level = reader.GetInt32(6);
                    item.ExpPlus = reader.GetInt32(7);
                    item.CoinPlus = reader.GetInt32(8);
                    item.HpPlus = reader.GetInt32(9);
                    item.ResiliencePlus = reader.GetInt32(10);
                    item.DefensePlus = reader.GetInt32(11);
                    item.a = reader.GetInt32(12);
                    item.b = reader.GetInt32(13);
                    item.k = reader.GetInt32(14);
                    item.l = reader.GetInt32(15);
                    item.m = reader.GetInt32(16);
                    item.n = reader.GetInt32(17);
                    item.o = reader.GetInt32(18);
                    item.q = reader.GetInt32(19);
                    item.Type = (ItemType) reader.GetInt32(20);
                    item.s = reader.GetInt32(21);
                    item.t = reader.GetInt32(22);
                    item.u = reader.GetInt32(23);

                    InventoryItem inventoryItem = new InventoryItem();
                    inventoryItem.Id = reader.GetInt32(24);
                    inventoryItem.AccountId = reader.GetInt32(25);
                    inventoryItem.PurchaseDate = reader.GetDateTime(26);
                    inventoryItem.Slot = reader.GetInt32(27);
                    inventoryItem.Equipped = reader.GetInt32(28);
                    inventoryItem.Item = item;
                    inventoryItems.Add(inventoryItem);
                }
            });
            return inventoryItems;
        }

        public bool UpdateInventoryItem(InventoryItem inventoryItem)
        {
            string sqlUpate = String.Join(Seperator,
                "UPDATE `inventory`",
                "SET",
                $"`account_id`={inventoryItem.AccountId},",
                $"`item_id`={inventoryItem.Item.Id},",
                $"`purchase_date`='{inventoryItem.PurchaseDate:yyyy-MM-dd HH:mm:ss}',",
                $"`slot`={inventoryItem.Slot},",
                $"`equipped`={inventoryItem.Equipped}",
                "WHERE",
                $"`id`={inventoryItem.Id};"
            );
            int rowsAffected = ExecuteNonQuery(sqlUpate);
            return rowsAffected > 0;
        }

        public Item SelectItem(int itemId)
        {
            string sql = String.Join(Seperator,
                "SELECT `name`, `effect`, `image`, `duration`, `coins`, `level`, `exp_plus`, `coin_plus`, `hp_plus`,",
                "`resilience_plus`, `defense_plus`, `a`, `b`, `k`, `l`, `m`, `n`, `o`, `q`, `type`, `s`, `t`, `u`",
                "FROM `item`",
                "WHERE",
                $"`id`={itemId};"
            );
            Item item = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.HasRows && reader.Read())
                {
                    item = new Item();
                    item.Id = itemId;
                    item.Name = reader.GetString(0);
                    item.Effect = reader.GetString(1);
                    item.Image = reader.GetString(2);
                    item.Duration = reader.GetInt32(3);
                    item.Coins = reader.GetInt32(4);
                    item.Level = reader.GetInt32(5);
                    item.ExpPlus = reader.GetInt32(6);
                    item.CoinPlus = reader.GetInt32(7);
                    item.HpPlus = reader.GetInt32(8);
                    item.ResiliencePlus = reader.GetInt32(9);
                    item.DefensePlus = reader.GetInt32(10);
                    item.a = reader.GetInt32(11);
                    item.b = reader.GetInt32(12);
                    item.k = reader.GetInt32(13);
                    item.l = reader.GetInt32(14);
                    item.m = reader.GetInt32(15);
                    item.n = reader.GetInt32(16);
                    item.o = reader.GetInt32(17);
                    item.q = reader.GetInt32(18);
                    item.Type = (ItemType) reader.GetInt32(19);
                    item.s = reader.GetInt32(20);
                    item.t = reader.GetInt32(21);
                    item.u = reader.GetInt32(22);
                }
            });


            return item;
        }

        public bool UpsertItem(Item item)
        {
            string[] parameters =
            {
                "id", "name", "effect", "image", "duration", "coins", "level", "exp_plus", "coin_plus", "hp_plus",
                "resilience_plus", "defense_plus", "a", "b", "k", "l", "m", "n", "o", "q", "type", "s", "t", "u"
            };

            object[] values =
            {
                item.Id, item.Name, item.Effect, item.Image, item.Duration, item.Coins, item.Level, item.ExpPlus,
                item.CoinPlus, item.HpPlus, item.ResiliencePlus, item.DefensePlus, item.a, item.b, item.k, item.l,
                item.m, item.n, item.o, item.q, (int) item.Type, item.s, item.t, item.u,
            };
            return Upsert("item", "id", item.Id, parameters, values);
        }

        public Quest SelectQuest(int questId)
        {
            string sql = String.Join(Seperator,
                "SELECT `a`, `b`, `c`, `d`, `title`, `mission`, `g`, `h`, `i`, `j`, `k`, `l`, `m`, `n`, `o`, `p`,",
                "`q`, `r`, `s`, `t`, `u`, `v`, `w`, `x`, `y`, `z`, `z1`, `z2`, `z3`, `z4`, `z5`, `z6`, `z7`, `z8`, `z9`, `z10`, `z11`",
                "FROM `quest`",
                "WHERE",
                $"`id`={questId};"
            );
            Quest quest = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.HasRows && reader.Read())
                {
                    quest = new Quest();
                    quest.Id = questId;
                    quest.a = reader.GetInt32(0);
                    quest.b = reader.GetInt32(1);
                    quest.c = reader.GetInt32(2);
                    quest.d = reader.GetInt32(3);
                    quest.Title = reader.GetString(4);
                    quest.Mission = reader.GetString(5);
                    quest.g = reader.GetInt32(6);
                    quest.h = reader.GetInt32(7);
                    quest.i = reader.GetInt32(8);
                    quest.j = reader.GetInt32(9);
                    quest.k = reader.GetInt32(10);
                    quest.l = reader.GetInt32(11);
                    quest.m = reader.GetInt32(12);
                    quest.n = reader.GetInt32(13);
                    quest.o = reader.GetInt32(14);
                    quest.p = reader.GetInt32(15);
                    quest.q = reader.GetInt32(16);
                    quest.r = reader.GetInt32(17);
                    quest.s = reader.GetInt32(18);
                    quest.t = reader.GetInt32(19);
                    quest.u = reader.GetInt32(20);
                    quest.v = reader.GetInt32(21);
                    quest.w = reader.GetInt32(22);
                    quest.x = reader.GetInt32(23);
                    quest.y = reader.GetInt32(24);
                    quest.z = reader.GetInt32(25);
                    quest.z1 = reader.GetInt32(26);
                    quest.z2 = reader.GetInt32(27);
                    quest.z3 = reader.GetInt32(28);
                    quest.z4 = reader.GetInt32(29);
                    quest.z5 = reader.GetInt32(30);
                    quest.z6 = reader.GetInt32(31);
                    quest.z7 = reader.GetInt32(32);
                    quest.z8 = reader.GetInt32(33);
                    quest.z9 = reader.GetInt32(34);
                    quest.z10 = reader.GetInt32(35);
                    quest.z11 = reader.GetInt32(36);
                }
            });
            return quest;
        }

        public bool UpsertQuest(Quest quest)
        {
            string[] parameters =
            {
                "id", "a", "b", "c", "d", "title", "mission", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p",
                "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "z1", "z2", "z3", "z4", "z5", "z6", "z7", "z8", "z9",
                "z10", "z11"
            };

            object[] values =
            {
                quest.Id, quest.a, quest.b, quest.c, quest.d, quest.Title, quest.Mission, quest.g, quest.h, quest.i,
                quest.j, quest.k, quest.l, quest.m, quest.n, quest.o, quest.p, quest.q, quest.r, quest.s, quest.t,
                quest.u, quest.v, quest.w, quest.x, quest.y, quest.z, quest.z1, quest.z2, quest.z3, quest.z4, quest.z5,
                quest.z6, quest.z7, quest.z8, quest.z9, quest.z10, quest.z11,
            };
            return Upsert("quest", "id", quest.Id, parameters, values);
        }

        public Song SelectSong(int songId)
        {
            string sql = String.Join(Seperator,
                "SELECT `id`, `name`, `b`, `duration`, `bpm`, `e`, `d1`, `d2`,",
                "`ruby_ez_difficulty`, `d4`, `ruby_ez_notes`, `d6`, `d7`, `d8`, `d9`, `d10`, `d11`, `d12`, `d13`, `d14`, `d15`, `d16`, `d17`,",
                "`ruby_shd_difficulty`,`d19`,`ruby_shd_notes`,`d21`,`d22`,`d23`,`d24`,`d25`,`d26`,`d27`,`d28`,`d29`,`d30`,`d31`,`d32`,",
                "`d33`, `d34`, `d35`, `d36`, `d37`, `d38`, `d39`, `d40`, `d41`, `d42`, `d43`, `d44`, `d45`, `d46`, `d47`, `d48`, `d49`, `d50`, `d51`,",
                "`d52`, `club_hd_difficulty`, `d54`, `club_hd_notes`, `d56`, `d57`, `d58`, `d59`, `club_shd_notes`, `d61`",
                "FROM `song`",
                "WHERE",
                $"`id`={songId};"
            );
            Song song = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.HasRows && reader.Read())
                {
                    song = new Song();
                    song.Id = reader.GetInt32(0);
                    song.Name = reader.GetString(1);
                    song.b = reader.GetString(2);
                    song.Duration = reader.GetString(3);
                    song.Bpm = reader.GetInt32(4);
                    song.e = reader.GetString(5);
                    song.d1 = reader.GetInt32(6);
                    song.d2 = reader.GetInt32(7);
                    song.RubyEzDifficulty = reader.GetInt32(8);
                    song.d4 = reader.GetInt32(9);
                    song.RubyEzNotes = reader.GetInt32(10);
                    song.d6 = reader.GetInt32(11);
                    song.d7 = reader.GetInt32(12);
                    song.d8 = reader.GetInt32(13);
                    song.d9 = reader.GetInt32(14);
                    song.d10 = reader.GetInt32(15);
                    song.d11 = reader.GetInt32(16);
                    song.d12 = reader.GetInt32(17);
                    song.d13 = reader.GetInt32(18);
                    song.d14 = reader.GetInt32(19);
                    song.d15 = reader.GetInt32(20);
                    song.d16 = reader.GetInt32(21);
                    song.d17 = reader.GetInt32(22);
                    song.RubyShdDifficulty = reader.GetInt32(23);
                    song.d19 = reader.GetInt32(24);
                    song.RubyShdNotes = reader.GetInt32(25);
                    song.d21 = reader.GetInt32(26);
                    song.d22 = reader.GetInt32(27);
                    song.d23 = reader.GetInt32(28);
                    song.d24 = reader.GetInt32(29);
                    song.d25 = reader.GetInt32(30);
                    song.d26 = reader.GetInt32(31);
                    song.d27 = reader.GetInt32(32);
                    song.d28 = reader.GetInt32(33);
                    song.d29 = reader.GetInt32(34);
                    song.d30 = reader.GetInt32(35);
                    song.d31 = reader.GetInt32(36);
                    song.d32 = reader.GetInt32(37);
                    song.d33 = reader.GetInt32(38);
                    song.d34 = reader.GetInt32(39);
                    song.d35 = reader.GetInt32(40);
                    song.d36 = reader.GetInt32(41);
                    song.d37 = reader.GetInt32(42);
                    song.d38 = reader.GetInt32(43);
                    song.d39 = reader.GetInt32(44);
                    song.d40 = reader.GetInt32(45);
                    song.d41 = reader.GetInt32(46);
                    song.d42 = reader.GetInt32(47);
                    song.d43 = reader.GetInt32(48);
                    song.d44 = reader.GetInt32(49);
                    song.d45 = reader.GetInt32(50);
                    song.d46 = reader.GetInt32(51);
                    song.d47 = reader.GetInt32(52);
                    song.d48 = reader.GetInt32(53);
                    song.d49 = reader.GetInt32(54);
                    song.d50 = reader.GetInt32(55);
                    song.d51 = reader.GetInt32(56);
                    song.d52 = reader.GetInt32(57);
                    song.ClubHdDifficulty = reader.GetInt32(58);
                    song.d54 = reader.GetInt32(59);
                    song.ClubHdNotes = reader.GetInt32(60);
                    song.d56 = reader.GetInt32(61);
                    song.d57 = reader.GetInt32(62);
                    song.d58 = reader.GetInt32(63);
                    song.d59 = reader.GetInt32(64);
                    song.ClubShdNotes = reader.GetInt32(65);
                    song.d61 = reader.GetInt32(66);
                }
            });
            return song;
        }

        public bool UpsertSong(Song song)
        {
            string[] parameters =
            {
                "id", "name", "b", "duration", "bpm", "e", "d1", "d2", "ruby_ez_difficulty", "d4", "ruby_ez_notes",
                "d6", "d7", "d8", "d9", "d10", "d11", "d12", "d13", "d14", "d15", "d16", "d17", "ruby_shd_difficulty",
                "d19", "ruby_shd_notes", "d21", "d22", "d23", "d24", "d25", "d26", "d27", "d28", "d29", "d30", "d31",
                "d32", "d33", "d34", "d35", "d36", "d37", "d38", "d39", "d40", "d41", "d42", "d43", "d44", "d45",
                "d46", "d47", "d48", "d49", "d50", "d51", "d52", "club_hd_difficulty", "d54", "club_hd_notes", "d56",
                "d57", "d58", "d59", "club_shd_notes", "d61"
            };

            object[] values =
            {
                song.Id, song.Name, song.b, song.Duration, song.Bpm, song.e, song.d1, song.d2, song.RubyEzDifficulty,
                song.d4, song.RubyEzNotes, song.d6, song.d7, song.d8, song.d9, song.d10, song.d11, song.d12, song.d13,
                song.d14, song.d15, song.d16, song.d17, song.RubyShdDifficulty, song.d19, song.RubyShdNotes, song.d21,
                song.d22, song.d23, song.d24, song.d25, song.d26, song.d27, song.d28, song.d29, song.d30, song.d31,
                song.d32, song.d33, song.d34, song.d35, song.d36, song.d37, song.d38, song.d39, song.d40, song.d41,
                song.d42, song.d43, song.d44, song.d45, song.d46, song.d47, song.d48, song.d49, song.d50, song.d51,
                song.d52, song.ClubHdDifficulty, song.d54, song.ClubHdNotes, song.d56, song.d57, song.d58, song.d59,
                song.ClubShdNotes, song.d61
            };

            return Upsert("song", "id", song.Id, parameters, values);
        }

        public Score SelectBestScore(int accountId, int songId, DifficultyType difficulty)
        {
            string sql = String.Join(Seperator,
                "SELECT `id`, `game_id`, `score`.`account_id`, `song_id`, `difficulty`, `stage_clear`, `score`.`max_combo`, `kool`, `cool`, `good`,",
                "`miss`, `fail`, `raw_score`, `rank`, `total_notes`, `combo_type`, `total_score`, `note_effect`, `fade_effect`,",
                "`team`, `slot`, `created`, `character`.`name`",
                "FROM `score`",
                "INNER JOIN `character` on `character`.`account_id` = `score`.`account_id`",
                "WHERE",
                $"`song_id`={songId} AND `score`.`account_id`={accountId} AND `difficulty`={(int) difficulty}",
                "ORDER BY `total_score` DESC",
                "LIMIT 1;"
            );
            Score score = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    score = new Score();
                    score.Id = reader.GetInt32(0);
                    score.GameId = reader.GetInt32(1);
                    score.AccountId = reader.GetInt32(2);
                    score.SongId = reader.GetInt32(3);
                    score.Difficulty = (DifficultyType) reader.GetInt32(4);
                    score.StageClear = reader.GetInt32(5) > 0;
                    score.MaxCombo = reader.GetInt32(6);
                    score.Kool = reader.GetInt32(7);
                    score.Cool = reader.GetInt32(8);
                    score.Good = reader.GetInt32(9);
                    score.Miss = reader.GetInt32(10);
                    score.Fail = reader.GetInt32(11);
                    score.RawScore = reader.GetInt32(12);
                    score.Rank = (ScoreRankType) reader.GetInt32(13);
                    score.TotalNotes = reader.GetInt32(14);
                    score.ComboType = (ComboType) reader.GetInt32(15);
                    //score.TotalScore = reader.GetInt32(16);
                    score.NoteEffect = (NoteEffectType) reader.GetInt32(17);
                    score.FadeEffect = (FadeEffectType) reader.GetInt32(18);
                    score.Team = (TeamType) reader.GetInt32(19);
                    score.Slot = reader.GetInt32(20);
                    score.Created = reader.GetDateTime(21);
                    score.CharacterName = reader.GetString(22);
                }
            });
            return score;
        }

        public List<Score> SelectBestScores(int songId, DifficultyType difficulty, int scoreCount = -1)
        {
            string sql = String.Join(Seperator,
                "SELECT `id`, `game_id`, `score`.`account_id`, `song_id`, `difficulty`, `stage_clear`, `score`.`max_combo`, `kool`, `cool`, `good`,",
                "`miss`, `fail`, `raw_score`, `rank`, `total_notes`, `combo_type`, `total_score`, `note_effect`, `fade_effect`,",
                "`team`, `slot`, `created`, `character`.`name`",
                "FROM `score`",
                "INNER JOIN `character` on `character`.`account_id` = `score`.`account_id`",
                "WHERE",
                $"`song_id`={songId} AND `difficulty`={(int) difficulty}",
                "ORDER BY `total_score` DESC"
            );

            if (scoreCount > 0)
            {
                sql += $" LIMIT {scoreCount}";
            }

            sql += ";";

            List<Score> scores = new List<Score>();
            ExecuteReader(sql, reader =>
            {
                while (reader.Read())
                {
                    Score score = new Score();
                    score.Id = reader.GetInt32(0);
                    score.GameId = reader.GetInt32(1);
                    score.AccountId = reader.GetInt32(2);
                    score.SongId = reader.GetInt32(3);
                    score.Difficulty = (DifficultyType) reader.GetInt32(4);
                    score.StageClear = reader.GetInt32(5) > 0;
                    score.MaxCombo = reader.GetInt32(6);
                    score.Kool = reader.GetInt32(7);
                    score.Cool = reader.GetInt32(8);
                    score.Good = reader.GetInt32(9);
                    score.Miss = reader.GetInt32(10);
                    score.Fail = reader.GetInt32(11);
                    score.RawScore = reader.GetInt32(12);
                    score.Rank = (ScoreRankType) reader.GetInt32(13);
                    score.TotalNotes = reader.GetInt32(14);
                    score.ComboType = (ComboType) reader.GetInt32(15);
                    //score.TotalScore = reader.GetInt32(16);
                    score.NoteEffect = (NoteEffectType) reader.GetInt32(17);
                    score.FadeEffect = (FadeEffectType) reader.GetInt32(18);
                    score.Team = (TeamType) reader.GetInt32(19);
                    score.Slot = reader.GetInt32(20);
                    score.Created = reader.GetDateTime(21);
                    score.CharacterName = reader.GetString(22);
                    scores.Add(score);
                }
            });
            return scores;
        }

        public bool InsertScore(Score score)
        {
            string sqlCreate = String.Join(Seperator,
                "INSERT INTO `score`",
                "(`game_id`, `account_id`, `song_id`, `difficulty`, `stage_clear`, `max_combo`, `kool`, `cool`, `good`,",
                "`miss`, `fail`, `raw_score`, `rank`, `total_notes`, `combo_type`, `total_score`, `note_effect`, `fade_effect`,",
                "`team`, `slot`, `created`)",
                "VALUES",
                "(",
                $"{score.GameId},",
                $"{score.AccountId},",
                $"{score.SongId},",
                $"{(int) score.Difficulty},",
                $"{(score.StageClear ? 1 : 0)},",
                $"{score.MaxCombo},",
                $"{score.Kool},",
                $"{score.Cool},",
                $"{score.Good},",
                $"{score.Miss},",
                $"{score.Fail},",
                $"{score.RawScore},",
                $"{(int) score.Rank},",
                $"{score.TotalNotes},",
                $"{(int) score.ComboType},",
                $"{score.TotalScore},",
                $"{(int) score.NoteEffect},",
                $"{(int) score.FadeEffect},",
                $"{(int) score.Team},",
                $"{score.Slot},",
                $"'{score.Created:yyyy-MM-dd HH:mm:ss}'",
                ");"
            );
            int affectedRows = ExecuteNonQuery(sqlCreate, out int autoIncrement);
            score.Id = autoIncrement;
            return affectedRows > 0;
        }

        public bool InsertGame(Game game)
        {
            string sqlCreate = String.Join(Seperator,
                "INSERT INTO `game`",
                "(`group_type`, `type`, `name`)",
                "VALUES",
                "(",
                $"{(int) game.GroupType},",
                $"{(int) game.Type},",
                $"'{game.Name}'",
                ");"
            );
            int affectedRows = ExecuteNonQuery(sqlCreate, out int autoIncrement);
            game.Id = autoIncrement;
            return affectedRows > 0;
        }

        public bool UpsertMeta(DatabaseMeta meta)
        {
            string sqlUpate = String.Join(Seperator,
                "UPDATE `meta`",
                "SET",
                $"`version`={meta.Version},",
                $"`read_item`={(meta.ReadItemData ? 1 : 0)},",
                $"`read_quest`={(meta.ReadQuestData ? 1 : 0)},",
                $"`read_song`={(meta.ReadSongData ? 1 : 0)}",
                "WHERE",
                $"`version`={meta.Version};"
            );
            int rowsAffected = ExecuteNonQuery(sqlUpate);
            if (rowsAffected == 0)
            {
                string sqlCreate = String.Join(Seperator,
                    "INSERT INTO `meta`",
                    "(`version`, `read_item`, `read_quest`, `read_song`)",
                    "VALUES",
                    "(",
                    $"{meta.Version},",
                    $"{(meta.ReadItemData ? 1 : 0)},",
                    $"{(meta.ReadQuestData ? 1 : 0)},",
                    $"{(meta.ReadSongData ? 1 : 0)}",
                    ");"
                );
                ExecuteNonQuery(sqlCreate);
            }

            return true;
        }

        public DatabaseMeta SelectMeta(int version)
        {
            string sql = String.Join(Seperator,
                "SELECT `version`, `read_item`, `read_quest`, `read_song`",
                "FROM `meta`",
                "WHERE",
                $"`version`={version};"
            );
            DatabaseMeta meta = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.HasRows && reader.Read())
                {
                    meta = new DatabaseMeta(version);
                    meta.Version = reader.GetInt32(0);
                    meta.ReadItemData = reader.GetInt32(1) > 0;
                    meta.ReadQuestData = reader.GetInt32(2) > 0;
                    meta.ReadSongData = reader.GetInt32(3) > 0;
                }
            });
            return meta;
        }

        private void CreateDatabase()
        {
            if (_databasePath != MemoryDatabasePath && !File.Exists(_databasePath))
            {
                FileStream fs = File.Create(_databasePath);
                fs.Close();
                fs.Dispose();
            }
        }

        private void CreateTables()
        {
            string meta = String.Join(Seperator,
                "CREATE TABLE IF NOT EXISTS `meta` (",
                "`version` INTEGER PRIMARY KEY NOT NULL,",
                "`read_item` INTEGER NOT NULL,",
                "`read_quest` INTEGER NOT NULL,",
                "`read_song` INTEGER NOT NULL",
                ");"
            );
            ExecuteNonQuery(meta);

            string account = String.Join(Seperator,
                "CREATE TABLE IF NOT EXISTS `account` (",
                "`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,",
                "`name` VARCHAR(17) CONSTRAINT `uq_account_name` UNIQUE NOT NULL,",
                "`hash` VARCHAR(255) NOT NULL,",
                "`state` INTEGER NOT NULL",
                ");"
            );
            ExecuteNonQuery(account);

            string character = String.Join(Seperator,
                "CREATE TABLE IF NOT EXISTS `character` (",
                "`account_id` INTEGER REFERENCES `account` (`id`) PRIMARY KEY NOT NULL,",
                "`name` VARCHAR(17) CONSTRAINT `uq_character_name` UNIQUE NOT NULL,",
                "`sex` INTEGER NOT NULL,",
                "`level` INTEGER NOT NULL,",
                "`ruby_exr` INTEGER NOT NULL,",
                "`street_exr` INTEGER NOT NULL,",
                "`club_exr` INTEGER NOT NULL,",
                "`exp` INTEGER NOT NULL,",
                "`coin` INTEGER NOT NULL,",
                "`cash` INTEGER NOT NULL,",
                "`max_combo` INTEGER NOT NULL,",
                "`ruby_wins` INTEGER NOT NULL,",
                "`street_wins` INTEGER NOT NULL,",
                "`club_wins` INTEGER NOT NULL,",
                "`ruby_loses` INTEGER NOT NULL,",
                "`street_loses` INTEGER NOT NULL,",
                "`club_loses` INTEGER NOT NULL,",
                "`premium` INTEGER NOT NULL",
                ");"
            );
            ExecuteNonQuery(character);

            string setting = String.Join(Seperator,
                "CREATE TABLE IF NOT EXISTS `setting` (",
                "`account_id` INTEGER REFERENCES `account` (`id`) PRIMARY KEY NOT NULL,",
                "`ruby_key_on_1` INTEGER NOT NULL,",
                "`ruby_key_on_2` INTEGER NOT NULL,",
                "`ruby_key_on_3` INTEGER NOT NULL,",
                "`ruby_key_on_4` INTEGER NOT NULL,",
                "`ruby_key_ac_1` INTEGER NOT NULL,",
                "`ruby_key_ac_2` INTEGER NOT NULL,",
                "`ruby_key_ac_3` INTEGER NOT NULL,",
                "`ruby_key_ac_4` INTEGER NOT NULL,",
                "`street_key_on_1` INTEGER NOT NULL,",
                "`street_key_on_2` INTEGER NOT NULL,",
                "`street_key_on_3` INTEGER NOT NULL,",
                "`street_key_on_4` INTEGER NOT NULL,",
                "`street_key_on_5` INTEGER NOT NULL,",
                "`street_key_ac_1` INTEGER NOT NULL,",
                "`street_key_ac_2` INTEGER NOT NULL,",
                "`street_key_ac_3` INTEGER NOT NULL,",
                "`street_key_ac_4` INTEGER NOT NULL,",
                "`street_key_ac_5` INTEGER NOT NULL,",
                "`club_key_on_1` INTEGER NOT NULL,",
                "`club_key_on_2` INTEGER NOT NULL,",
                "`club_key_on_3` INTEGER NOT NULL,",
                "`club_key_on_4` INTEGER NOT NULL,",
                "`club_key_on_5` INTEGER NOT NULL,",
                "`club_key_on_6` INTEGER NOT NULL,",
                "`club_key_ac_1` INTEGER NOT NULL,",
                "`club_key_ac_2` INTEGER NOT NULL,",
                "`club_key_ac_3` INTEGER NOT NULL,",
                "`club_key_ac_4` INTEGER NOT NULL,",
                "`club_key_ac_5` INTEGER NOT NULL,",
                "`club_key_ac_6` INTEGER NOT NULL,",
                "`volume_menu_music` INTEGER NOT NULL,",
                "`volume_menu_sfx` INTEGER NOT NULL,",
                "`volume_game_music` INTEGER NOT NULL,",
                "`volume_game_sfx` INTEGER NOT NULL,",
                "`bga_settings` INTEGER NOT NULL,",
                "`skin_position` INTEGER NOT NULL",
                ");"
            );
            ExecuteNonQuery(setting);

            string item = String.Join(Seperator,
                "CREATE TABLE IF NOT EXISTS `item` (",
                "`id` INTEGER PRIMARY KEY NOT NULL,",
                "`name` VARCHAR(255) NOT NULL,",
                "`effect` VARCHAR(255) NOT NULL,",
                "`image` VARCHAR(255) NOT NULL,",
                "`duration` INTEGER NOT NULL,",
                "`coins` INTEGER NOT NULL,",
                "`level` INTEGER NOT NULL,",
                "`exp_plus` INTEGER NOT NULL,",
                "`coin_plus` INTEGER NOT NULL,",
                "`hp_plus` INTEGER NOT NULL,",
                "`resilience_plus` INTEGER NOT NULL,",
                "`defense_plus` INTEGER NOT NULL,",
                "`a` INTEGER NOT NULL,",
                "`b` INTEGER NOT NULL,",
                "`k` INTEGER NOT NULL,",
                "`l` INTEGER NOT NULL,",
                "`m` INTEGER NOT NULL,",
                "`n` INTEGER NOT NULL,",
                "`o` INTEGER NOT NULL,",
                "`q` INTEGER NOT NULL,",
                "`type` INTEGER NOT NULL,",
                "`s` INTEGER NOT NULL,",
                "`t` INTEGER NOT NULL,",
                "`u` INTEGER NOT NULL",
                ");"
            );
            ExecuteNonQuery(item);

            string inventory = String.Join(Seperator,
                "CREATE TABLE IF NOT EXISTS `inventory` (",
                "`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,",
                "`account_id` INTEGER REFERENCES `account` (`id`) NOT NULL,",
                "`item_id` INTEGER REFERENCES `item` (`id`) NOT NULL,",
                "`purchase_date` VARCHAR(255) NOT NULL,",
                "`slot` INTEGER NOT NULL,",
                "`equipped` INTEGER NOT NULL",
                ");"
            );
            ExecuteNonQuery(inventory);

            string quest = String.Join(Seperator,
                "CREATE TABLE IF NOT EXISTS `quest` (",
                "`id` INTEGER PRIMARY KEY NOT NULL,",
                "`a` INTEGER NOT NULL,",
                "`b` INTEGER NOT NULL,",
                "`c` INTEGER NOT NULL,",
                "`d` INTEGER NOT NULL,",
                "`title` VARCHAR(255) NOT NULL,",
                "`mission` VARCHAR(255) NOT NULL,",
                "`g` INTEGER NOT NULL,",
                "`h` INTEGER NOT NULL,",
                "`i` INTEGER NOT NULL,",
                "`j` INTEGER NOT NULL,",
                "`k` INTEGER NOT NULL,",
                "`l` INTEGER NOT NULL,",
                "`m` INTEGER NOT NULL,",
                "`n` INTEGER NOT NULL,",
                "`o` INTEGER NOT NULL,",
                "`p` INTEGER NOT NULL,",
                "`q` INTEGER NOT NULL,",
                "`r` INTEGER NOT NULL,",
                "`s` INTEGER NOT NULL,",
                "`t` INTEGER NOT NULL,",
                "`u` INTEGER NOT NULL,",
                "`v` INTEGER NOT NULL,",
                "`w` INTEGER NOT NULL,",
                "`x` INTEGER NOT NULL,",
                "`y` INTEGER NOT NULL,",
                "`z` INTEGER NOT NULL,",
                "`z1` INTEGER NOT NULL,",
                "`z2` INTEGER NOT NULL,",
                "`z3` INTEGER NOT NULL,",
                "`z4` INTEGER NOT NULL,",
                "`z5` INTEGER NOT NULL,",
                "`z6` INTEGER NOT NULL,",
                "`z7` INTEGER NOT NULL,",
                "`z8` INTEGER NOT NULL,",
                "`z9` INTEGER NOT NULL,",
                "`z10` INTEGER NOT NULL,",
                "`z11` INTEGER NOT NULL",
                ");"
            );
            ExecuteNonQuery(quest);

            string song = String.Join(Seperator,
                "CREATE TABLE IF NOT EXISTS `song` (",
                "`id` INTEGER PRIMARY KEY NOT NULL,",
                "`name` VARCHAR(255) NOT NULL,",
                "`b` VARCHAR(255) NOT NULL,",
                "`duration` VARCHAR(255) NOT NULL,",
                "`bpm` INTEGER NOT NULL,",
                "`e` VARCHAR(255) NOT NULL,",
                "`d1` INTEGER NOT NULL,",
                "`d2` INTEGER NOT NULL,",
                "`ruby_ez_difficulty` INTEGER NOT NULL,",
                "`d4` INTEGER NOT NULL,",
                "`ruby_ez_notes` INTEGER NOT NULL,",
                "`d6` INTEGER NOT NULL,",
                "`d7` INTEGER NOT NULL,",
                "`d8` INTEGER NOT NULL,",
                "`d9` INTEGER NOT NULL,",
                "`d10` INTEGER NOT NULL,",
                "`d11` INTEGER NOT NULL,",
                "`d12` INTEGER NOT NULL,",
                "`d13` INTEGER NOT NULL,",
                "`d14` INTEGER NOT NULL,",
                "`d15` INTEGER NOT NULL,",
                "`d16` INTEGER NOT NULL,",
                "`d17` INTEGER NOT NULL,",
                "`ruby_shd_difficulty` INTEGER NOT NULL,",
                "`d19` INTEGER NOT NULL,",
                "`ruby_shd_notes` INTEGER NOT NULL,",
                "`d21` INTEGER NOT NULL,",
                "`d22` INTEGER NOT NULL,",
                "`d23` INTEGER NOT NULL,",
                "`d24` INTEGER NOT NULL,",
                "`d25` INTEGER NOT NULL,",
                "`d26` INTEGER NOT NULL,",
                "`d27` INTEGER NOT NULL,",
                "`d28` INTEGER NOT NULL,",
                "`d29` INTEGER NOT NULL,",
                "`d30` INTEGER NOT NULL,",
                "`d31` INTEGER NOT NULL,",
                "`d32` INTEGER NOT NULL,",
                "`d33` INTEGER NOT NULL,",
                "`d34` INTEGER NOT NULL,",
                "`d35` INTEGER NOT NULL,",
                "`d36` INTEGER NOT NULL,",
                "`d37` INTEGER NOT NULL,",
                "`d38` INTEGER NOT NULL,",
                "`d39` INTEGER NOT NULL,",
                "`d40` INTEGER NOT NULL,",
                "`d41` INTEGER NOT NULL,",
                "`d42` INTEGER NOT NULL,",
                "`d43` INTEGER NOT NULL,",
                "`d44` INTEGER NOT NULL,",
                "`d45` INTEGER NOT NULL,",
                "`d46` INTEGER NOT NULL,",
                "`d47` INTEGER NOT NULL,",
                "`d48` INTEGER NOT NULL,",
                "`d49` INTEGER NOT NULL,",
                "`d50` INTEGER NOT NULL,",
                "`d51` INTEGER NOT NULL,",
                "`d52` INTEGER NOT NULL,",
                "`club_hd_difficulty` INTEGER NOT NULL,",
                "`d54` INTEGER NOT NULL,",
                "`club_hd_notes` INTEGER NOT NULL,",
                "`d56` INTEGER NOT NULL,",
                "`d57` INTEGER NOT NULL,",
                "`d58` INTEGER NOT NULL,",
                "`d59` INTEGER NOT NULL,",
                "`club_shd_notes` INTEGER NOT NULL,",
                "`d61` INTEGER NOT NULL",
                ");"
            );
            ExecuteNonQuery(song);

            string game = String.Join(Seperator,
                "CREATE TABLE IF NOT EXISTS `game` (",
                "`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,",
                "`group_type` INTEGER NOT NULL,",
                "`type` INTEGER NOT NULL,",
                "`name` VARCHAR(255) NOT NULL",
                ");"
            );
            ExecuteNonQuery(game);

            string score = String.Join(Seperator,
                "CREATE TABLE IF NOT EXISTS `score` (",
                "`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,",
                "`game_id` INTEGER REFERENCES `game` (`id`) NOT NULL,",
                "`account_id` INTEGER REFERENCES `account` (`id`) NOT NULL,",
                "`song_id` INTEGER REFERENCES `song` (`id`) NOT NULL,",
                "`difficulty` INTEGER NOT NULL,",
                "`stage_clear` INTEGER NOT NULL,",
                "`max_combo` INTEGER NOT NULL,",
                "`kool` INTEGER NOT NULL,",
                "`cool` INTEGER NOT NULL,",
                "`good` INTEGER NOT NULL,",
                "`miss` INTEGER NOT NULL,",
                "`fail` INTEGER NOT NULL,",
                "`raw_score` INTEGER NOT NULL,",
                "`rank` INTEGER NOT NULL,",
                "`total_notes` INTEGER NOT NULL,",
                "`combo_type` INTEGER NOT NULL,",
                "`total_score` INTEGER NOT NULL,",
                "`note_effect` INTEGER NOT NULL,",
                "`fade_effect` INTEGER NOT NULL,",
                "`team` INTEGER NOT NULL,",
                "`slot` INTEGER NOT NULL,",
                "`created` VARCHAR(255) NOT NULL",
                ");"
            );

            ExecuteNonQuery(score);
        }

        /// <summary>
        /// Execute the command and return a reader.
        /// </summary>
        /// <param name="sql"></param>
        /// <exception cref="SQLiteException">If the query failed. ResultCode will provide the reason.</exception>
        private void ExecuteReader(string sql, Action<SQLiteDataReader> readAction)
        {
            using (SQLiteConnection connection = NewConnection())
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    readAction(reader);
                }
            }
        }

        /// <summary>
        /// Execute the command and return the number of rows inserted/updated affected by it.
        /// </summary>
        /// <param name="sql"></param>
        /// <exception cref="SQLiteException">If the query failed. ResultCode will provide the reason.</exception>
        /// <returns>The number of rows inserted/updated affected by it.</returns>
        private int ExecuteNonQuery(string sql)
        {
            using (SQLiteConnection connection = NewConnection())
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
        }


        private bool Upsert(string table, string whereColumn, object whereValue, string[] parameters, object[] values)
        {
            int rowsAffected;
            using (SQLiteConnection connection = NewConnection())
            {
                int length = parameters.Length;
                if (length != values.Length)
                {
                    throw new Exception("parameters length doesn't match values length");
                }

                SQLiteCommand command = new SQLiteCommand(connection);
                StringBuilder query = new StringBuilder($"UPDATE `{table}` SET ");
                for (int i = 0; i < length; i++)
                {
                    string parameter = parameters[i];
                    query.Append($"`{parameter}`=@{parameter}");
                    if (i < length - 1)
                    {
                        query.Append(", ");
                    }

                    command.Parameters.AddWithValue($"@{parameter}", values[i]);
                }

                query.Append($" WHERE `{whereColumn}`=@{whereColumn};");
                command.Parameters.AddWithValue($"@{whereColumn}", whereValue);
                command.CommandText = query.ToString();

                rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    command = new SQLiteCommand(connection);
                    query = new StringBuilder($"INSERT INTO `{table}` (");

                    for (int i = 0; i < length; i++)
                    {
                        string parameter = parameters[i];
                        query.Append($"`{parameter}`");
                        if (i < length - 1)
                        {
                            query.Append(", ");
                        }
                    }

                    query.Append(") VALUES (");
                    for (int i = 0; i < length; i++)
                    {
                        string parameter = parameters[i];
                        query.Append($"@{parameter}");
                        if (i < length - 1)
                        {
                            query.Append(", ");
                        }

                        command.Parameters.AddWithValue($"@{parameter}", values[i]);
                    }

                    query.Append(");");
                    command.CommandText = query.ToString();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0;
        }

        private int ExecuteNonQuery(string sql, out int autoIncrement)
        {
            using (SQLiteConnection connection = NewConnection())
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                int rowsAffected = command.ExecuteNonQuery();
                autoIncrement = GetAutoIncrement(connection);
                return rowsAffected;
            }
        }

        private int GetAutoIncrement(SQLiteConnection connection)
        {
            return (int) connection.LastInsertRowId;
        }

        private string BuildConnectionString(string source)
        {
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = source;
            builder.Version = 3;
            builder.ForeignKeys = true;

            _connectionString = builder.ToString();
            _logger.Info("Connection String: {0}", _connectionString);
            return _connectionString;
        }

        private SQLiteConnection NewConnection()
        {
            SQLiteConnection connection;
            if (_connectionString != null)
            {
                connection = new SQLiteConnection(_connectionString);
            }
            else
            {
                connection = new SQLiteConnection(BuildConnectionString(_databasePath));
            }

            return connection.OpenAndReturn();
        }
    }
}