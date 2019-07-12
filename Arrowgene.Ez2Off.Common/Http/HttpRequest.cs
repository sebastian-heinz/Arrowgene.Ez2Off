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
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading;

namespace Arrowgene.Ez2Off.Common.Http
{
    /// <summary>
    /// Creates a statefull <see cref="HttpWebRequest"/> with configureable default values.
    /// </summary>
    public class HttpRequest
    {
        public const HttpStatusCode NoHttpStatusCodeAvailable = 0;
        public const string GetMethod = "GET";
        public const string HeadMethod = "HEAD";
        public const string PostMethod = "POST";
        public const string PutMethod = "PUT";
        public const string DeleteMethod = "DELETE";
        public const string TraceMethod = "TRACE";
        public const string OptionsMethod = "OPTIONS";

        public static long RequestSize(string url)
        {
            WebRequest req = WebRequest.Create(url);
            req.Method = "HEAD";
            using (WebResponse resp = req.GetResponse())
            {
                if (int.TryParse(resp.Headers.Get("Content-Length"), out int contentLength))
                {
                    return contentLength;
                }
            }

            return -1;
        }
        
        public static string GetRequestContent(HttpRequest request)
        {
            string page = string.Empty;
            if (request.Response != null)
            {
                if (string.IsNullOrEmpty(request.ResponseCharacterSet))
                {
                    page = Encoding.UTF8.GetString(request.Response);
                }
                else
                {
                    page = Encoding.GetEncoding(request.ResponseCharacterSet).GetString(request.Response);
                }
            }

            return page;
        }

        private Thread _asyncHttpResponseThread;
        private int _rangeFrom;
        private int _rangeTo;

        public HttpRequest()
        {
            Reset();
            ResetReturnProperties();
        }

        public event EventHandler<AsyncHttpResponseEventArgs> AsyncHttpResponse;
        public event EventHandler<HttpRequestErrorArgs> HttpRequestError;
        public event EventHandler<HttpRequestProgressArgs> HttpRequestProgress;

        public bool AllowAutoRedirect { get; set; }
        public bool PreAuthenticate { get; set; }
        public bool KeepAlive { get; set; }
        public byte[] Payload { get; set; }
        public string PayloadContentType { get; set; }
        
        public byte[] Response { get; private set; }
        public string ResponseCharacterSet { get; private set; }

        /// <summary>
        /// Size of read buffer.
        /// Bigger size results in fewer copy actions,
        /// but increases memory allocation.
        /// </summary>
        public uint BufferSize { get; set; }

        /// <summary>
        /// Timeout in ms to wait till a http response arrives.
        /// </summary>
        public int Timeout { get; set; }

        public int ReadWriteTimeout { get; set; }
        public string UserAgent { get; set; }

        /// <summary>
        /// Exception message of the last request.
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// URL of the last request.
        /// Available after a request.
        /// </summary>
        public string RequestUrl { get; private set; }

        public string Method { get; set; }
        public string Accept { get; set; }
        public IWebProxy Proxy { get; set; }
        public AuthenticationLevel AuthenticationLevel { get; set; }
        public HttpStatusCode StatusCode { get; private set; }
        public WebExceptionStatus Status { get; private set; }
        public NetworkCredential NetworkCredential { get; set; }
        public BindIPEndPoint BindIpEndPointDelegate { get; set; }
        public WebHeaderCollection RequestHeaders { get; set; }

        /// <summary>
        /// Headers of the last response.
        /// Available after a request.
        /// </summary>
        public WebHeaderCollection ResponseHeaders { get; private set; }

        public void SetRange(int from, int to)
        {
            _rangeFrom = from;
            _rangeTo = to;
        }

        public void SetCredential(string userName, string password)
        {
            NetworkCredential = new NetworkCredential(userName, password);
        }

        public void SetBasicAuthenticationHeader(string userName, string password)
        {
            string authentication =
                "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(userName + ":" + password));
            RequestHeaders.Add("Authorization", authentication);
        }

        public void AddHeader(string name, string value)
        {
            RequestHeaders.Add(name, value);
        }

        public byte[] Request(string url)
        {
            byte[] response = null;
            RequestUrl = url;
            HttpWebResponse httpResponse = null;
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest) WebRequest.Create(url);
                httpRequest.Proxy = Proxy;
                httpRequest.Timeout = Timeout;
                httpRequest.ReadWriteTimeout = ReadWriteTimeout;
                httpRequest.ServicePoint.BindIPEndPointDelegate = BindIpEndPointDelegate;
                httpRequest.UserAgent = UserAgent;
                httpRequest.Headers = RequestHeaders;
                httpRequest.AllowAutoRedirect = AllowAutoRedirect;
                httpRequest.PreAuthenticate = PreAuthenticate;
                httpRequest.AuthenticationLevel = AuthenticationLevel;
                httpRequest.KeepAlive = KeepAlive;
                httpRequest.Method = Method;
                httpRequest.Accept = Accept;
                if (_rangeFrom >= 0 && _rangeTo >= 0)
                {
                    httpRequest.AddRange(_rangeFrom, _rangeTo);
                }

                if (NetworkCredential != null)
                {
                    httpRequest.Credentials = NetworkCredential;
                }

                if (Payload != null)
                {
                    WritePostPayload(httpRequest);
                }

