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

using System.IO;
using System.Reflection;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Data.BinFiles;
using Arrowgene.Ez2Off.Server.Bridge;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.Login;
using Arrowgene.Ez2Off.Server.Scripting;
using Arrowgene.Ez2Off.Server.Settings;

namespace Arrowgene.Ez2Off.Server.Reboot13
{
    public class LoginServer : EzLoginServer
    {
        public LoginServer(SettingsContainer settingsContainer) : base(settingsContainer)
        {
            EzScriptEngine.Instance.AddReference(Assembly.GetAssembly(typeof(LoginServer)));
        }

        public override string Name => "Reboot13 LoginServer";

        protected override void _Start()
        {
            UpdateDatabase(1);
            base._Start();
        }

        protected override void LoadHandles()
        {
            AddHandler(new LoginRequest(this));
            AddHandler(new ExitGame(this));
            AddHandler(new SelectMode(this));
            AddHandler(new SelectServer(this));
            AddHandler(new SelectChannel(this));
            AddHandler(new CreateCharacter(this));
            Bridge.AddHandler(new SessionHandler(SessionManager));
        }

        private void UpdateDatabase(int version)
        {
            DatabaseMeta meta = Database.SelectMeta(version);
            if (meta == null)
            {
                meta = new DatabaseMeta(version);
                Database.UpsertMeta(meta);
            }

            if (!meta.ReadItemData)
            {
                ItemDataBin itemDataBin = new ItemDataBin();
                itemDataBin.Read(Path.Combine(Utils.RelativeApplicationDirectory(), "Data/ItemData.bin"));
                _logger.Info("Loading: {0} items from ItemData.bin", itemDataBin.Entries.Count);
                foreach (Item item in itemDataBin.Entries)
                {
                    Database.UpsertItem(item);
                }

                meta.ReadItemData = true;
                Database.UpsertMeta(meta);
            }


            if (!meta.ReadQuestData)
            {
                QuestDataBin questDataBin = new QuestDataBin();
                questDataBin.Read(Path.Combine(Utils.RelativeApplicationDirectory(), "Data/Quest_data.bin"));
                _logger.Info("Loading: {0} quests from Quest_data.bin", questDataBin.Entries.Count);
                foreach (Quest quest in questDataBin.Entries)
                {
                    Database.UpsertQuest(quest);
                }

                meta.ReadQuestData = true;
                Database.UpsertMeta(meta);
            }

            if (!meta.ReadSongData)
            {
                MusicDataBin musicDataBin = new MusicDataBin();
                musicDataBin.Read(Path.Combine(Utils.RelativeApplicationDirectory(), "Data/music_data.bin"));
                _logger.Info("Loading: {0} songs from music_data.bin", musicDataBin.Entries.Count);
                foreach (Song song in musicDataBin.Entries)
                {
                    Database.UpsertSong(song);
                }

                meta.ReadSongData = true;
                Database.UpsertMeta(meta);
            }
        }
    }
}