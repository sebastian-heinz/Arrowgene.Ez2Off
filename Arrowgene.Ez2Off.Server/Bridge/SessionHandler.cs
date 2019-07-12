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
using Arrowgene.Ez2Off.Server.Sessions;
using Arrowgene.Services.Networking.ServerBridge;
using Arrowgene.Services.Networking.ServerBridge.Messages;

namespace Arrowgene.Ez2Off.Server.Bridge
{
    public class SessionHandler : IMessageHandler<string, Session>
    {
        public const string Id = "SessionHandler";

        private ISessionManager _sessionManager;

        public SessionHandler(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public Response<Session> Handle(Request<string> request)
        {
            Session session = _sessionManager.GetSession(request.Content);
            return new Response<Session>(request, session);
        }

        public string HandlerId => Id;
    }
}