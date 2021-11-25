using System;
using System.Collections.Generic;
using System.IO;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Data.BinFile;
using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Ez2Off.Server.Database.Sql;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Logging;
using MySql.Data.MySqlClient;

namespace Arrowgene.Ez2Off.Server.Reboot14
{
    /// <summary>
    /// Class to prepare database.
    /// </summary>
    public class R14Database
    {
        private static readonly ILogger _logger = LogProvider.Logger(typeof(R14Database));

        public R14Database()
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
            UpdateSongData(database);
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
            Ez2OnBinFileIo binIo = new Ez2OnBinFileIo();

            string readItemData = database.GetSetting("ez2on_loaded_item");
            if (readItemData != "true")
            {
                Ez2OnItemBinFile itemDataBin =
                    (Ez2OnItemBinFile) binIo.Read(Path.Combine(Utils.RelativeExecutingDirectory(),
                        "Data/R14/ItemData.bin"));
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
                        "Data/R14/Quest_data.bin"));
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
                Ez2OnMusicBinFile musicDataBin =
                    (Ez2OnMusicBinFile) binIo.Read(Path.Combine(Utils.RelativeExecutingDirectory(),
                        "Data/R14/music_data.bin"));
                _logger.Info($"Loading: {musicDataBin.Entries.Count} songs from music_data.bin");
                foreach (Ez2OnModelMusic song in musicDataBin.Entries)
                {
                    database.UpsertSong(song.ToSong());
                }

                //ScriptRunner scriptRunner = new ScriptRunner(database);
                //scriptRunner.Run(Path.Combine(Utils.RelativeCommonDirectory(), "Database/ez2on_song.sql"));
                database.SetSetting("ez2on_loaded_song", "true");
                _logger.Info("Loaded songs from ez2on_song.sql");
            }

            string readRadiomixData = database.GetSetting("ez2on_loaded_radiomix");
            if (readRadiomixData != "true")
            {
                Ez2OnRadiomixBinFile radiomixDataBin =
                    (Ez2OnRadiomixBinFile) binIo.Read(Path.Combine(Utils.RelativeExecutingDirectory(),
                        "Data/R14/radio_mix_data.bin"));
                _logger.Info($"Loading: {radiomixDataBin.Entries.Count} radiomixes from radio_mix_data.bin");
                foreach (Ez2OnModelRadiomix radiomixModel in radiomixDataBin.Entries)
                {
                    database.UpsertRadiomix(radiomixModel.ToRadiomix());
                }

                database.SetSetting("ez2on_loaded_radiomix", "true");
            }

            // ApplyCsv(database, "C:/Users/railgun/Downloads/song.csv");
        }

        public void ApplyCsv(IDatabase database, string path)
        {
            using (var reader = new StreamReader(path))
            {
                int cur = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (cur == 0)
                    {
                        cur++;
                        continue;
                    }

                    string[] values = line.Split(',');
                    int id = int.Parse(values[0]);
                    string name = values[1];
                    string filename = values[2];
                    int bpm = int.Parse(values[3]);
                    float measureScale = float.Parse(values[4]);
                    byte judgementDeltaKool = byte.Parse(values[5]);
                    byte judgementDeltaCool = byte.Parse(values[6]);
                    byte judgementDeltaGood = byte.Parse(values[7]);
                    byte judgementDeltaMiss = byte.Parse(values[8]);
                    float gaugeCool = float.Parse(values[9]);
                    float gaugeGood = float.Parse(values[10]);
                    float gaugeMiss = float.Parse(values[11]);
                    float gaugeFail = float.Parse(values[12]);

                    Song song = database.SelectSong(id);
                    song.MeasureScale = measureScale;
                    song.JudgmentKool = judgementDeltaKool;
                    song.JudgmentCool = judgementDeltaCool;
                    song.JudgmentGood = judgementDeltaGood;
                    song.JudgmentMiss = judgementDeltaMiss;
                    song.GaugeCool = gaugeCool;
                    song.GaugeGood = gaugeGood;
                    song.GaugeMiss = gaugeMiss;
                    song.GaugeFail = gaugeFail;

                    database.UpsertSong(song);
                    cur++;
                }
            }
        }

        public void UpdateSongData(IDatabase database)
        {
            int scrollSpeed = 1440; //1296 //1350 //1440

            List<Song> songs = database.SelectSongs();
            foreach (Song song in songs)
            {
                if (song.Id == 0)
                {
                    // Ignore
                    continue;
                }

                float measureScale = scrollSpeed / (song.Bpm * 4.5f);
                byte judgementKool;
                byte judgementCool;
                byte judgementGood;
                byte judgementMiss;
                float gaugeCool;
                float gaugeGood;
                float gaugeMiss;
                float gaugeFail;
                if (song.Bpm <= 300)
                {
                    judgementKool = 10;
                    judgementCool = 33;
                    judgementGood = 55;
                    judgementMiss = 77;
                    gaugeCool = 0.3f;
                    gaugeGood = 0.2f;
                    gaugeMiss = -4.5f;
                    gaugeFail = -6.0f;
                }
                //else if (song.Bpm <= 0)
                //{
                //    judgementKool = 0;
                //    judgementCool = 0;
                //    judgementGood = 0;
                //    judgementMiss = 0;
                //    gaugeCool = 0f;
                //    gaugeGood = 0f;
                //    gaugeMiss = -0f;
                //    gaugeFail = -0f;
                //}
                else
                {
                    throw new Exception("UNSUPPORTED BPM");
                }

                if (song.FileName == "sudden")
                {
                    judgementKool = 3;
                    judgementCool = 4;
                    judgementGood = 5;
                    judgementMiss = 31;
                    gaugeCool = 1.5f;
                    gaugeGood = 1.0f;
                    gaugeMiss = -18f;
                    gaugeFail = -23f;
                }

                song.MeasureScale = measureScale;
                song.JudgmentKool = judgementKool;
                song.JudgmentCool = judgementCool;
                song.JudgmentGood = judgementGood;
                song.JudgmentMiss = judgementMiss;
                song.GaugeCool = gaugeCool;
                song.GaugeGood = gaugeGood;
                song.GaugeMiss = gaugeMiss;
                song.GaugeFail = gaugeFail;

                database.UpsertSong(song);
            }
        }
    }
}