                ResetReturnProperties();
                httpResponse = (HttpWebResponse) httpRequest.GetResponse();
                StatusCode = httpResponse.StatusCode;
                ResponseCharacterSet = httpResponse.CharacterSet;
                response = ReadResponse(httpResponse);
            }
            catch (WebException webException)
            {
                if (httpResponse != null)
                {
                    httpResponse.Dispose();
                }
                ExceptionMessage = webException.Message;
                Status = webException.Status;
                if (webException.Status == WebExceptionStatus.ProtocolError && webException.Response != null)
                {
                    HttpWebResponse webResponse = (HttpWebResponse) webException.Response;
                    response = ReadResponse(webResponse);
                    StatusCode = webResponse.StatusCode;
                }
                else
                {
                    OnHttpRequestError(webException);
                }
            }
            catch (Exception exception)
            {
                if (httpResponse != null)
                {
                    httpResponse.Dispose();
                }
                ExceptionMessage = exception.Message;
                Status = WebExceptionStatus.UnknownError;
                OnHttpRequestError(exception);
            }
            if (httpResponse != null)
            {
                httpResponse.Dispose();
            }
            Response = response;
            return response;
        }

        public string RequestContent(string url)
        {
            string page = string.Empty;
            byte[] response = Request(url);
            return GetRequestContent(this);
        }

        public void RequestAsync(string url)
        {
            RequestUrl = url;
            _asyncHttpResponseThread = new Thread(RequestAsync);
            _asyncHttpResponseThread.Name = "AsyncHttpRequest (" + RequestUrl + ")";
            _asyncHttpResponseThread.Start();
        }

        private void RequestAsync()
        {
            byte[] response = Request(RequestUrl);
            OnAsyncHttpResponse(response);
        }

        public void Reset()
        {
            BufferSize = 2048;
            Timeout = 2000;
            ReadWriteTimeout = 2000;
            NetworkCredential = null;
            Proxy = null;
            BindIpEndPointDelegate = null;
            RequestHeaders = new WebHeaderCollection();
            AllowAutoRedirect = false;
            PreAuthenticate = false;
            AuthenticationLevel = AuthenticationLevel.None;
            KeepAlive = false;
            Payload = null;
            PayloadContentType = "application/x-www-form-urlencoded";
            Method = GetMethod;
            _rangeFrom = -1;
            _rangeTo = -1;
            ResponseCharacterSet = null;
            Response = null;
        }

        private byte[] ReadResponse(HttpWebResponse httpResponse)
        {
            byte[] response = new byte[0];
            Stream responseStream = null;
            ResponseHeaders = httpResponse.Headers;
            string contentEncoding = httpResponse.Headers.Get("Content-Encoding");
            if (!string.IsNullOrEmpty(contentEncoding) && contentEncoding == "gzip")
            {
                responseStream = new GZipStream(httpResponse.GetResponseStream(), CompressionMode.Decompress);
            }
            else
            {
                responseStream = httpResponse.GetResponseStream();
            }

            byte[] buffer = new byte[BufferSize];
            long totalLength;
            try
            {
                totalLength = responseStream.Length;
            }
            catch (NotSupportedException)
            {
                totalLength = -1;
            }

            using (responseStream)
            {
                var read = 0;
                while ((read = responseStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    int newSize = response.Length + read;
                    byte[] newResponse = new byte[newSize];
                    Buffer.BlockCopy(response, 0, newResponse, 0, response.Length);
                    Buffer.BlockCopy(buffer, 0, newResponse, response.Length, read);
                    response = newResponse;
                    OnHttpRequestProgress(response.Length, totalLength);
                }
            }

            return response;
        }

        private void WritePostPayload(HttpWebRequest httpRequest)
        {
            httpRequest.ContentType = PayloadContentType;
            httpRequest.ContentLength = Payload.Length;
            using (Stream stream = httpRequest.GetRequestStream())
            {
                stream.Write(Payload, 0, Payload.Length);
            }
        }

        private void ResetReturnProperties()
        {
            Status = WebExceptionStatus.Success;
            StatusCode = NoHttpStatusCodeAvailable;
        }

        private void OnAsyncHttpResponse(byte[] response)
        {
            EventHandler<AsyncHttpResponseEventArgs> asyncHttpResponse = AsyncHttpResponse;
            if (asyncHttpResponse != null)
            {
                AsyncHttpResponseEventArgs asyncHttpResponseEventArgs = new AsyncHttpResponseEventArgs(response);
                asyncHttpResponse(this, asyncHttpResponseEventArgs);
            }
        }

        private void OnHttpRequestError(Exception exception)
        {
            EventHandler<HttpRequestErrorArgs> httpRequestError = HttpRequestError;
            if (httpRequestError != null)
            {
                HttpRequestErrorArgs httpRequestErrorArgs = new HttpRequestErrorArgs(exception);
                httpRequestError(this, httpRequestErrorArgs);
            }
        }

        private void OnHttpRequestProgress(long current, long total)
        {
            EventHandler<HttpRequestProgressArgs> httpRequestProgress = HttpRequestProgress;
            if (httpRequestProgress != null)
            {
                HttpRequestProgressArgs httpRequestProgressArgs = new HttpRequestProgressArgs(current, total);
                httpRequestProgress(this, httpRequestProgressArgs);
            }
        }

        public override string ToString()
        {
            string headers = string.Empty;
            foreach (string key in RequestHeaders.AllKeys)
            {
                headers += key + " " + RequestHeaders.Get(key) + "; ";
            }

            return string.Format(
                "URL:{0}\r\n Method:{1}\r\n StatusCode:{2}\r\n Timeout:{3}\r\n ReadWriteTimeout:{4}\r\n Headers:{5}",
                RequestUrl, Method, StatusCode, Timeout, ReadWriteTimeout, headers);
        }
    }
}