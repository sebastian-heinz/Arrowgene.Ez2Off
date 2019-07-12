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

using System.Collections.Generic;
using System.Runtime.Serialization;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Services.Networking;
using Arrowgene.Services.Networking.ServerBridge.Tcp;

namespace Arrowgene.Ez2Off.Server.Settings
{
    [DataContract]
    public class SettingsContainer
    {
        public const int MaxWorldServer = 4;

        [DataMember(Order = 0)]
        public LoginServerSettings LoginSettings { get; set; }

        [DataMember(Order = 1)]
        public List<WorldServerSettings> WorldSettingsList { get; set; }

        public SettingsContainer()
        {
            WorldSettingsList = new List<WorldServerSettings>();
        }

        public void AddWorldServer(WorldServerSettings worldServerSettings)
        {
            WorldSettingsList.Add(worldServerSettings);
        }

        public List<ServerPoint> GetWorldServerPoints()
        {
            List<ServerPoint> worldServerPoints = new List<ServerPoint>();
            int id = 0;
            foreach (WorldServerSettings worldSettings in WorldSettingsList)
            {
                if (id >= MaxWorldServer)
                {
                    break;
                }

                worldServerPoints.Add(new ServerPoint()
                {
                    Id = id,
                    Public = new NetworkPoint(worldSettings.PublicEndPoint),
                    Bridge = new NetworkPoint(worldSettings.BridgePublicEndPoint),
                });
                id++;
            }

            return worldServerPoints;
        }

        public ServerPoint GetLoginServerPoint()
        {
            return new ServerPoint()
            {
                Public = new NetworkPoint(LoginSettings.PublicEndPoint),
                Bridge = new NetworkPoint(LoginSettings.BridgePublicEndPoint),
            };
        }

        public TcpBridgeSettings GetBridgeSettings(LoginServerSettings loginSettings)
        {
            List<NetworkPoint> clients = new List<NetworkPoint>();
            foreach (WorldServerSettings worldSettings in WorldSettingsList)
            {
                clients.Add(new NetworkPoint(worldSettings.BridgePublicEndPoint));
            }

            return GetBridgeSettings(loginSettings, clients);
        }

        public TcpBridgeSettings GetBridgeSettings(WorldServerSettings worldSettings)
        {
            List<NetworkPoint> clients = new List<NetworkPoint>()
            {
                new NetworkPoint(LoginSettings.BridgePublicEndPoint)
            };
            return GetBridgeSettings(worldSettings, clients);
        }

        private TcpBridgeSettings GetBridgeSettings(EzServerSettings settings, List<NetworkPoint> clients)
        {
            NetworkPoint listenPoint = new NetworkPoint(settings.BridgeListenEndPoint);
            NetworkPoint publicPoint = new NetworkPoint(settings.BridgePublicEndPoint);
            TcpBridgeSettings bridgeSettings = new TcpBridgeSettings(listenPoint, publicPoint, clients);
            bridgeSettings.ServerSettings = settings.BridgeServerSettings;
            return bridgeSettings;
        }
    }
}