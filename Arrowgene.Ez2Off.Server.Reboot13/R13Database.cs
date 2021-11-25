using System;
using System.IO;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Data.BinFile;
using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Ez2Off.Server.Database.Sql;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Logging;
using MySql.Data.MySqlClient;

namespace Arrowgene.Ez2Off.Server.Reboot13
{
    /// <summary>
    /// Class to prepare database.
    /// </summary>
    public class R13Database
    {
        private static readonly ILogger _logger = LogProvider.Logger(typeof(R13Database));

        public R13Database()
        {
        }

        public void Prepare(DatabaseSettings settings)
        {
            IDatabase database = null;
            switch (settings.Type)
            {
                case DatabaseType.SQLite:
                    database = PrepareSqlLiteDb(settings.SqLitePath);
                    break;
                case DatabaseType.Maria:
                    database = PrepareMaria(
                        settings.MariaHost,
                        settings.MariaPort,
                        settings.MariaUser,
                        settings.MariaPassword,
                        settings.MariaDatabase
                    );
                    break;
            }

            if (database == null)
            {
                _logger.Error("Database could not be created, exiting...");
                Environment.Exit(1);
            }

            InsertDefaultData(database);
        }


        private MariaDb PrepareMaria(string host, short port, string user, string password, string dbName)
        {
            MariaDb database = null;
            try
            {
                using (MySqlConnection connection =
                    new MySqlConnection($"host={host};port={port};user id={user};password={password}"))
                {
                    connection.Open();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText =
                            $"CREATE DATABASE IF NOT EXISTS {dbName} default character set utf8 default collate utf8_bin;";
                        command.ExecuteNonQuery();
                    }
                }

                database = new MariaDb(host, port, user, password, dbName);
                ScriptRunner scriptRunner = new ScriptRunner(database);
                scriptRunner.Run(Path.Combine(Utils.RelativeCommonDirectory(), "Database/ez2on_structure.sql"));
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
            }

            return database;
        }

        private SqLiteDb PrepareSqlLiteDb(string sqlLitePath)
        {
            SqLiteDb db = new SqLiteDb(sqlLitePath);

            ScriptRunner scriptRunner = new ScriptRunner(db);
            scriptRunner.Run(Path.Combine(Utils.RelativeCommonDirectory(), "Database/sqlite_ez2on_structure.sql"));

            return db;
        }

        private void InsertDefaultData(IDatabase database)
        {
            Ez2OnBinFileIoR13 binIo = new Ez2OnBinFileIoR13();

            string readItemData = database.GetSetting("ez2on_loaded_item");
            if (readItemData != "true")
            {
                Ez2OnItemBinFileR13 itemDataBin =
                    (Ez2OnItemBinFileR13) binIo.Read(Path.Combine(Utils.RelativeExecutingDirectory(),
                        "Data/R13/ItemData.bin"));
                _logger.Info($"Loading: {itemDataBin.Entries.Count} items from ItemData.bin");
                foreach (Ez2OnModelItem item in itemDataBin.Entries)
                {
                    database.UpsertItem(item.ToItem());
                }

                database.SetSetting("ez2on_loaded_item", "true");
            }

            string readQuestData = database.GetSetting("ez2on_loaded_quest");
            if (readQuestData != "true")
            {
                Ez2OnQuestBinFile questDataBin =
                    (Ez2OnQuestBinFile) binIo.Read(Path.Combine(Utils.RelativeExecutingDirectory(),
                        "Data/R13/Quest_data.bin"));
                _logger.Info($"Loading: {questDataBin.Entries.Count} quests from Quest_data.bin");
                foreach (Ez2OnModelQuest quest in questDataBin.Entries)
                {
                    database.UpsertQuest(quest.ToQuest());
                }

                database.SetSetting("ez2on_loaded_quest", "true");
            }

            string readMusicData = database.GetSetting("ez2on_loaded_song");
            if (readMusicData != "true")
            {
                ScriptRunner scriptRunner = new ScriptRunner(database);
                scriptRunner.Run(Path.Combine(Utils.RelativeCommonDirectory(), "Database/ez2on_song_r13.sql"));
                database.SetSetting("ez2on_loaded_song", "true");
                _logger.Info("Loaded songs from database");
            }
        }
    }
}