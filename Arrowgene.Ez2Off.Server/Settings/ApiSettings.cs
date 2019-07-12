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
using System.Runtime.Serialization;
using Arrowgene.Ez2Off.Common;

namespace Arrowgene.Ez2Off.Server.Settings
{
    [DataContract]
    public class ApiSettings
    {
        /// <summary>
        /// Warning:
        /// Changing while having existing accounts requires to rehash all passwords.
        /// </summary>
        public const int BCryptWorkFactor = 14;

        public ApiSettings()
        {
            NeedRegistration = false;
            ApiPrefix = "localhost";
            ApiPort = 8080;
            ApiWebRoot = Path.Combine(Utils.RelativeApplicationDirectory(), "Web");
            ApiMaximumConnectionCount = 30;
        }

        public ApiSettings(ApiSettings apiSettings)
        {
            NeedRegistration = apiSettings.NeedRegistration;
            ApiPrefix = apiSettings.ApiPrefix;
            ApiPort = apiSettings.ApiPort;
            ApiWebRoot = apiSettings.ApiWebRoot;
            ApiMaximumConnectionCount = apiSettings.ApiMaximumConnectionCount;
        }

        [DataMember(Order = 0)]
        public bool NeedRegistration { get; set; }

        [DataMember(Order = 1)]
        public string ApiPrefix { get; set; }

        [DataMember(Order = 2)]
        public ushort ApiPort { get; set; }

        [DataMember(Order = 3)]
        public string ApiWebRoot { get; set; }

        [DataMember(Order = 4)]
        public int ApiMaximumConnectionCount { get; set; }
    }
}