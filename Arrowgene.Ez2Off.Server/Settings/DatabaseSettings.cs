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
using System.Runtime.Serialization;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Server.Database;

namespace Arrowgene.Ez2Off.Server.Settings
{
    [DataContract]
    public class DatabaseSettings
    {
        public DatabaseSettings()
        {
            Type = DatabaseType.SQLite;
            SqLitePath = Path.Combine(Utils.RelativeExecutingDirectory(), "db.sqlite");
            MariaHost = "localhost";
            MariaPort = 3306;
            MariaDatabase = "arrowgene";
            MariaUser = string.Empty;
            MariaPassword = string.Empty;

            string envDbType = System.Environment.GetEnvironmentVariable("DB_TYPE");
            switch (envDbType)
            {
                case "maria":
                    Type = DatabaseType.Maria;
                    break;
                case "sqlite":
                    Type = DatabaseType.SQLite;
                    break;
            }

            string envDbUser = System.Environment.GetEnvironmentVariable("DB_USER");
            if (!string.IsNullOrEmpty(envDbUser))
            {
                MariaUser = envDbUser;
            }

            string envDbPass = System.Environment.GetEnvironmentVariable("DB_PASS");
            if (!string.IsNullOrEmpty(envDbPass))
            {
                MariaPassword = envDbPass;
            }
        }

        public DatabaseSettings(DatabaseSettings databaseSettings)
        {
            Type = databaseSettings.Type;
            SqLitePath = databaseSettings.SqLitePath;
            MariaHost = databaseSettings.MariaHost;
            MariaPort = databaseSettings.MariaPort;
            MariaUser = databaseSettings.MariaUser;
            MariaPassword = databaseSettings.MariaPassword;
            MariaDatabase = databaseSettings.MariaDatabase;
        }

        [DataMember(Order = 0)]
        public DatabaseType Type { get; set; }

        [DataMember(Order = 1)]
        public string SqLitePath { get; set; }

        [DataMember(Order = 2)]
        public string MariaHost { get; set; }

        [DataMember(Order = 3)]
        public short MariaPort { get; set; }

        [DataMember(Order = 4)]
        public string MariaUser { get; set; }

        [DataMember(Order = 5)]
        public string MariaPassword { get; set; }

        [DataMember(Order = 6)]
        public string MariaDatabase { get; set; }
    }
}