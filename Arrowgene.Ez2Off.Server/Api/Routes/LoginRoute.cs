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

using System;
using System.Net;
using Arrowgene.Ez2Off.Common.Json.Models;
using Arrowgene.Ez2Off.Common.Models;

namespace Arrowgene.Ez2Off.Server.Api.Routes
{
    public class LoginRoute : ApiRequest
    {
        public override string Route => "/login";

        public LoginRoute(ApiServer server) : base(server)
        {
        }

        public override async void Post(HttpListenerContext ctx)
        {
            LoginRequest request = await ReadJsonObject<LoginRequest>(ctx.Request);
            LoginResponse response = new LoginResponse();
            
            if (!Settings.NeedRegistration)
            {
                string message = "No registration needed";
                Logger.Debug(message);
                response.Message = message;
                response.Success = true;
                response.SessionKey = SessionManager.NewSessionKey();
                SendJsonObject(ctx.Response, response);
                return;
            }

            if (request == null)
            {
                string message = "Invalid json request";
                Logger.Error(message);
                response.Message = message;
                response.Success = false;
                SendJsonObject(ctx.Response, response);
                return;
            }

            Account account = Database.SelectAccount(request.Account);
            if (account == null)
            {
                string message = String.Format("Account not found: {0}", request.Account);
                Logger.Error(message);
                response.Message = message;
                response.Success = false;
                SendJsonObject(ctx.Response, response);
                return;
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, account.Hash))
            {
                string message = String.Format("Wrong password for account: {0}", request.Account);
                Logger.Error(message);
                response.Message = message;
                response.Success = false;
                SendJsonObject(ctx.Response, response);
                return;
            }

            String sessionKey = SessionManager.NewSessionKey();
            Session session = new Session(sessionKey, account);
            Logger.Info("Created session ({0}) for {0} ", session.Key, request.Account);
            SessionManager.StoreSession(session);
            response.Success = true;
            response.SessionKey = session.Key;
        }
    }
}