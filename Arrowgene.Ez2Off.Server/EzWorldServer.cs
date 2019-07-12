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

using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Models;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Services.Networking.ServerBridge;
using Arrowgene.Services.Networking.ServerBridge.Tcp;

namespace Arrowgene.Ez2Off.Server
{
    public abstract class EzWorldServer : EzServer
    {
        private readonly Channel[] _rubyChannels;
        private readonly Channel[] _streeChannels;
        private readonly Channel[] _clubChannels;
        private readonly TcpBridgeSettings _bridgeSettings;

        protected EzWorldServer(SettingsContainer settingsContainer, WorldServerSettings settings) : base(settings)
        {
            Settings = settings;

            _bridgeSettings = settingsContainer.GetBridgeSettings(Settings);
            LoginServerPoint = settingsContainer.GetLoginServerPoint();

            _rubyChannels = new Channel[MaxChannels];
            _streeChannels = new Channel[MaxChannels];
            _clubChannels = new Channel[MaxChannels];

            for (int i = 0; i < MaxChannels; i++)
            {
                _rubyChannels[i] = new Channel(i);
                _streeChannels[i] = new Channel(i);
                _clubChannels[i] = new Channel(i);
            }

            Bridge = new TcpBridge(_bridgeSettings);
        }

        public new WorldServerSettings Settings { get; }
        public override IBridge Bridge { get; }
        public ServerPoint LoginServerPoint { get; }

        /// <summary>
        /// Gets a channel.
        /// </summary>
        public Channel GetChannel(ModeType mode, int index)
        {
            if (index > MaxChannels)
            {
                return null;
            }

            switch (mode)
            {
                case ModeType.RubyMix: return _rubyChannels[index];
                case ModeType.StreetMix: return _streeChannels[index];
                case ModeType.ClubMix: return _clubChannels[index];
            }

            return null;
        }

        public Channel[] GetChannels(ModeType mode)
        {
            switch (mode)
            {
                case ModeType.RubyMix: return _rubyChannels;
                case ModeType.StreetMix: return _streeChannels;
                case ModeType.ClubMix: return _clubChannels;
            }

            return null;
        }
    }
}