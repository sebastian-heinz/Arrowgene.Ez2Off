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
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.Server.Database.Sql
{
    public class SqLiteDb : IDatabase
    {
        public const string MemoryDatabasePath = ":memory:";


        private static readonly ILogger _logger = LogProvider.Logger(typeof(SqLiteDb));

        private const string Separator = " ";
        private const long NoAutoIncrement = long.MaxValue;

        private readonly string _databasePath;
        private string _connectionString;

        public SqLiteDb(string databasePath)
        {
            _databasePath = databasePath;
            _logger.Info($"Database Path: {_databasePath}");
            CreateDatabase();
        }

        public void Execute(string sql)
        {
            using (SQLiteConnection connection = NewConnection())
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        private void ExecuteReader(string sql, Action<DbDataReader> readAction)
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

        private int ExecuteNonQuery(string sql)
        {
            using (SQLiteConnection connection = NewConnection())
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
        }

        private int ExecuteNonQuery(string sql, out long autoIncrement)
        {
            using (SQLiteConnection connection = NewConnection())
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                int rowsAffected = command.ExecuteNonQuery();
                autoIncrement = GetAutoIncrement(connection);
                return rowsAffected;
            }
        }

        private string Escape(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            StringBuilder sb = null;
            int last = -1;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '\'' || value[i] == '\"' || value[i] == '\\')
                {
                    if (sb == null)
                        sb = new StringBuilder();
                    sb.Append(value, last + 1, i - (last + 1));
                    sb.Append('\\');
                    sb.Append(value[i]);
                    last = i;
                }
            }

            if (sb != null)
                sb.Append(value, last + 1, value.Length - (last + 1));

            return sb?.ToString() ?? value;
        }

        private DateTime? GetNullable(DbDataReader reader, int ordinal, Func<int, DateTime> getValue)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return getValue(ordinal);
        }

        private string GetNullable(DbDataReader reader, int ordinal, Func<int, string> getValue)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return getValue(ordinal);
        }

        private string SetNullable(DateTime? value)
        {
            if (value == null)
            {
                return "null";
            }

            return $"'{value:yyyy-MM-dd HH:mm:ss}'";
        }

        private string SetNullable(string value)
        {
            if (value == null)
            {
                return "null";
            }

            return $"'{value}'";
        }

        private string SetNullable(int? value)
        {
            if (value == null)
            {
                return "null";
            }

            return $"{value}";
        }

        /// <summary>
        /// Updates or Inserts a row.
        /// 
        /// - Returns an integer indicating how many rows were updated.
        ///   If the value is 0, the operation failed.
        /// 
        /// - autoIncrement is only available for a insert function,
        ///   otherwise a value of <see cref="NoAutoIncrement"/> will be returned.
        /// </summary>
        private int Upsert(string table, string[] columns, object[] values, string whereColumn, object whereValue,
            out long autoIncrement)
        {
            int rowsAffected;
            using (SQLiteConnection connection = NewConnection())
            {
                int length = columns.Length;
                if (length != values.Length)
                {
                    throw new Exception("parameters length doesn't match values length");
                }

                SQLiteCommand command = new SQLiteCommand(connection);
                StringBuilder query = new StringBuilder($"UPDATE `{table}` SET ");
                for (int i = 0; i < length; i++)
                {
                    string parameter = columns[i];
                    query.Append($"`{parameter}`=@{parameter}");
                    if (i < length - 1)
                    {
                        query.Append(", ");
                    }

                    command.Parameters.AddWithValue($"@{parameter}", values[i]);
                }

                query.Append($" WHERE `{whereColumn}`=@whereColumn;");
                command.Parameters.AddWithValue("@whereColumn", whereValue);
                command.CommandText = query.ToString();
                try
                {
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    rowsAffected = 0;
                }

                if (rowsAffected > 0)
                {
                    autoIncrement = NoAutoIncrement;
                }
                else
                {
                    command = new SQLiteCommand(connection);
                    query = new StringBuilder($"INSERT INTO `{table}` (");

                    for (int i = 0; i < length; i++)
                    {
                        string parameter = columns[i];
                        query.Append($"`{parameter}`");
                        if (i < length - 1)
                        {
                            query.Append(", ");
                        }
                    }

                    query.Append(") VALUES (");
                    for (int i = 0; i < length; i++)
                    {
                        string parameter = columns[i];
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
                    autoIncrement = GetAutoIncrement(connection);
                }
            }

            return rowsAffected;
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

        private long GetAutoIncrement(SQLiteConnection connection)
        {
            return connection.LastInsertRowId;
        }

        private string BuildConnectionString(string source)
        {
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = source;
            builder.Version = 3;
            builder.ForeignKeys = true;

            string connectionString = builder.ToString();
            _logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }

        private SQLiteConnection NewConnection()
        {
            if (_connectionString == null)
            {
                _connectionString = BuildConnectionString(_databasePath);
            }

            SQLiteConnection connection = new SQLiteConnection(_connectionString);
            return connection.OpenAndReturn();
        }

        public string ServerVersion()
        {
            return NewConnection().ServerVersion;
        }

        public bool SetSetting(string key, string value)
        {
            int rowsAffected;
            string sql = $"UPDATE `setting` SET `value`='{value}' WHERE `key`='{key}';";
            try
            {
                rowsAffected = ExecuteNonQuery(sql);
            }
            catch (Exception)
            {
                rowsAffected = 0;
            }

            if (rowsAffected <= 0)
            {
                sql = $"INSERT INTO `setting` (`key`, `value`) VALUES ('{key}', '{value}');";
                rowsAffected = ExecuteNonQuery(sql);
            }

            return rowsAffected > 0;
        }

        public string GetSetting(string key)
        {
            string value = null;
            string sql = $"SELECT `value` FROM `setting` WHERE `key` = '{key}';";
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    value = reader.GetString(0);
                }
            });
            return value;
        }

        public bool DeleteSetting(string key)
        {
            string sql = $"DELETE FROM `setting` WHERE `key`='{key}';";
            return ExecuteNonQuery(sql) > 0;
        }

        public Account CreateAccount(string name, string hash)
        {
            Account account = new Account();
            string escapedName = Escape(name);
            account.Name = escapedName;
            account.NormalName = escapedName.ToLowerInvariant();
            account.Mail = escapedName;
            account.Hash = hash;
            account.State = AccountState.User;
            account.Created = DateTime.Now;

            string sql = String.Join(Separator,
                "INSERT INTO `account` (`name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`,",
                "`mail_token`, `password_token`, `state`, `last_login`, `created`) VALUES (",
                $"'{account.Name}',",
                $"'{account.NormalName}',",
                $"'{account.Hash}',",
                $"'{account.Mail}',",
                $"{(account.MailVerified ? 1 : 0)},",
                $"{SetNullable(account.MailVerifiedAt)},",
                $"{SetNullable(account.MailToken)},",
                $"{SetNullable(account.PasswordToken)},",
                $"{(int) account.State},",
                $"{SetNullable(account.LastLogin)},",
                $"'{account.Created:yyyy-MM-dd HH:mm:ss}'",
                ");"
            );
            try

            {
                ExecuteNonQuery(sql, out long id);
                account.Id = (int) id;
                return account;
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                return null;
            }
        }

        public bool UpdateAccount(Account account)
        {
            string sql = String.Join(Separator,
                "UPDATE `account` SET",
                $"`name`='{account.Name}',",
                $"`normal_name`='{account.NormalName}',",
                $"`hash`='{account.Hash}',",
                $"`mail`='{account.Mail}',",
                $"`mail_verified`={(account.MailVerified ? 1 : 0)},",
                $"`mail_verified_at`={SetNullable(account.MailVerifiedAt)},",
                $"`mail_token`={SetNullable(account.MailToken)},",
                $"`password_token`={SetNullable(account.PasswordToken)},",
                $"`state`={(int) account.State},",
                $"`last_login`={SetNullable(account.LastLogin)},",
                $"`created`='{account.Created:yyyy-MM-dd HH:mm:ss}'",
                $"WHERE `id`={account.Id};"
            );
            return ExecuteNonQuery(sql) > 0;
        }

        public Account SelectAccount(string accountName)
        {
            Account account = null;
            string sql = String.Join(Separator,
                "SELECT `id`, `name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`,",
                "`mail_token`, `password_token`, `state`, `last_login`, `created` FROM `account`",
                $"WHERE `name`='{accountName}'");
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    account = new Account();
                    account.Id = reader.GetInt32(0);
                    account.Name = reader.GetString(1);
                    account.NormalName = reader.GetString(2);
                    account.Hash = reader.GetString(3);
                    account.Mail = reader.GetString(4);
                    account.MailVerified = reader.GetBoolean(5);
                    account.MailVerifiedAt = GetNullable(reader, 6, reader.GetDateTime);
                    account.MailToken = GetNullable(reader, 7, reader.GetString);
                    account.PasswordToken = GetNullable(reader, 8, reader.GetString);
                    account.State = (AccountState) reader.GetInt32(9);
                    account.LastLogin = GetNullable(reader, 10, reader.GetDateTime);
                    account.Created = reader.GetDateTime(11);
                }
            });
            return account;
        }

        public Setting SelectSetting(int characterId)
        {
            Setting setting = null;
            string sql = String.Join(Separator,
                "SELECT `character_id`, `ruby_key_on_1`, `ruby_key_on_2`, `ruby_key_on_3`, `ruby_key_on_4`, `ruby_key_ac_1`, `ruby_key_ac_2`, `ruby_key_ac_3`, `ruby_key_ac_4`,",
                "`street_key_on_1`, `street_key_on_2`, `street_key_on_3`, `street_key_on_4`, `street_key_on_5`, `street_key_ac_1`, `street_key_ac_2`, `street_key_ac_3`, `street_key_ac_4`, `street_key_ac_5`,",
                "`club_key_on_1`, `club_key_on_2`, `club_key_on_3`, `club_key_on_4`, `club_key_on_5`, `club_key_on_6`, `club_key_ac_1`, `club_key_ac_2`, `club_key_ac_3`, `club_key_ac_4`, `club_key_ac_5`, `club_key_ac_6`,",
                "`volume_menu_music`, `volume_menu_sfx`, `volume_game_music`, `volume_game_sfx`, `bga_settings`, `skin_position`, `skin_type`",
                "FROM `ez2on_setting` WHERE",
                $"`character_id`={characterId};"
            );
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    setting = new Setting();
                    setting.CharacterId = reader.GetInt32(0);
                    setting.KeySettingsRuby.KeyOn1 = reader.GetByte(1);
                    setting.KeySettingsRuby.KeyOn2 = reader.GetByte(2);
                    setting.KeySettingsRuby.KeyOn3 = reader.GetByte(3);
                    setting.KeySettingsRuby.KeyOn4 = reader.GetByte(4);
                    setting.KeySettingsRuby.KeyAc1 = reader.GetByte(5);
                    setting.KeySettingsRuby.KeyAc2 = reader.GetByte(6);
                    setting.KeySettingsRuby.KeyAc3 = reader.GetByte(7);
                    setting.KeySettingsRuby.KeyAc4 = reader.GetByte(8);
                    setting.KeySettingsStreet.KeyOn1 = reader.GetByte(9);
                    setting.KeySettingsStreet.KeyOn2 = reader.GetByte(10);
                    setting.KeySettingsStreet.KeyOn3 = reader.GetByte(11);
                    setting.KeySettingsStreet.KeyOn4 = reader.GetByte(12);
                    setting.KeySettingsStreet.KeyOn5 = reader.GetByte(13);
                    setting.KeySettingsStreet.KeyAc1 = reader.GetByte(14);
                    setting.KeySettingsStreet.KeyAc2 = reader.GetByte(15);
                    setting.KeySettingsStreet.KeyAc3 = reader.GetByte(16);
                    setting.KeySettingsStreet.KeyAc4 = reader.GetByte(17);
                    setting.KeySettingsStreet.KeyAc5 = reader.GetByte(18);
                    setting.KeySettingsClub.KeyOn1 = reader.GetByte(19);
                    setting.KeySettingsClub.KeyOn2 = reader.GetByte(20);
                    setting.KeySettingsClub.KeyOn3 = reader.GetByte(21);
                    setting.KeySettingsClub.KeyOn4 = reader.GetByte(22);
                    setting.KeySettingsClub.KeyOn5 = reader.GetByte(23);
                    setting.KeySettingsClub.KeyOn6 = reader.GetByte(24);
                    setting.KeySettingsClub.KeyAc1 = reader.GetByte(25);
                    setting.KeySettingsClub.KeyAc2 = reader.GetByte(26);
                    setting.KeySettingsClub.KeyAc3 = reader.GetByte(27);
                    setting.KeySettingsClub.KeyAc4 = reader.GetByte(28);
                    setting.KeySettingsClub.KeyAc5 = reader.GetByte(29);
                    setting.KeySettingsClub.KeyAc6 = reader.GetByte(30);
                    setting.VolumeMenuMusic = reader.GetByte(31);
                    setting.VolumeMenuSfx = reader.GetByte(32);
                    setting.VolumeGameMusic = reader.GetByte(33);
                    setting.VolumeGameSfx = reader.GetByte(34);
                    setting.BgaSettings.FromByte(reader.GetByte(35));
                    setting.SkinPosition = reader.GetByte(36);
                    setting.SkinType = reader.GetByte(37);
                }
            });
            return setting;
        }

        public bool UpsertSetting(Setting setting)
        {
            string[] columns =
            {
                "character_id", "ruby_key_on_1", "ruby_key_on_2", "ruby_key_on_3", "ruby_key_on_4", "ruby_key_ac_1",
                "ruby_key_ac_2", "ruby_key_ac_3", "ruby_key_ac_4", "street_key_on_1", "street_key_on_2",
                "street_key_on_3", "street_key_on_4", "street_key_on_5", "street_key_ac_1", "street_key_ac_2",
                "street_key_ac_3", "street_key_ac_4", "street_key_ac_5", "club_key_on_1", "club_key_on_2",
                "club_key_on_3", "club_key_on_4", "club_key_on_5", "club_key_on_6", "club_key_ac_1", "club_key_ac_2",
                "club_key_ac_3", "club_key_ac_4", "club_key_ac_5", "club_key_ac_6", "volume_menu_music",
                "volume_menu_sfx", "volume_game_music", "volume_game_sfx", "bga_settings", "skin_position", "skin_type"
            };
            object[] values =
            {
                setting.CharacterId, setting.KeySettingsRuby.KeyOn1, setting.KeySettingsRuby.KeyOn2,
                setting.KeySettingsRuby.KeyOn3, setting.KeySettingsRuby.KeyOn4,
                setting.KeySettingsRuby.KeyAc1, setting.KeySettingsRuby.KeyAc2,
                setting.KeySettingsRuby.KeyAc3, setting.KeySettingsRuby.KeyAc4,
                setting.KeySettingsStreet.KeyOn1, setting.KeySettingsStreet.KeyOn2,
                setting.KeySettingsStreet.KeyOn3, setting.KeySettingsStreet.KeyOn4,
                setting.KeySettingsStreet.KeyOn5, setting.KeySettingsStreet.KeyAc1,
                setting.KeySettingsStreet.KeyAc2, setting.KeySettingsStreet.KeyAc3,
                setting.KeySettingsStreet.KeyAc4, setting.KeySettingsStreet.KeyAc5,
                setting.KeySettingsClub.KeyOn1, setting.KeySettingsClub.KeyOn2,
                setting.KeySettingsClub.KeyOn3, setting.KeySettingsClub.KeyOn4,
                setting.KeySettingsClub.KeyOn5, setting.KeySettingsClub.KeyOn6,
                setting.KeySettingsClub.KeyAc1, setting.KeySettingsClub.KeyAc2,
                setting.KeySettingsClub.KeyAc3, setting.KeySettingsClub.KeyAc4,
                setting.KeySettingsClub.KeyAc5, setting.KeySettingsClub.KeyAc6,
                setting.VolumeMenuMusic, setting.VolumeMenuSfx, setting.VolumeGameMusic,
                setting.VolumeGameSfx, setting.BgaSettings.ToByte(), setting.SkinPosition,
                setting.SkinType
            };
            try
            {
                Upsert("ez2on_setting", columns, values, "character_id", setting.CharacterId, out long id);
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                return false;
            }

            return true;
        }

        public Character SelectCharacter(int characterId)
        {
            Character character = null;
            string sql = String.Join(Separator,
                "SELECT `id`, `name`, `sex`, `level`, `ruby_exr`, `street_exr`, `club_exr`, `exp`, `coin`, `cash`,",
                "`max_combo`, `ruby_wins`, `street_wins`, `club_wins`, `ruby_loses`, `street_loses`, `club_loses`,",
                "`premium`, `dj_points`, `dj_points_plus` FROM `ez2on_character`",
                "WHERE",
                $"`id`={characterId};"
            );
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    character = new Character();
                    character.Id = reader.GetInt32(0);
                    character.Name = reader.GetString(1);
                    character.Sex = (CharacterSex) reader.GetInt32(2);
                    character.Level = reader.GetByte(3);
                    character.RubyExr = reader.GetInt32(4);
                    character.StreetExr = reader.GetInt32(5);
                    character.ClubExr = reader.GetInt32(6);
                    character.Exp = reader.GetInt32(7);
                    character.Coin = reader.GetInt32(8);
                    character.Cash = reader.GetInt32(9);
                    character.MaxCombo = reader.GetInt16(10);
                    character.RubyWins = reader.GetInt32(11);
                    character.StreetWins = reader.GetInt32(12);
                    character.ClubWins = reader.GetInt32(13);
                    character.RubyLoses = reader.GetInt32(14);
                    character.StreetLoses = reader.GetInt32(15);
                    character.ClubLoses = reader.GetInt32(16);
                    character.Premium = reader.GetInt16(17);
                    character.DjPoints = reader.GetInt16(18);
                    character.DjPointsPlus = reader.GetInt16(19);
                }
            });
            return character;
        }

        public bool UpsertCharacter(Character character)
        {
            string[] columns =
            {
                "id", "name", "sex", "level", "ruby_exr", "street_exr", "club_exr", "exp", "coin",
                "cash", "max_combo", "ruby_wins", "street_wins", "club_wins", "ruby_loses", "street_loses",
                "club_loses", "premium", "dj_points", "dj_points_plus"
            };
            object[] values =
            {
                character.Id, character.Name, (int) character.Sex, character.Level,
                character.RubyExr, character.StreetExr, character.ClubExr, character.Exp,
                character.Coin, character.Cash, character.MaxCombo, character.RubyWins,
                character.StreetWins, character.ClubWins, character.RubyLoses,
                character.StreetLoses, character.ClubLoses, character.Premium,
                character.DjPoints, character.DjPointsPlus
            };
            try
            {
                Upsert("ez2on_character", columns, values, "id", character.Id, out long id);
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                return false;
            }

            return true;
        }

        public Character SelectCharacter(string characterName)
        {
            Character character = null;
            string sql = String.Join(Separator,
                "SELECT `id`, `name`, `sex`, `level`, `ruby_exr`, `street_exr`, `club_exr`, `exp`, `coin`, `cash`,",
                "`max_combo`, `ruby_wins`, `street_wins`, `club_wins`, `ruby_loses`, `street_loses`, `club_loses`,",
                "`premium`, `dj_points`, `dj_points_plus` FROM `ez2on_character`",
                "WHERE",
                $"`name`='{characterName}';"
            );
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    character = new Character();
                    character.Id = reader.GetInt32(0);
                    character.Name = reader.GetString(1);
                    character.Sex = (CharacterSex) reader.GetInt32(2);
                    character.Level = reader.GetByte(3);
                    character.RubyExr = reader.GetInt32(4);
                    character.StreetExr = reader.GetInt32(5);
                    character.ClubExr = reader.GetInt32(6);
                    character.Exp = reader.GetInt32(7);
                    character.Coin = reader.GetInt32(8);
                    character.Cash = reader.GetInt32(9);
                    character.MaxCombo = reader.GetInt16(10);
                    character.RubyWins = reader.GetInt32(11);
                    character.StreetWins = reader.GetInt32(12);
                    character.ClubWins = reader.GetInt32(13);
                    character.RubyLoses = reader.GetInt32(14);
                    character.StreetLoses = reader.GetInt32(15);
                    character.ClubLoses = reader.GetInt32(16);
                    character.Premium = reader.GetInt16(17);
                    character.DjPoints = reader.GetInt16(18);
                    character.DjPointsPlus = reader.GetInt16(19);
                }
            });
            return character;
        }

        public Item SelectItem(int itemId)
        {
            string sql = String.Join(Separator,
                "SELECT `name`, `effect`, `image`, `duration`, `price`, `level`, `exp_plus`, `coin_plus`, `hp_plus`,",
                "`resilience_plus`, `defense_plus`, `type`, `enabled`, `currency`",
                "FROM `ez2on_item`",
                "WHERE",
                $"`id`={itemId};"
            );
            Item item = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    item = new Item();
                    item.Id = itemId;
                    item.Name = reader.GetString(0);
                    item.Effect = reader.GetString(1);
                    item.Image = reader.GetString(2);
                    item.Duration = reader.GetInt32(3);
                    item.Price = reader.GetInt32(4);
                    item.Level = reader.GetInt32(5);
                    item.ExpPlus = reader.GetInt32(6);
                    item.CoinPlus = reader.GetInt32(7);
                    item.HpPlus = reader.GetInt32(8);
                    item.ResiliencePlus = reader.GetInt32(9);
                    item.DefensePlus = reader.GetInt32(10);
                    item.Type = (ItemType) reader.GetInt32(11);
                    item.Enabled = reader.GetBoolean(12);
                    item.Currency = (ItemCurrencyType) reader.GetInt32(13);
                }
            });
            return item;
        }

        public bool UpsertItem(Item item)
        {
            string[] columns =
            {
                "id", "name", "effect", "image", "duration", "price", "level", "exp_plus", "coin_plus",
                "hp_plus", "resilience_plus", "defense_plus", "type", "enabled", "currency"
            };
            object[] values =
            {
                item.Id, item.Name, item.Effect, item.Image, item.Duration,
                item.Price, item.Level, item.ExpPlus, item.CoinPlus, item.HpPlus,
                item.ResiliencePlus, item.DefensePlus, (int) item.Type, item.Enabled ? 1 : 0, (int) item.Currency
            };
            try
            {
                Upsert("ez2on_item", columns, values, "id", item.Id, out long id);
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                return false;
            }

            return true;
        }

        public List<Item> SelectItems()
        {
            List<Item> items = new List<Item>();
            string sql = String.Join(Separator,
                "SELECT `id`, `name`, `effect`, `image`, `duration`, `price`, `level`, `exp_plus`, `coin_plus`, `hp_plus`,",
                "`resilience_plus`, `defense_plus`, `type`, `enabled`, `currency`",
                "FROM `ez2on_item`;"
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
                    item.Price = reader.GetInt32(5);
                    item.Level = reader.GetInt32(6);
                    item.ExpPlus = reader.GetInt32(7);
                    item.CoinPlus = reader.GetInt32(8);
                    item.HpPlus = reader.GetInt32(9);
                    item.ResiliencePlus = reader.GetInt32(10);
                    item.DefensePlus = reader.GetInt32(11);
                    item.Type = (ItemType) reader.GetInt32(12);
                    item.Enabled = reader.GetBoolean(13);
                    item.Currency = (ItemCurrencyType) reader.GetInt32(14);
                    items.Add(item);
                }
            });
            return items;
        }

        public InventoryItem SelectInventoryItem(int inventoryItemId)
        {
            string sql = String.Join(Separator,
                "SELECT `ez2on_item`.`id`, `ez2on_item`.`name`, `ez2on_item`.`effect`, `ez2on_item`.`image`,",
                "`ez2on_item`.`duration`, `ez2on_item`.`price`, `ez2on_item`.`level`, `ez2on_item`.`exp_plus`,",
                "`ez2on_item`.`coin_plus`, `ez2on_item`.`hp_plus`, `ez2on_item`.`resilience_plus`,",
                "`ez2on_item`.`defense_plus`, `ez2on_item`.`type`, `ez2on_item`.`enabled`, `ez2on_item`.`currency`,",
                "`ez2on_inventory`.`character_id`, `ez2on_inventory`.`purchase_date`, `ez2on_inventory`.`slot`,",
                "`ez2on_inventory`.`equipped`, `ez2on_inventory`.`equip_date`, `ez2on_inventory`.`expire_date`",
                "FROM `ez2on_inventory`",
                "INNER JOIN `ez2on_item` on `ez2on_item`.`id` = `ez2on_inventory`.`item_id`",
                "WHERE",
                $"`ez2on_inventory`.`id`={inventoryItemId};"
            );
            InventoryItem inventoryItem = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    Item item = new Item();
                    item.Id = reader.GetInt32(0);
                    item.Name = reader.GetString(1);
                    item.Effect = reader.GetString(2);
                    item.Image = reader.GetString(3);
                    item.Duration = reader.GetInt32(4);
                    item.Price = reader.GetInt32(5);
                    item.Level = reader.GetInt32(6);
                    item.ExpPlus = reader.GetInt32(7);
                    item.CoinPlus = reader.GetInt32(8);
                    item.HpPlus = reader.GetInt32(9);
                    item.ResiliencePlus = reader.GetInt32(10);
                    item.DefensePlus = reader.GetInt32(11);
                    item.Type = (ItemType) reader.GetInt32(12);
                    item.Enabled = reader.GetBoolean(13);
                    item.Currency = (ItemCurrencyType) reader.GetInt32(14);
                    inventoryItem = new InventoryItem();
                    inventoryItem.Id = inventoryItemId;
                    inventoryItem.CharacterId = reader.GetInt32(15);
                    inventoryItem.PurchaseDate = reader.GetDateTime(16);
                    inventoryItem.Slot = reader.GetInt32(17);
                    inventoryItem.Equipped = reader.GetInt32(18);
                    inventoryItem.EquipDate = GetNullable(reader, 19, reader.GetDateTime);
                    inventoryItem.ExpireDate = GetNullable(reader, 20, reader.GetDateTime);
                    inventoryItem.Item = item;
                }
            });
            return inventoryItem;
        }

        public List<InventoryItem> SelectExpiredInventoryItems()
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();
            DateTime now = DateTime.Now;
            string sql = String.Join(Separator,
                "SELECT `ez2on_item`.`id`, `ez2on_item`.`name`, `ez2on_item`.`effect`, `ez2on_item`.`image`,",
                "`ez2on_item`.`duration`, `ez2on_item`.`price`, `ez2on_item`.`level`, `ez2on_item`.`exp_plus`,",
                "`ez2on_item`.`coin_plus`, `ez2on_item`.`hp_plus`, `ez2on_item`.`resilience_plus`,",
                "`ez2on_item`.`defense_plus`, `ez2on_item`.`type`, `ez2on_item`.`enabled`, `ez2on_item`.`currency`,",
                "`ez2on_inventory`.`id`, `ez2on_inventory`.`character_id`, `ez2on_inventory`.`purchase_date`,",
                "`ez2on_inventory`.`slot`, `ez2on_inventory`.`equipped`, `ez2on_inventory`.`equip_date`, `ez2on_inventory`.`expire_date`",
                "FROM `ez2on_inventory`",
                "INNER JOIN `ez2on_item` on `ez2on_item`.`id` = `ez2on_inventory`.`item_id`",
                "WHERE",
                $"`ez2on_inventory`.`expire_date` IS NOT NULL AND `ez2on_inventory`.`expire_date` <= '{now:yyyy-MM-dd HH:mm:ss}';"
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
                    item.Price = reader.GetInt32(5);
                    item.Level = reader.GetInt32(6);
                    item.ExpPlus = reader.GetInt32(7);
                    item.CoinPlus = reader.GetInt32(8);
                    item.HpPlus = reader.GetInt32(9);
                    item.ResiliencePlus = reader.GetInt32(10);
                    item.DefensePlus = reader.GetInt32(11);
                    item.Type = (ItemType) reader.GetInt32(12);
                    item.Enabled = reader.GetBoolean(13);
                    item.Currency = (ItemCurrencyType) reader.GetInt32(14);
                    InventoryItem inventoryItem = new InventoryItem();
                    inventoryItem.Id = reader.GetInt32(15);
                    inventoryItem.CharacterId = reader.GetInt32(16);
                    inventoryItem.PurchaseDate = reader.GetDateTime(17);
                    inventoryItem.Slot = reader.GetInt32(18);
                    inventoryItem.Equipped = reader.GetInt32(19);
                    inventoryItem.EquipDate = GetNullable(reader, 20, reader.GetDateTime);
                    inventoryItem.ExpireDate = GetNullable(reader, 21, reader.GetDateTime);
                    inventoryItem.Item = item;
                    inventoryItems.Add(inventoryItem);
                }
            });
            return inventoryItems;
        }

        public bool InsertInventoryItem(InventoryItem inventoryItem)
        {
            string sqlCreate = String.Join(Separator,
                "INSERT INTO `ez2on_inventory`",
                "(`character_id`, `item_id`, `purchase_date`, `slot`, `equipped`, `equip_date`, `expire_date`)",
                "VALUES",
                "(",
                $"{inventoryItem.CharacterId},",
                $"{inventoryItem.Item.Id},",
                $"'{inventoryItem.PurchaseDate:yyyy-MM-dd HH:mm:ss}',",
                $"{inventoryItem.Slot},",
                $"{inventoryItem.Equipped},",
                $"{SetNullable(inventoryItem.EquipDate)},",
                $"{SetNullable(inventoryItem.ExpireDate)}",
                ");"
            );
            int affectedRows = ExecuteNonQuery(sqlCreate, out long autoIncrement);
            inventoryItem.Id = (int) autoIncrement;
            return affectedRows > 0;
        }

        public bool DeleteInventoryItem(int inventoryItemId)
        {
            string sqlDelete = String.Join(Separator,
                "DELETE FROM `ez2on_inventory` WHERE",
                $"`id`={inventoryItemId};"
            );
            ExecuteNonQuery(sqlDelete);
            return true;
        }

        public bool DeleteInventoryItems(List<int> inventoryItemIds)
        {
            string ids = String.Join(",", inventoryItemIds);
            string sqlDelete = String.Join(Separator,
                "DELETE FROM `ez2on_inventory` WHERE",
                $"`id` IN ({ids});"
            );
            ExecuteNonQuery(sqlDelete);
            return true;
        }

        public List<InventoryItem> SelectInventoryItems(int characterId)
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();
            string sql = String.Join(Separator,
                "SELECT `ez2on_item`.`id`, `ez2on_item`.`name`, `ez2on_item`.`effect`, `ez2on_item`.`image`,",
                "`ez2on_item`.`duration`, `ez2on_item`.`price`, `ez2on_item`.`level`, `ez2on_item`.`exp_plus`,",
                "`ez2on_item`.`coin_plus`, `ez2on_item`.`hp_plus`, `ez2on_item`.`resilience_plus`,",
                "`ez2on_item`.`defense_plus`, `ez2on_item`.`type`, `ez2on_item`.`enabled`, `ez2on_item`.`currency`,",
                "`ez2on_inventory`.`id`, `ez2on_inventory`.`character_id`, `ez2on_inventory`.`purchase_date`,",
                "`ez2on_inventory`.`slot`, `ez2on_inventory`.`equipped`, `ez2on_inventory`.`equip_date`, `ez2on_inventory`.`expire_date`",
                "FROM `ez2on_inventory`",
                "INNER JOIN `ez2on_item` on `ez2on_item`.`id` = `ez2on_inventory`.`item_id`",
                "WHERE",
                $"`ez2on_inventory`.`character_id`={characterId};"
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
                    item.Price = reader.GetInt32(5);
                    item.Level = reader.GetInt32(6);
                    item.ExpPlus = reader.GetInt32(7);
                    item.CoinPlus = reader.GetInt32(8);
                    item.HpPlus = reader.GetInt32(9);
                    item.ResiliencePlus = reader.GetInt32(10);
                    item.DefensePlus = reader.GetInt32(11);
                    item.Type = (ItemType) reader.GetInt32(12);
                    item.Enabled = reader.GetBoolean(13);
                    item.Currency = (ItemCurrencyType) reader.GetInt32(14);
                    InventoryItem inventoryItem = new InventoryItem();
                    inventoryItem.Id = reader.GetInt32(15);
                    inventoryItem.CharacterId = reader.GetInt32(16);
                    inventoryItem.PurchaseDate = reader.GetDateTime(17);
                    inventoryItem.Slot = reader.GetInt32(18);
                    inventoryItem.Equipped = reader.GetInt32(19);
                    inventoryItem.EquipDate = GetNullable(reader, 20, reader.GetDateTime);
                    inventoryItem.ExpireDate = GetNullable(reader, 21, reader.GetDateTime);
                    inventoryItem.Item = item;
                    inventoryItems.Add(inventoryItem);
                }
            });
            return inventoryItems;
        }

        public bool UpdateInventoryItem(InventoryItem inventoryItem)
        {
            string sqlUpate = String.Join(Separator,
                "UPDATE `ez2on_inventory`",
                "SET",
                $"`character_id`={inventoryItem.CharacterId},",
                $"`item_id`={inventoryItem.Item.Id},",
                $"`purchase_date`='{inventoryItem.PurchaseDate:yyyy-MM-dd HH:mm:ss}',",
                $"`slot`={inventoryItem.Slot},",
                $"`equipped`={inventoryItem.Equipped},",
                $"`equip_date`={SetNullable(inventoryItem.EquipDate)},",
                $"`expire_date`={SetNullable(inventoryItem.ExpireDate)}",
                "WHERE",
                $"`id`={inventoryItem.Id};"
            );
            int rowsAffected = ExecuteNonQuery(sqlUpate);
            return rowsAffected > 0;
        }

        public Quest SelectQuest(int questId)
        {
            string sql = String.Join(Separator,
                "SELECT `title`, `mission`",
                "FROM `ez2on_quest`",
                "WHERE",
                $"`id`={questId};"
            );
            Quest quest = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    quest = new Quest();
                    quest.Id = questId;
                    quest.Title = reader.GetString(0);
                    quest.Mission = reader.GetString(1);
                }
            });
            return quest;
        }

        public bool UpsertQuest(Quest quest)
        {
            string[] columns =
            {
                "id", "title", "mission"
            };
            object[] values =
            {
                quest.Id, quest.Title, quest.Mission
            };
            try
            {
                Upsert("ez2on_quest", columns, values, "id", quest.Id, out long id);
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                return false;
            }

            return true;
        }

        private static Dictionary<int, Song> _songs = new Dictionary<int, Song>();

