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
using Arrowgene.Ez2Off.Server.Api;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Services.Networking.ServerBridge;
using Arrowgene.Services.Networking.ServerBridge.Tcp;

namespace Arrowgene.Ez2Off.Server
{
    public abstract class EzLoginServer : EzServer
    {
        private readonly TcpBridgeSettings _bridgeSettings;

        protected EzLoginServer(SettingsContainer settingsContainer) : base(settingsContainer.LoginSettings)
        {
            Settings = settingsContainer.LoginSettings;
            _bridgeSettings = settingsContainer.GetBridgeSettings(settingsContainer.LoginSettings);
            WorldServerPoints = settingsContainer.GetWorldServerPoints();
            ApiServer = new ApiServer(Settings.ApiSettings, SessionManager, Database);
            Bridge = new TcpBridge(_bridgeSettings);
        }


        protected ApiServer ApiServer { get; }

        public List<ServerPoint> WorldServerPoints { get; }
        public override IBridge Bridge { get; }
        public new LoginServerSettings Settings { get; }

        protected override void _Start()
        {
            base._Start();
            _logger.Info("Need Registration: {0}", Settings.ApiSettings.NeedRegistration);
            ApiServer.Start();
        }

        protected override void _Stop()
        {
            ApiServer.Stop();
            base._Stop();
        }

        public ServerPoint GetServerPoint(int id)
        {
            foreach (ServerPoint serverPoint in WorldServerPoints)
            {
                if (serverPoint.Id == id)
                {
                    return serverPoint;
                }
            }

            return null;
        }
    }
}