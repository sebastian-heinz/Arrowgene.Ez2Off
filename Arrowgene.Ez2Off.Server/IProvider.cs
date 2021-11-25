﻿/*
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

using System.Collections.Generic;
using Arrowgene.Ez2Off.Server.Chat.Command.Commands;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Ez2Off.Server.Trait;

namespace Arrowgene.Ez2Off.Server
{
    public interface IProvider
    {
        IPacketFactoryProvider ProvidePacketFactoryProvider(EzSettings settings);
        ChatTrait ProvideChatTrait(EzServer server);
        RoomTrait ProvideRoomTrait(EzServer server);
        ChannelTrait ProvideChannelTrait(EzServer server);
        ServerTrait ProvideServerTrait(EzServer server);
        IEnumerable<IHandler> ProvideLoginHandler(EzServer server);
        IEnumerable<IHandler> ProvideWorldHandler(EzServer server);
        List<BaseChatCommand> ProvideChatCommands(EzServer server);
    }
}