public Song SelectSong(int songId)
        {
            string sql = String.Join(Separator,
                "SELECT `name`, `category`, `duration`, `bpm`, `file_name`,",
                "`ruby_ez_exr`, `ruby_ez_notes`, `ruby_nm_exr`, `ruby_nm_notes`, `ruby_hd_exr`, `ruby_hd_notes`,`ruby_shd_exr`, `ruby_shd_notes`,",
                "`street_ez_exr`, `street_ez_notes`, `street_nm_exr`, `street_nm_notes`, `street_hd_exr`, `street_hd_notes`, `street_shd_exr`, `street_shd_notes`,",
                "`club_ez_exr`, `club_ez_notes`, `club_nm_exr`, `club_nm_notes`, `club_hd_exr`, `club_hd_notes`, `club_shd_exr`, `club_shd_notes`,",
                "`measure_scale`, `judgment_kool` ,`judgment_cool`, `judgment_good`, `judgment_miss`, `gauge_cool`, `gauge_good`, `gauge_miss`, `gauge_fail`",
                "FROM `ez2on_song`",
                "WHERE",
                $"`id`={songId};"
            );
            Song song = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    song = new Song();
                    song.Id = songId;
                    song.Name = reader.GetString(0);
                    song.Category = (SongCategoryType) reader.GetInt32(1);
                    song.Duration = reader.GetString(2);
                    song.Bpm = reader.GetInt32(3);
                    song.FileName = reader.GetString(4);
                    song.RubyEzExr = reader.GetInt32(5);
                    song.RubyEzNotes = reader.GetInt32(6);
                    song.RubyNmExr = reader.GetInt32(7);
                    song.RubyNmNotes = reader.GetInt32(8);
                    song.RubyHdExr = reader.GetInt32(9);
                    song.RubyHdNotes = reader.GetInt32(10);
                    song.RubyShdExr = reader.GetInt32(11);
                    song.RubyShdNotes = reader.GetInt32(12);
                    song.StreetEzExr = reader.GetInt32(13);
                    song.StreetEzNotes = reader.GetInt32(14);
                    song.StreetNmExr = reader.GetInt32(15);
                    song.StreetNmNotes = reader.GetInt32(16);
                    song.StreetHdExr = reader.GetInt32(17);
                    song.StreetHdNotes = reader.GetInt32(18);
                    song.StreetShdExr = reader.GetInt32(19);
                    song.StreetShdNotes = reader.GetInt32(20);
                    song.ClubEzExr = reader.GetInt32(21);
                    song.ClubEzNotes = reader.GetInt32(22);
                    song.ClubNmExr = reader.GetInt32(23);
                    song.ClubNmNotes = reader.GetInt32(24);
                    song.ClubHdExr = reader.GetInt32(25);
                    song.ClubHdNotes = reader.GetInt32(26);
                    song.ClubShdExr = reader.GetInt32(27);
                    song.ClubShdNotes = reader.GetInt32(28);
                    song.MeasureScale = reader.GetFloat(29);
                    song.JudgmentKool = reader.GetByte(30);
                    song.JudgmentCool = reader.GetByte(31);
                    song.JudgmentGood = reader.GetByte(32);
                    song.JudgmentMiss = reader.GetByte(33);
                    song.GaugeCool = reader.GetFloat(34);
                    song.GaugeGood = reader.GetFloat(35);
                    song.GaugeMiss = reader.GetFloat(36);
                    song.GaugeFail = reader.GetFloat(37);
                }
            });
            return song;
        }

        public List<Song> SelectSongs()
        {
            List<Song> songs = new List<Song>();
            string sql = String.Join(Separator,
                "SELECT `id`, `name`, `category`, `duration`, `bpm`, `file_name`,",
                "`ruby_ez_exr`, `ruby_ez_notes`, `ruby_nm_exr`, `ruby_nm_notes`, `ruby_hd_exr`, `ruby_hd_notes`,`ruby_shd_exr`, `ruby_shd_notes`,",
                "`street_ez_exr`, `street_ez_notes`, `street_nm_exr`, `street_nm_notes`, `street_hd_exr`, `street_hd_notes`, `street_shd_exr`, `street_shd_notes`,",
                "`club_ez_exr`, `club_ez_notes`, `club_nm_exr`, `club_nm_notes`, `club_hd_exr`, `club_hd_notes`, `club_shd_exr`, `club_shd_notes`,",
                "`measure_scale`, `judgment_kool` ,`judgment_cool`, `judgment_good`, `judgment_miss`, `gauge_cool`, `gauge_good`, `gauge_miss`, `gauge_fail`",
                "FROM `ez2on_song`;"
            );
            ExecuteReader(sql, reader =>
            {
                while (reader.Read())
                {
                    Song song = new Song();
                    song.Id = reader.GetInt32(0);
                    song.Name = reader.GetString(1);
                    song.Category = (SongCategoryType) reader.GetInt32(2);
                    song.Duration = reader.GetString(3);
                    song.Bpm = reader.GetInt32(4);
                    song.FileName = reader.GetString(5);
                    song.RubyEzExr = reader.GetInt32(6);
                    song.RubyEzNotes = reader.GetInt32(7);
                    song.RubyNmExr = reader.GetInt32(8);
                    song.RubyNmNotes = reader.GetInt32(9);
                    song.RubyHdExr = reader.GetInt32(10);
                    song.RubyHdNotes = reader.GetInt32(11);
                    song.RubyShdExr = reader.GetInt32(12);
                    song.RubyShdNotes = reader.GetInt32(13);
                    song.StreetEzExr = reader.GetInt32(14);
                    song.StreetEzNotes = reader.GetInt32(15);
                    song.StreetNmExr = reader.GetInt32(16);
                    song.StreetNmNotes = reader.GetInt32(17);
                    song.StreetHdExr = reader.GetInt32(18);
                    song.StreetHdNotes = reader.GetInt32(19);
                    song.StreetShdExr = reader.GetInt32(20);
                    song.StreetShdNotes = reader.GetInt32(21);
                    song.ClubEzExr = reader.GetInt32(22);
                    song.ClubEzNotes = reader.GetInt32(23);
                    song.ClubNmExr = reader.GetInt32(24);
                    song.ClubNmNotes = reader.GetInt32(25);
                    song.ClubHdExr = reader.GetInt32(26);
                    song.ClubHdNotes = reader.GetInt32(27);
                    song.ClubShdExr = reader.GetInt32(28);
                    song.ClubShdNotes = reader.GetInt32(29);
                    song.MeasureScale = reader.GetFloat(30);
                    song.JudgmentKool = reader.GetByte(31);
                    song.JudgmentCool = reader.GetByte(32);
                    song.JudgmentGood = reader.GetByte(33);
                    song.JudgmentMiss = reader.GetByte(34);
                    song.GaugeCool = reader.GetFloat(35);
                    song.GaugeGood = reader.GetFloat(36);
                    song.GaugeMiss = reader.GetFloat(37);
                    song.GaugeFail = reader.GetFloat(38);
                    songs.Add(song);
                }
            });
            return songs;
        }

        public bool UpsertSong(Song song)
        {
            string[] columns =
            {
                "id", "name", "category", "duration", "bpm", "file_name",
                "ruby_ez_exr", "ruby_ez_notes", "ruby_nm_exr", "ruby_nm_notes", "ruby_hd_exr", "ruby_hd_notes",
                "ruby_shd_exr", "ruby_shd_notes",
                "street_ez_exr", "street_ez_notes", "street_nm_exr", "street_nm_notes", "street_hd_exr",
                "street_hd_notes", "street_shd_exr", "street_shd_notes",
                "club_ez_exr", "club_ez_notes", "club_nm_exr", "club_nm_notes", "club_hd_exr", "club_hd_notes",
                "club_shd_exr", "club_shd_notes",
                "measure_scale", "judgment_kool", "judgment_cool", "judgment_good", "judgment_miss", "gauge_cool",
                "gauge_good", "gauge_miss", "gauge_fail"
            };
            object[] values =
            {
                song.Id, Escape(song.Name), (int) song.Category, song.Duration, song.Bpm, song.FileName,
                song.RubyEzExr, song.RubyEzNotes, song.RubyNmExr, song.RubyNmNotes,
                song.RubyHdExr, song.RubyHdNotes, song.RubyShdExr, song.RubyShdNotes,
                song.StreetEzExr, song.StreetEzNotes, song.StreetNmExr, song.StreetNmNotes,
                song.StreetHdExr, song.StreetHdNotes, song.StreetShdExr, song.StreetShdNotes,
                song.ClubEzExr, song.ClubEzNotes, song.ClubNmExr, song.ClubNmNotes,
                song.ClubHdExr, song.ClubHdNotes, song.ClubShdExr, song.ClubShdNotes,
                song.MeasureScale, song.JudgmentKool, song.JudgmentCool, song.JudgmentGood,
                song.JudgmentMiss, song.GaugeCool, song.GaugeGood, song.GaugeMiss,
                song.GaugeFail
            };
            try
            {
                Upsert("ez2on_song", columns, values, "id", song.Id, out long id);
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                return false;
            }

            return true;
        }

        public List<Radiomix> SelectRadiomixes()
        {
            List<Radiomix> radiomixes = new List<Radiomix>();
            string sql = String.Join(Separator,
                "SELECT `id`, `b`, `c`, `d`, `e`,",
                "`song_1_id`, `song_1_ruby_notes`, `song_1_street_notes`, `song_1_club_notes`, `song_1_club8k_notes`,",
                "`song_2_id`, `song_2_ruby_notes`, `song_2_street_notes`, `song_2_club_notes`, `song_2_club8k_notes`,",
                "`song_3_id`, `song_3_ruby_notes`, `song_3_street_notes`, `song_3_club_notes`, `song_3_club8k_notes`,",
                "`song_4_id`, `song_4_ruby_notes`, `song_4_street_notes`, `song_4_club_notes`, `song_4_club8k_notes`",
                "FROM `ez2on_radiomix`;"
            );
            ExecuteReader(sql, reader =>
            {
                while (reader.Read())
                {
                    Radiomix song = new Radiomix();
                    song.Id = reader.GetInt32(0);
                    song.B = reader.GetInt32(1);
                    song.C = reader.GetInt32(2);
                    song.D = reader.GetInt32(3);
                    song.E = reader.GetInt32(4);
                    song.Song1Id = reader.GetInt32(5);
                    song.Song1RubyNotes = reader.GetInt32(6);
                    song.Song1StreetNotes = reader.GetInt32(7);
                    song.Song1ClubNotes = reader.GetInt32(8);
                    song.Song1Club8KNotes = reader.GetInt32(9);
                    song.Song2Id = reader.GetInt32(10);
                    song.Song2RubyNotes = reader.GetInt32(11);
                    song.Song2StreetNotes = reader.GetInt32(12);
                    song.Song2ClubNotes = reader.GetInt32(13);
                    song.Song2Club8KNotes = reader.GetInt32(14);
                    song.Song3Id = reader.GetInt32(15);
                    song.Song3RubyNotes = reader.GetInt32(16);
                    song.Song3StreetNotes = reader.GetInt32(17);
                    song.Song3ClubNotes = reader.GetInt32(18);
                    song.Song3Club8KNotes = reader.GetInt32(19);
                    song.Song4Id = reader.GetInt32(20);
                    song.Song4RubyNotes = reader.GetInt32(21);
                    song.Song4StreetNotes = reader.GetInt32(22);
                    song.Song4ClubNotes = reader.GetInt32(23);
                    song.Song4Club8KNotes = reader.GetInt32(24);
                    radiomixes.Add(song);
                }
            });
            return radiomixes;
        }

        public bool UpsertRadiomix(Radiomix radiomix)
        {
            string[] columns =
            {
                "id", "b", "c", "d", "e",
                "song_1_id", "song_1_ruby_notes", "song_1_street_notes", "song_1_club_notes", "song_1_club8k_notes",
                "song_2_id", "song_2_ruby_notes", "song_2_street_notes", "song_2_club_notes", "song_2_club8k_notes",
                "song_3_id", "song_3_ruby_notes", "song_3_street_notes", "song_3_club_notes", "song_3_club8k_notes",
                "song_4_id", "song_4_ruby_notes", "song_4_street_notes", "song_4_club_notes", "song_4_club8k_notes"
            };
            object[] values =
            {
                radiomix.Id, radiomix.B, radiomix.C, radiomix.D, radiomix.E,
                radiomix.Song1Id, radiomix.Song1RubyNotes, radiomix.Song1StreetNotes, radiomix.Song1ClubNotes,
                radiomix.Song1Club8KNotes,
                radiomix.Song2Id, radiomix.Song2RubyNotes, radiomix.Song2StreetNotes, radiomix.Song2ClubNotes,
                radiomix.Song2Club8KNotes,
                radiomix.Song3Id, radiomix.Song3RubyNotes, radiomix.Song3StreetNotes, radiomix.Song3ClubNotes,
                radiomix.Song3Club8KNotes,
                radiomix.Song4Id, radiomix.Song4RubyNotes, radiomix.Song4StreetNotes, radiomix.Song4ClubNotes,
                radiomix.Song4Club8KNotes,
            };
            try
            {
                Upsert("ez2on_radiomix", columns, values, "id", radiomix.Id, out long id);
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                return false;
            }

            return true;
        }

        private List<int> SelectBestScoreIds(int songId, ModeType mode, DifficultyType difficulty, int scoreCount = -1)
        {
            List<int> ids = new List<int>();
            string sql = String.Join(Separator, "SELECT `id`, MAX(`total_score`)",
                "FROM `ez2on_score`",
                "WHERE",
                $"`song_id`={songId} AND `mode`={(int) mode} AND `difficulty`={(int) difficulty} AND `incident`={0}",
                "GROUP BY `character_id`",
                "ORDER BY `total_score` DESC, `created` ASC"
            );
            if (scoreCount > 0)
            {
                sql += $" LIMIT {scoreCount}";
            }

            sql += ";";
            ExecuteReader(sql, reader =>
            {
                while (reader.Read())
                {
                    ids.Add(reader.GetInt32(0));
                }
            });
            return ids;
        }

        public List<Score> SelectBestScores(int songId, ModeType mode, DifficultyType difficulty, int scoreCount = -1)
        {
            List<Score> scores = new List<Score>();
            List<int> scoreIds = SelectBestScoreIds(songId, mode, difficulty, scoreCount);
            if (scoreIds.Count <= 0)
            {
                return scores;
            }

            string ids = String.Join(",", scoreIds);
            string sql = String.Join(Separator,
                "SELECT `ez2on_score`.`id`, `ez2on_score`.`game_id`, `ez2on_score`.`character_id`,",
                "`ez2on_score`.`song_id`, `ez2on_score`.`difficulty`, `ez2on_score`.`stage_clear`,",
                "`ez2on_score`.`max_combo`, `ez2on_score`.`kool`, `ez2on_score`.`cool`, `ez2on_score`.`good`,",
                "`ez2on_score`.`miss`, `ez2on_score`.`fail`, `ez2on_score`.`raw_score`, `ez2on_score`.`rank`,",
                "`ez2on_score`.`total_notes`, `ez2on_score`.`combo_type`, `ez2on_score`.`total_score`,",
                "`ez2on_score`.`note_effect`, `ez2on_score`.`fade_effect`, `ez2on_score`.`created`,",
                "`ez2on_score`.`mode`, `ez2on_score`.`incident`",
                "FROM `ez2on_score`",
                "WHERE",
                $"`ez2on_score`.`id` IN ({ids})",
                "ORDER BY `ez2on_score`.`total_score` DESC, `ez2on_score`.`created` ASC;"
            );
            ExecuteReader(sql, reader =>
            {
                while (reader.Read())
                {
                    Score score = new Score();
                    score.Id = reader.GetInt32(0);
                    int selectedGameId = reader.GetInt32(1);
                    score.Game = SelectGame(selectedGameId);
                    int selectedCharacterId = reader.GetInt32(2);
                    score.Character = SelectCharacter(selectedCharacterId);
                    int selectedSongId = reader.GetInt32(3);
                    score.Song = score.Game.Song;
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
                    score.Created = reader.GetDateTime(19);
                    score.Mode = (ModeType) reader.GetInt32(20);
                    score.Incident = reader.GetBoolean(21);
                    scores.Add(score);
                }
            });
            return scores;
        }

        public Score SelectBestScore(int characterId, int songId, ModeType mode, DifficultyType difficulty)
        {
            string sql = String.Join(Separator,
                "SELECT `ez2on_score`.`id`, `ez2on_score`.`game_id`, `ez2on_score`.`character_id`,",
                "`ez2on_score`.`song_id`, `ez2on_score`.`difficulty`, `ez2on_score`.`stage_clear`,",
                "`ez2on_score`.`max_combo`, `ez2on_score`.`kool`, `ez2on_score`.`cool`, `ez2on_score`.`good`,",
                "`ez2on_score`.`miss`, `ez2on_score`.`fail`, `ez2on_score`.`raw_score`, `ez2on_score`.`rank`,",
                "`ez2on_score`.`total_notes`, `ez2on_score`.`combo_type`, `ez2on_score`.`total_score`,",
                "`ez2on_score`.`note_effect`, `ez2on_score`.`fade_effect`, `ez2on_score`.`created`,",
                "`ez2on_score`.`mode`, `ez2on_score`.`incident`",
                "FROM `ez2on_score`",
                "WHERE",
                $"`ez2on_score`.`song_id`={songId} AND `ez2on_score`.`character_id`={characterId}",
                $"AND `ez2on_score`.`mode`={(int) mode} AND `ez2on_score`.`difficulty`={(int) difficulty} AND `ez2on_score`.`incident`={0}",
                "ORDER BY `ez2on_score`.`total_score` DESC",
                "LIMIT 1;"
            );
            Score score = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    score = new Score();
                    score.Id = reader.GetInt32(0);
                    int selectedGameId = reader.GetInt32(1);
                    score.Game = SelectGame(selectedGameId);
                    int selectedCharacterId = reader.GetInt32(2);
                    score.Character = SelectCharacter(selectedCharacterId);
                    int selectedSongId = reader.GetInt32(3);
                    score.Song = score.Game.Song;
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
                    score.Created = reader.GetDateTime(19);
                    score.Mode = (ModeType) reader.GetInt32(20);
                    score.Incident = reader.GetBoolean(21);
                }
            });
            return score;
        }

        public Score SelectMaxScore(int characterId, ModeType mode)
        {
            string sql = String.Join(Separator,
                "SELECT `ez2on_score`.`id`, `ez2on_score`.`game_id`, `ez2on_score`.`character_id`,",
                "`ez2on_score`.`song_id`, `ez2on_score`.`difficulty`, `ez2on_score`.`stage_clear`,",
                "`ez2on_score`.`max_combo`, `ez2on_score`.`kool`, `ez2on_score`.`cool`, `ez2on_score`.`good`,",
                "`ez2on_score`.`miss`, `ez2on_score`.`fail`, `ez2on_score`.`raw_score`, `ez2on_score`.`rank`,",
                "`ez2on_score`.`total_notes`, `ez2on_score`.`combo_type`, `ez2on_score`.`total_score`,",
                "`ez2on_score`.`note_effect`, `ez2on_score`.`fade_effect`, `ez2on_score`.`created`,",
                "`ez2on_score`.`mode`, `ez2on_score`.`incident`",
                "FROM `ez2on_score`",
                "WHERE",
                $"`ez2on_score`.`character_id` = {characterId} AND `ez2on_score`.`mode` = {(int) mode}",
                $"AND `ez2on_score`.`incident`={0}",
                "ORDER BY `ez2on_score`.`total_score` DESC",
                "LIMIT 1;"
            );
            Score score = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    score = new Score();
                    score.Id = reader.GetInt32(0);
                    int selectedGameId = reader.GetInt32(1);
                    score.Game = SelectGame(selectedGameId);
                    int selectedCharacterId = reader.GetInt32(2);
                    score.Character = SelectCharacter(selectedCharacterId);
                    int selectedSongId = reader.GetInt32(3);
                    score.Song = score.Game.Song;
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
                    score.Created = reader.GetDateTime(19);
                    score.Mode = (ModeType) reader.GetInt32(20);
                    score.Incident = reader.GetBoolean(21);
                }
            });
            return score;
        }

        public Score SelectScore(int scoreId)
        {
            string sql = String.Join(Separator,
                "SELECT `ez2on_score`.`id`, `ez2on_score`.`game_id`, `ez2on_score`.`character_id`,",
                "`ez2on_score`.`song_id`, `ez2on_score`.`difficulty`, `ez2on_score`.`stage_clear`,",
                "`ez2on_score`.`max_combo`, `ez2on_score`.`kool`, `ez2on_score`.`cool`, `ez2on_score`.`good`,",
                "`ez2on_score`.`miss`, `ez2on_score`.`fail`, `ez2on_score`.`raw_score`, `ez2on_score`.`rank`,",
                "`ez2on_score`.`total_notes`, `ez2on_score`.`combo_type`, `ez2on_score`.`total_score`,",
                "`ez2on_score`.`note_effect`, `ez2on_score`.`fade_effect`, `ez2on_score`.`created`,",
                "`ez2on_score`.`mode`, `ez2on_score`.`incident`",
                "FROM `ez2on_score`",
                "WHERE",
                $"`ez2on_score`.`id` = {scoreId};"
            );
            Score score = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    score = new Score();
                    score.Id = reader.GetInt32(0);
                    int selectedGameId = reader.GetInt32(1);
                    score.Game = SelectGame(selectedGameId);
                    int selectedCharacterId = reader.GetInt32(2);
                    score.Character = SelectCharacter(selectedCharacterId);
                    int selectedSongId = reader.GetInt32(3);
                    score.Song = score.Game.Song;
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
                    score.Created = reader.GetDateTime(19);
                    score.Mode = (ModeType) reader.GetInt32(20);
                    score.Incident = reader.GetBoolean(21);
                }
            });
            return score;
        }

        public bool InsertScore(Score score)
        {
            string sqlCreate = String.Join(Separator,
                "INSERT INTO `ez2on_score`",
                "(`game_id`, `character_id`, `song_id`, `difficulty`, `stage_clear`, `max_combo`, `kool`, `cool`, `good`,",
                "`miss`, `fail`, `raw_score`, `rank`, `total_notes`, `combo_type`, `total_score`, `note_effect`, `fade_effect`,",
                "`created`, `mode`, `incident`)",
                "VALUES",
                "(",
                $"{score.Game.Id},",
                $"{score.Character.Id},",
                $"{score.Song.Id},",
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
                $"'{score.Created:yyyy-MM-dd HH:mm:ss}',",
                $"{(int) score.Mode},",
                $"{(score.Incident ? 1 : 0)}",
                ");"
            );
            int affectedRows = ExecuteNonQuery(sqlCreate, out long autoIncrement);
            score.Id = (int) autoIncrement;
            return affectedRows > 0;
        }

        public Game SelectGame(int gameId)
        {
            string sql = String.Join(Separator,
                "SELECT `ez2on_game`.`id`, `ez2on_game`.`song_id`, `ez2on_game`.`group_type`,",
                "`ez2on_game`.`type`, `ez2on_game`.`name`, `ez2on_game`.`created`, `ez2on_game`.`mode`,",
                "`ez2on_game`.`difficulty`",
                "FROM `ez2on_game`",
                "WHERE",
                $"`ez2on_game`.`id`={gameId};"
            );
            Game game = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    game = new Game();
                    game.Id = reader.GetInt32(0);
                    int songId = reader.GetInt32(1);
                    game.Song = SelectSong(songId);
                    game.GroupType = (GameGroupType) reader.GetInt32(2);
                    game.Type = (GameType) reader.GetInt32(3);
                    game.Name = reader.GetString(4);
                    game.Created = reader.GetDateTime(5);
                    game.Mode = (ModeType) reader.GetInt32(6);
                    game.Difficulty = (DifficultyType) reader.GetInt32(7);
                }
            });
            return game;
        }

        public bool InsertGame(Game game)
        {
            string sqlCreate = String.Join(Separator,
                "INSERT INTO `ez2on_game`",
                "(`song_id`, `group_type`, `type`, `name`, `created`, `mode`, `difficulty`)",
                "VALUES",
                "(",
                $"{game.Song.Id},",
                $"{(int) game.GroupType},",
                $"{(int) game.Type},",
                $"'{game.Name}',",
                $"'{game.Created:yyyy-MM-dd HH:mm:ss}',",
                $"{(int) game.Mode},",
                $"{(int) game.Difficulty}",
                ");"
            );
            int affectedRows = ExecuteNonQuery(sqlCreate, out long autoIncrement);
            game.Id = (int) autoIncrement;
            return affectedRows > 0;
        }

        public Rank SelectRank(int rankId)
        {
            string sql = String.Join(Separator,
                "SELECT `ez2on_rank`.`id`, `ez2on_rank`.`game_id`, `ez2on_rank`.`score_id`,",
                "`ez2on_rank`.`ranking`, `ez2on_rank`.`team`",
                "FROM `ez2on_rank`",
                "WHERE",
                $"`ez2on_rank`.`id`={rankId};"
            );
            Rank rank = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    rank = new Rank();
                    rank.Id = reader.GetInt32(0);
                    int gameId = reader.GetInt32(1);
                    int scoreId = reader.GetInt32(2);
                    rank.Score = SelectScore(scoreId);
                    rank.Game = rank.Score.Game;
                    rank.Ranking = reader.GetByte(3);
                    rank.Team = (TeamType) reader.GetInt32(4);
                }
            });
            return rank;
        }

        public bool InsertRank(Rank rank)
        {
            string sqlCreate = String.Join(Separator,
                "INSERT INTO `ez2on_rank`",
                "(`game_id`, `score_id`, `ranking`, `team`)",
                "VALUES",
                "(",
                $"{rank.Game.Id},",
                $"{rank.Score.Id},",
                $"{rank.Ranking},",
                $"{(int) rank.Team}",
                ");"
            );
            int affectedRows = ExecuteNonQuery(sqlCreate, out long autoIncrement);
            rank.Id = (int) autoIncrement;
            return affectedRows > 0;
        }

        public bool UpsertStatus(DateTime status)
        {
            return SetSetting("ez_status", $"'{status:yyyy-MM-dd HH:mm:ss}'");
        }

        public List<Message> SelectMessages(int characterId)
        {
            List<Message> messages = new List<Message>();
            string sql = String.Join(Separator,
                "SELECT `ez2on_message`.`id`, `ez2on_message`.`sender_id`, `ez2on_message`.`receiver_id`,",
                "`ez2on_message`.`content`, `ez2on_message`.`send_at`, `ez2on_message`.`read`,",
                "`sender`.`name`, `receiver`.`name`",
                "FROM `ez2on_message`",
                "INNER JOIN `ez2on_character` AS `sender` on `sender`.`id` = `ez2on_message`.`sender_id`",
                "INNER JOIN `ez2on_character` AS `receiver` on `receiver`.`id` = `ez2on_message`.`receiver_id`",
                "WHERE",
                $"`ez2on_message`.`receiver_id`={characterId};"
            );
            ExecuteReader(sql, reader =>
            {
                while (reader.Read())
                {
                    Message message = new Message();
                    message.Id = reader.GetInt32(0);
                    message.SenderId = reader.GetInt32(1);
                    message.ReceiverId = reader.GetInt32(2);
                    message.Content = reader.GetString(3);
                    message.SendAt = reader.GetDateTime(4);
                    message.Read = reader.GetBoolean(5);
                    message.Sender = reader.GetString(6);
                    message.Receiver = reader.GetString(7);
                    messages.Add(message);
                }
            });
            return messages;
        }

        public bool InsertMessage(Message message)
        {
            string sql = String.Join(Separator,
                "INSERT INTO `ez2on_message`",
                "(`sender_id`, `receiver_id`,`content`, `send_at`, `read`)",
                "VALUES",
                "(",
                $"{message.SenderId},",
                $"{message.ReceiverId},",
                $"'{message.Content}',",
                $"'{message.SendAt:yyyy-MM-dd HH:mm:ss}',",
                $"{(message.Read ? 1 : 0)}",
                ");"
            );
            int affectedRows = ExecuteNonQuery(sql, out long autoIncrement);
            message.Id = (int) autoIncrement;
            return affectedRows > 0;
        }

        public bool UpdateMessage(Message message)
        {
            string sql = String.Join(Separator,
                "UPDATE `ez2on_message`",
                "SET",
                $"`sender_id`={message.SenderId},",
                $"`receiver_id`={message.ReceiverId},",
                $"`content`='{message.Content}',",
                $"`send_at`='{message.SendAt:yyyy-MM-dd HH:mm:ss}',",
                $"`read`={(message.Read ? 1 : 0)}",
                "WHERE",
                $"`id`={message.Id};"
            );
            int rowsAffected = ExecuteNonQuery(sql);
            return rowsAffected > 0;
        }

        public bool DeleteMessage(int messageId)
        {
            string sql = $"DELETE FROM `ez2on_message` WHERE `id`='{messageId}';";
            return ExecuteNonQuery(sql) > 0;
        }

        public List<Friend> SelectFriends(int characterId)
        {
            List<Friend> friends = new List<Friend>();
            string sql = String.Join(Separator,
                "SELECT `ez2on_friend`.`id`, `ez2on_friend`.`friend_character_id`, `ez2on_character`.`name`",
                "FROM `ez2on_friend`",
                "INNER JOIN `ez2on_character` on `ez2on_friend`.`friend_character_id` = `ez2on_character`.`id`",
                "WHERE",
                $"`ez2on_friend`.`character_id`={characterId};"
            );
            ExecuteReader(sql, reader =>
            {
                while (reader.Read())
                {
                    Friend friend = new Friend();
                    friend.Id = reader.GetInt32(0);
                    friend.FriendCharacterId = reader.GetInt32(1);
                    friend.FriendCharacterName = reader.GetString(2);
                    friend.CharacerId = characterId;
                    friends.Add(friend);
                }
            });
            return friends;
        }

        public bool InsertFriend(Friend friend)
        {
            string sql = String.Join(Separator,
                "INSERT INTO `ez2on_friend`",
                "(`character_id`, `friend_character_id`)",
                "VALUES",
                "(",
                $"{friend.CharacerId},",
                $"{friend.FriendCharacterId}",
                ");"
            );
            int affectedRows = ExecuteNonQuery(sql, out long autoIncrement);
            friend.Id = (int) autoIncrement;
            return affectedRows > 0;
        }

        public bool DeleteFriend(int friendId)
        {
            string sql = $"DELETE FROM `ez2on_friend` WHERE `id`='{friendId}';";
            return ExecuteNonQuery(sql) > 0;
        }

        public GiftItem SelectGiftItem(int giftItemId)
        {
            string sql = String.Join(Separator,
                "SELECT `ez2on_gift`.`item_id`, `ez2on_gift`.`sender_id`, `ez2on_gift`.`receiver_id`,",
                "`ez2on_gift`.`send_at`, `ez2on_gift`.`read`, `ez2on_gift`.`expire_date`,",
                "`sender`.`name`",
                "FROM `ez2on_gift`",
                "INNER JOIN `ez2on_character` AS `sender` on `sender`.`id` = `ez2on_gift`.`sender_id`",
                "WHERE",
                $"`ez2on_gift`.`id`={giftItemId};"
            );
            GiftItem giftItem = null;
            ExecuteReader(sql, reader =>
            {
                if (reader.Read())
                {
                    giftItem = new GiftItem();
                    giftItem.Id = giftItemId;
                    giftItem.ItemId = reader.GetInt32(0);
                    giftItem.SenderId = reader.GetInt32(1);
                    giftItem.ReceiverId = reader.GetInt32(2);
                    giftItem.SendAt = reader.GetDateTime(3);
                    giftItem.Read = reader.GetBoolean(4);
                    giftItem.ExpireDate = GetNullable(reader, 5, reader.GetDateTime);
                    giftItem.SenderName = reader.GetString(6);
                }
            });
            return giftItem;
        }

        public bool InsertGiftItem(GiftItem giftItem)
        {
            string sqlCreate = String.Join(Separator,
                "INSERT INTO `ez2on_gift`",
                "(`item_id`, `sender_id`, `receiver_id`, `send_at`, `read`, `expire_date`)",
                "VALUES",
                "(",
                $"{giftItem.ItemId},",
                $"{giftItem.SenderId},",
                $"{giftItem.ReceiverId},",
                $"'{giftItem.SendAt:yyyy-MM-dd HH:mm:ss}',",
                $"{(giftItem.Read ? 1 : 0)},",
                $"{SetNullable(giftItem.ExpireDate)}",
                ");"
            );
            int affectedRows = ExecuteNonQuery(sqlCreate, out long autoIncrement);
            giftItem.Id = (int) autoIncrement;
            return affectedRows > 0;
        }

        public bool UpdateGiftItem(GiftItem giftItem)
        {
            string sqlUpate = String.Join(Separator,
                "UPDATE `ez2on_gift`",
                "SET",
                $"`item_id`={giftItem.ItemId},",
                $"`sender_id`={giftItem.SenderId},",
                $"`receiver_id`={giftItem.ReceiverId},",
                $"`send_at`='{giftItem.SendAt:yyyy-MM-dd HH:mm:ss}',",
                $"`read`={(giftItem.Read ? 1 : 0)},",
                $"`expire_date`={SetNullable(giftItem.ExpireDate)}",
                "WHERE",
                $"`id`={giftItem.Id};"
            );
            int rowsAffected = ExecuteNonQuery(sqlUpate);
            return rowsAffected > 0;
        }

        public bool DeleteGiftItem(int giftItemId)
        {
            string sqlDelete = String.Join(Separator,
                "DELETE FROM `ez2on_gift` WHERE",
                $"`id`={giftItemId}"
            );
            ExecuteNonQuery(sqlDelete);
            return true;
        }

        public List<GiftItem> SelectGiftItems(int characterId)
        {
            List<GiftItem> giftItems = new List<GiftItem>();
            string sql = String.Join(Separator,
                "SELECT `ez2on_gift`.`id`, `ez2on_gift`.`item_id`, `ez2on_gift`.`sender_id`,",
                "`ez2on_gift`.`receiver_id`, `ez2on_gift`.`send_at`, `ez2on_gift`.`read`, `ez2on_gift`.`expire_date`,",
                "`sender`.`name`",
                "FROM `ez2on_gift`",
                "INNER JOIN `ez2on_character` AS `sender` on `sender`.`id` = `ez2on_gift`.`sender_id`",
                "WHERE",
                $"`ez2on_gift`.`receiver_id`={characterId};"
            );
            ExecuteReader(sql, reader =>
            {
                while (reader.Read())
                {
                    GiftItem giftItem = new GiftItem();
                    giftItem.Id = reader.GetInt32(0);
                    giftItem.ItemId = reader.GetInt32(1);
                    giftItem.SenderId = reader.GetInt32(2);
                    giftItem.ReceiverId = reader.GetInt32(3);
                    giftItem.SendAt = reader.GetDateTime(4);
                    giftItem.Read = reader.GetBoolean(5);
                    giftItem.ExpireDate = GetNullable(reader, 6, reader.GetDateTime);
                    giftItem.SenderName = reader.GetString(7);
                    giftItems.Add(giftItem);
                }
            });
            return giftItems;
        }

        public List<GiftItem> SelectExpiredGifts()
        {
            List<GiftItem> giftItems = new List<GiftItem>();
            DateTime now = DateTime.Now;
            string sql = String.Join(Separator,
                "SELECT `ez2on_gift`.`id`, `ez2on_gift`.`item_id`, `ez2on_gift`.`sender_id`,",
                "`ez2on_gift`.`receiver_id`, `ez2on_gift`.`send_at`, `ez2on_gift`.`read`, `ez2on_gift`.`expire_date`,",
                "`sender`.`name`",
                "FROM `ez2on_gift`",
                "INNER JOIN `ez2on_character` AS `sender` on `sender`.`id` = `ez2on_gift`.`sender_id`",
                "WHERE",
                $"`ez2on_gift`.`expire_date` IS NOT NULL AND `ez2on_gift`.`expire_date` <= '{now:yyyy-MM-dd HH:mm:ss}';"
            );
            ExecuteReader(sql, reader =>
            {
                while (reader.Read())
                {
                    GiftItem giftItem = new GiftItem();
                    giftItem.Id = reader.GetInt32(0);
                    giftItem.ItemId = reader.GetInt32(1);
                    giftItem.SenderId = reader.GetInt32(2);
                    giftItem.ReceiverId = reader.GetInt32(3);
                    giftItem.SendAt = reader.GetDateTime(4);
                    giftItem.Read = reader.GetBoolean(5);
                    giftItem.ExpireDate = GetNullable(reader, 6, reader.GetDateTime);
                    giftItem.SenderName = reader.GetString(7);
                    giftItems.Add(giftItem);
                }
            });
            return giftItems;
        }

        public bool DeleteGifts(List<int> giftIds)
        {
            string ids = String.Join(",", giftIds);
            string sqlDelete = String.Join(Separator,
                "DELETE FROM `ez2on_gift` WHERE",
                $"`id` IN ({ids});"
            );
            ExecuteNonQuery(sqlDelete);
            return true;
        }

        public bool InsertIncident(Incident incident)
        {
            string sqlCreate = String.Join(Separator,
                "INSERT INTO `incident`",
                "(`account_id`, `type`, `description`, `created`)",
                "VALUES",
                "(",
                $"{incident.AccountId},",
                $"{incident.Type},",
                $"'{incident.Description}',",
                $"'{incident.Created:yyyy-MM-dd HH:mm:ss}'",
                ");"
            );
            int affectedRows = ExecuteNonQuery(sqlCreate, out long autoIncrement);
            incident.Id = (int) autoIncrement;
            return affectedRows > 0;
        }

        public bool InsertIdentification(Identification identification)
        {
            string sqlCreate = String.Join(Separator,
                "INSERT INTO `identification`",
                "(`account_id`, `ip`, `hardware_id`)",
                "VALUES",
                "(",
                $"{identification.AccountId},",
                $"'{identification.Ip}',",
                $"'{identification.HardwareId}'",
                ");"
            );
            try
            {
                int affectedRows = ExecuteNonQuery(sqlCreate, out long autoIncrement);
                identification.Id = (int) autoIncrement;
                return affectedRows > 0;
            }
            catch (SQLiteException ex)
            {
                if (ex.ResultCode == SQLiteErrorCode.Constraint)
                {
                    return true;
                }
            }

            return false;
        }

        public bool InsertScoreIncident(Incident incident, int scoreId)
        {
            if (!InsertIncident(incident))
            {
                return false;
            }

            string sqlCreate = String.Join(Separator,
                "INSERT INTO `ez2on_score_incident`",
                "(`score_id`, `incident_id`)",
                "VALUES",
                "(",
                $"{scoreId},",
                $"{incident.Id}",
                ");"
            );
            int affectedRows = ExecuteNonQuery(sqlCreate, out long autoIncrement);
            return affectedRows > 0;
        }

        public bool InsertScoreIncident(int incidentId, int scoreId)
        {
            string sqlCreate = String.Join(Separator,
                "INSERT INTO `ez2on_score_incident`",
                "(`score_id`, `incident_id`)",
                "VALUES",
                "(",
                $"{scoreId},",
                $"{incidentId}",
                ");"
            );
            try
            {
                int affectedRows = ExecuteNonQuery(sqlCreate, out long autoIncrement);
                return affectedRows > 0;
            }
            catch (SQLiteException ex)
            {
                if (ex.ResultCode == SQLiteErrorCode.Constraint)
                {
                    return true;
                }
            }

            return false;
        }
    }
}