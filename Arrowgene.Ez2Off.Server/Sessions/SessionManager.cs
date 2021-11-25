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

using System.Collections.Generic;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;

namespace Arrowgene.Ez2Off.Server.Sessions
{
    public class SessionManager
    {
        private readonly Dictionary<string, Session> _sessions;
        private object _lock;

        public SessionManager()
        {
            _sessions = new Dictionary<string, Session>();
            _lock = new object();
        }

        public string NewSessionKey()
        {
            string sessionKey = Utils.GenerateSessionKey(16);
            return sessionKey;
        }

        public Session GetSession(string sessionKey)
        {
            lock (_lock)
            {
                if (_sessions.ContainsKey(sessionKey))
                {
                    return _sessions[sessionKey];
                }
            }

            return null;
        }

        public List<Session> GetSessions()
        {
            List<Session> sessions;
            lock (_lock)
            {
                sessions = new List<Session>(_sessions.Values);
            }

            return sessions;
        }

        public Session FetchSession(string sessionKey)
        {
            lock (_lock)
            {
                if (_sessions.ContainsKey(sessionKey))
                {
                    Session session = _sessions[sessionKey];
                    _sessions.Remove(sessionKey);
                    return session;
                }
            }

            return null;
        }

        public Session GetSession(int accountId)
        {
            lock (_lock)
            {
                foreach (Session session in _sessions.Values)
                {
                    if (session.Account.Id == accountId)
                    {
                        return session;
                    }
                }
            }

            return null;
        }

        public void StoreSession(Session session)
        {
            lock (_lock)
            {
                if (_sessions.ContainsKey(session.Key))
                {
                    _sessions[session.Key] = session;
                }
                else
                {
                    _sessions.Add(session.Key, session);
                }
            }
        }

        public Session DeleteSession(string sessionKey)
        {
            Session session = null;
            lock (_lock)
            {
                if (_sessions.ContainsKey(sessionKey))
                {
                    session = _sessions[sessionKey];
                    _sessions.Remove(sessionKey);
                }
            }

            return session;
        }
    }
}