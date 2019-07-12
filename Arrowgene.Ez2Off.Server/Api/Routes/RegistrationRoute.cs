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
using Arrowgene.Ez2Off.Server.Settings;

namespace Arrowgene.Ez2Off.Server.Api.Routes
{
    public class RegistrationRoute : ApiRequest
    {
        public override string Route => "/register";

        public RegistrationRoute(ApiServer server) : base(server)
        {
        }

        public override async void Post(HttpListenerContext ctx)
        {
            RegisterRequest request = await ReadJsonObject<RegisterRequest>(ctx.Request);
            RegisterResponse response = new RegisterResponse();
            string message;

            if (String.IsNullOrWhiteSpace(request.Account) || String.IsNullOrWhiteSpace(request.Password))
            {
                message = String.Format("Could not create account ({0}), account or password empty", request.Account);
                Logger.Error(message);
                response.Success = false;
                response.Message = message;
                SendJsonObject(ctx.Response, response);
                return;
            }

            string bCryptHash = BCrypt.Net.BCrypt.HashPassword(request.Password, ApiSettings.BCryptWorkFactor);
            Account account = Database.CreateAccount(request.Account, bCryptHash);
            if (account == null)
            {
                message = String.Format("Could not create account ({0}), account already exists", request.Account);
                Logger.Error(message);
                response.Success = false;
                response.Message = message;
                SendJsonObject(ctx.Response, response);
                return;
            }

            message = String.Format("Created new account: {0}", request.Account);
            Logger.Info(message);
            response.Success = true;
            response.Message = message;
            SendJsonObject(ctx.Response, response);
        }

        public override void Get(HttpListenerContext ctx)
        {
            TrySendFile(ctx.Response, "register.html");
        }
    }
}