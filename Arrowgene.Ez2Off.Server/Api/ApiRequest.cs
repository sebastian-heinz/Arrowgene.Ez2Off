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
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Arrowgene.Ez2Off.Common.Json;
using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Ez2Off.Server.Log;
using Arrowgene.Ez2Off.Server.Sessions;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Services.Logging;

namespace Arrowgene.Ez2Off.Server.Api
{
    public abstract class ApiRequest
    {
        private static string defaultMime = "application/octet-stream";

        private static IDictionary<string, string> _mimeTypeMappings =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                {".avi", "video/x-msvideo"},
                {".bin", "application/octet-stream"},
                {".css", "text/css"},
                {".dll", "application/octet-stream"},
                {".exe", "application/octet-stream"},
                {".gif", "image/gif"},
                {".htm", "text/html"},
                {".html", "text/html"},
                {".ico", "image/x-icon"},
                {".img", "application/octet-stream"},
                {".jar", "application/java-archive"},
                {".jpeg", "image/jpeg"},
                {".jpg", "image/jpeg"},
                {".js", "application/x-javascript"},
                {".mov", "video/quicktime"},
                {".mp3", "audio/mpeg"},
                {".mpeg", "video/mpeg"},
                {".mpg", "video/mpeg"},
                {".pdf", "application/pdf"},
                {".png", "image/png"},
                {".rar", "application/x-rar-compressed"},
                {".shtml", "text/html"},
                {".txt", "text/plain"},
                {".xml", "text/xml"},
                {".zip", "application/zip"},
            };

        public static void SendHtml(HttpListenerResponse response, string html)
        {
            response.ContentType = "text/html";
            response.StatusCode = (int) HttpStatusCode.OK;
            SendResponse(response, Encoding.UTF8.GetBytes(html));
        }

        public static void SendHtml(HttpListenerResponse response, string html, string title)
        {
            string document = String.Format("<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\">" +
                                            "<title>{0}</title>" +
                                            "</head><body>{1}</body></html>", title, html);
            response.ContentType = "text/html";
            response.StatusCode = (int) HttpStatusCode.OK;
            SendResponse(response, Encoding.UTF8.GetBytes(document));
        }

        public static void SendJson(HttpListenerResponse response, string json)
        {
            response.ContentType = "application/json";
            response.StatusCode = (int) HttpStatusCode.OK;
            SendResponse(response, Encoding.UTF8.GetBytes(json));
        }

        public static void SendJsonObject<T>(HttpListenerResponse response, T obj)
        {
            response.ContentType = "application/json";
            response.StatusCode = (int) HttpStatusCode.OK;
            string json = JsonSerializer.Serialize(obj);
            SendResponse(response, Encoding.UTF8.GetBytes(json));
        }

        public static void SendError(HttpListenerResponse response, string error)
        {
            response.StatusCode = (int) HttpStatusCode.InternalServerError;
            SendResponse(response, Encoding.UTF8.GetBytes(error));
        }

        public static void SendNotFound(HttpListenerResponse response)
        {
            response.StatusCode = (int) HttpStatusCode.NotFound;
            response.Headers.Clear();
            response.SendChunked = false;
            response.ContentLength64 = 0;
            response.Close();
        }

        public static void SendResponse(HttpListenerResponse response, byte[] data)
        {
            int contentLength = data.Length;
            response.SendChunked = false;
            response.ContentLength64 = contentLength;
            using (var output = response.OutputStream)
            {
                using (var writer = new BinaryWriter(output))
                {
                    writer.Write(data);
                    writer.Flush();
                }
            }
        }

        public static async void SendFile(HttpListenerResponse response, FileInfo file, Logger logger = null)
        {
            if (file == null || !file.Exists)
            {
                SendNotFound(response);
                return;
            }

            try
            {
                string mime;
                if (!_mimeTypeMappings.TryGetValue(file.Extension, out mime))
                {
                    mime = defaultMime;
                }

                response.StatusCode = (int) HttpStatusCode.OK;
                response.ContentType = mime;
                response.AddHeader("Date", DateTime.Now.ToString("r"));
                response.AddHeader("Last-Modified", file.LastWriteTime.ToString("r"));

                using (Stream input = new FileStream(file.FullName, FileMode.Open))
                {
                    response.ContentLength64 = input.Length;
                    using (Stream output = response.OutputStream)
                    {
                        await input.CopyToAsync(output);
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                if (logger != null)
                {
                    logger.Exception(ex);
                }
                else
                {
                    Debug.Write(ex);
                }
            }
        }

        protected EzLogger Logger { get; }
        protected DirectoryInfo WebRoot { get; }
        protected ApiSettings Settings { get; }
        protected IDatabase Database { get; }
        protected ISessionManager SessionManager { get; }

        public ApiRequest(ApiServer server)
        {
            Logger = LogProvider<EzLogger>.GetLogger(this);
            Settings = server.Settings;
            SessionManager = server.SessionManager;
            Database = server.Database;
            if (!string.IsNullOrEmpty(Settings.ApiWebRoot))
            {
                DirectoryInfo webRoot = new DirectoryInfo(Settings.ApiWebRoot);
                if (webRoot.Exists)
                {
                    WebRoot = webRoot;
                }
            }
        }

        public abstract string Route { get; }

        public virtual void Get(HttpListenerContext ctx)
        {
            SendNotFound(ctx.Response);
        }

        public virtual async void Post(HttpListenerContext ctx)
        {
            SendNotFound(ctx.Response);
        }

        public virtual void Put(HttpListenerContext ctx)
        {
            SendNotFound(ctx.Response);
        }

        public virtual void Delete(HttpListenerContext ctx)
        {
            SendNotFound(ctx.Response);
        }

        protected async Task<byte[]> ReadStream(HttpListenerRequest request)
        {
            byte[] response = new byte[0];
            Stream responseStream;
            string contentEncoding = request.Headers.Get("Content-Encoding");

            if (!string.IsNullOrEmpty(contentEncoding) && contentEncoding == "gzip")
            {
                responseStream = new GZipStream(request.InputStream, CompressionMode.Decompress);
            }
            else
            {
                responseStream = request.InputStream;
            }

            try
            {
                int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                using (responseStream)
                {
                    var read = 0;
                    while ((read = await responseStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        int newSize = response.Length + read;
                        byte[] newResponse = new byte[newSize];
                        Buffer.BlockCopy(response, 0, newResponse, 0, response.Length);
                        Buffer.BlockCopy(buffer, 0, newResponse, response.Length, read);
                        response = newResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                response = new byte[0];
            }

            return response;
        }

        protected async Task<string> ReadJson(HttpListenerRequest request)
        {
            byte[] payload = await ReadStream(request);
            return Encoding.UTF8.GetString(payload);
        }

        protected async Task<T> ReadJsonObject<T>(HttpListenerRequest request)
        {
            byte[] payload = await ReadStream(request);
            string json = Encoding.UTF8.GetString(payload);
            T obj = JsonSerializer.Deserialize<T>(json);
            return obj;
        }

        protected void TrySendFile(HttpListenerResponse response, string relativePath)
        {
            if (WebRoot == null)
            {
                Logger.Error("Webroot is not defined");
                SendError(response, "WebRoot not defined");
                return;
            }

            string absolutePath = Path.Combine(WebRoot.FullName, relativePath);
            SendFile(response, new FileInfo(absolutePath), Logger);
        }
    }
}