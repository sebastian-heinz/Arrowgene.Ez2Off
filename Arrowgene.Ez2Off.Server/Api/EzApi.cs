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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Server.Api.Routes;
using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Ez2Off.Server.Sessions;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Services.Logging;

namespace Arrowgene.Ez2Off.Server.Api
{
    /// <summary>
    /// Communication via HTTP
    /// - Login & SessionKey generation
    /// </summary>
    public class ApiServer : IDisposable
    {
        private const int AccessDenied = 5;

        private HttpListener _listener;
        private bool _isRunning;

        private Dictionary<string, ApiRequest> _requestHandler;
        private Logger _logger;
        private DirectoryInfo _webRoot;
        private FileInfo _index;

        public ApiServer(ApiSettings settings, ISessionManager sessionManager, IDatabase database)
        {
            Settings = settings;
            SessionManager = sessionManager;
            Database = database;
            _requestHandler = new Dictionary<string, ApiRequest>();
            _logger = LogProvider<Logger>.GetLogger(this);

            if (string.IsNullOrEmpty(Settings.ApiWebRoot))
            {
                _logger.Error("WebRoot is empty, not serving any static files");
            }
            else
            {
                DirectoryInfo webRoot = new DirectoryInfo(Settings.ApiWebRoot);
                if (!webRoot.Exists)
                {
                    _logger.Error("WebRoot ({0}) does not exist, not serving any static files", webRoot.FullName);
                }
                else
                {
                    _webRoot = webRoot;
                    _index = new FileInfo(Path.Combine(_webRoot.FullName, "index.html"));
                    _logger.Info("Serving files from webroot: {0}", webRoot.FullName);
                }
            }

            AddRequestHandler(new LoginRoute(this));
            AddRequestHandler(new RegistrationRoute(this));
        }

        public ApiSettings Settings { get; }
        public IDatabase Database { get; }
        public ISessionManager SessionManager { get; }

        public void Dispose()
        {
            Stop();
        }

        public void AddRequestHandler(ApiRequest request)
        {
            if (_requestHandler.ContainsKey(request.Route))
            {
                _requestHandler[request.Route] = request;
            }
            else
            {
                _requestHandler.Add(request.Route, request);
            }
        }

        public void Start()
        {
            _listener = new HttpListener();
            string prefix = string.Format("http://{0}:{1}/", Settings.ApiPrefix, Settings.ApiPort);
            _listener.Prefixes.Add(prefix);
            try
            {
                _listener.Start();
            }
            catch (HttpListenerException e)
            {
                if (e.ErrorCode == AccessDenied)
                {
                    _logger.Error("No permission to listen on ({0}), try with elevated privileges", prefix);
                    return;
                }

                throw;
            }

            _isRunning = true;
            _logger.Info("Listening on: {0} serving {1} routes", prefix, _requestHandler.Count);
            foreach (ApiRequest apiRequest in _requestHandler.Values)
            {
                _logger.Info("Route: {0}", apiRequest.Route);
            }

            Task.Run(() =>
            {
                var semaphore = new Semaphore(
                    Settings.ApiMaximumConnectionCount,
                    Settings.ApiMaximumConnectionCount
                );
                while (_isRunning)
                {
                    semaphore.WaitOne();
                    try
                    {
                        _listener.GetContextAsync().ContinueWith(async contextTask =>
                        {
                            semaphore.Release();
                            var context = await contextTask.ConfigureAwait(false);
                            Process(context);
                        });
                    }
                    catch (Exception)
                    {
                        if (_isRunning)
                        {
                            throw;
                        }
                    }
                }
            });
        }

        public void Stop()
        {
            if (_listener != null && _listener.IsListening)
            {
                _isRunning = false;
                _listener.Stop();
            }
        }

        private void Process(HttpListenerContext ctx)
        {
            string request = ctx.Request.Url.LocalPath;
            request = request.ToLower();
            _logger.Debug("New Request: ({0}) {1}", ctx.Request.HttpMethod, request);
            if (Handle(request, ctx))
            {
                _logger.Debug("Handled: ({0}) {1}", ctx.Request.HttpMethod, request);
            }
            else
            {
                _logger.Error("Request could not be handled: ({0}) {1}", ctx.Request.HttpMethod, request);
                ApiRequest.SendNotFound(ctx.Response);
            }
        }

        private bool Handle(string request, HttpListenerContext ctx)
        {
            if (_requestHandler.ContainsKey(request))
            {
                ApiRequest requestHandler = _requestHandler[request];
                switch (ctx.Request.HttpMethod)
                {
                    case "GET":
                        requestHandler.Get(ctx);
                        break;
                    case "POST":
                        requestHandler.Post(ctx);
                        break;
                    case "PUT":
                        requestHandler.Put(ctx);
                        break;
                    case "DELETE":
                        requestHandler.Delete(ctx);
                        break;
                }

                return true;
            }

            return TryServeFile(ctx);
        }

        private bool TryServeFile(HttpListenerContext ctx)
        {
            if (_webRoot == null)
            {
                _logger.Error("Webroot is not defined");
            }
            else if (ctx.Request.HttpMethod == "GET")
            {
                string requestFile = ctx.Request.Url.AbsolutePath;
                requestFile = Utils.UnrootPath(requestFile);
                if (requestFile == "" && _index != null && _index.Exists)
                {
                    ApiRequest.SendFile(ctx.Response, _index);
                    return true;
                }

                string filePath = Path.Combine(_webRoot.FullName, requestFile);
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    ApiRequest.SendFile(ctx.Response, fileInfo);
                    return true;
                }
            }

            return false;
        }
    }
}