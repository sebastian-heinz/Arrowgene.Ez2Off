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

using System.IO;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Json;

namespace Arrowgene.Ez2Off.Server.Settings
{
    public class SettingsProvider
    {
        private readonly string _directory;


        public SettingsProvider(string directory = null)
        {
            if (Directory.Exists(directory))
            {
                _directory = directory;
            }
            else
            {
                _directory = Utils.RelativeExecutingDirectory();
            }
        }

        public string GetSettingsPath(string file)
        {
            return Path.Combine(_directory, file);
        }

        public void Save<T>(T settings, string file)
        {
            string path = GetSettingsPath(file);
            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(path, json);
        }

        public T Load<T>(string file)
        {
            T settings;
            string path = GetSettingsPath(file);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                settings = JsonSerializer.Deserialize<T>(json);
            }
            else
            {
                settings = default(T);
            }

            return settings;
        }
    }
}