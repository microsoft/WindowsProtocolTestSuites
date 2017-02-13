// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Http
{
    /// <summary>
    /// A wrapper class to System.Uri.
    /// The purpose is to support path without schema ahead,
    /// which is used in HTTP request and elsewhere.
    /// </summary>
    public class Url
    {
        private string rawUrl;
        private Uri uri;
        private readonly string baseUrl = @"http://localhost";

        public string Schema
        {
            get
            {
                return uri.Scheme;
            }
        }

        public string Host
        {
            get
            {
                return uri.DnsSafeHost;
            }
        }

        public int Port
        {
            get
            {
                return uri.Port;
            }
        }

        public string Path
        {
            get
            {
                if (uri.AbsolutePath == null || uri.AbsolutePath.Length == 0)
                    return "";
                else
                    return uri.AbsolutePath.Substring(1);
            }
        }

        public string Query
        {
            get
            {
                return uri.Query;
            }
        }

        public string Fragment
        {
            get
            {
                return uri.Fragment;
            }
        }

        public Url(string url)
        {
            try
            {
                // if contains schema
                if (url.Contains(":"))
                    this.uri = new Uri(url);
                // if starts from relative path
                else if (url[0] == '/')
                    this.uri = new Uri(this.baseUrl + url);
                else
                    this.uri = new Uri(this.baseUrl + "/" + url);

                this.rawUrl = url;
            }
            catch
            {
                throw;
            }
        }

        public List<KeyValuePair<string, string>> GetQueryPairs()
        {
            List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();

            if (uri.Query != null)
            {
                string key = "", value = "";
                bool settingKey = true;
                for (int i = 1; i < uri.Query.Length; i++)
                {
                    if (uri.Query[i] != '=' && uri.Query[i] != '&')
                    {
                        if (settingKey)
                            key += uri.Query[i];
                        else
                            value += uri.Query[i];

                    }
                    else
                    {
                        if (uri.Query[i] == '=')
                            settingKey = false;
                        else if (uri.Query[i] == '&')
                        {
                            ret.Add(new KeyValuePair<string, string>(key, value));
                            key = "";
                            value = "";
                            settingKey = true;
                        }
                    }
                }
                if (key != "")
                    ret.Add(new KeyValuePair<string, string>(key, value));
            }

            return ret;
        }

        public override string ToString()
        {
            return rawUrl;
        }
    }

    public abstract class HttpMessage
    {
        public readonly HashSet<KeyValuePair<string, string>> Header = new HashSet<KeyValuePair<string, string>>();

        protected string responseHeaderFieldNameToString(HttpResponseHeader val)
        {
            switch (val)
            {
                case HttpResponseHeader.ContentLength:
                    return "Content-Length";
                case HttpResponseHeader.ContentType:
                    return "Content-Type";
                case HttpResponseHeader.WwwAuthenticate:
                    return "WWW-Authenticate";
                case HttpResponseHeader.TransferEncoding:
                    return "Transfer-Encoding";
                case HttpResponseHeader.SetCookie:
                    return "Set-Cookie";
            }
            return val.ToString();
        }

        protected string requestHeaderFieldNameToString(HttpRequestHeader val)
        {
            switch (val)
            {
                case HttpRequestHeader.ContentLength:
                    return "Content-Length";
                case HttpRequestHeader.ContentType:
                    return "Content-Type";
                case HttpRequestHeader.TransferEncoding:
                    return "Transfer-Encoding";
            }
            return val.ToString();
        }

        public string Body { get; set; }
    }

    public class HttpRequest : HttpMessage
    {
        public enum HttpMethod
        {
            GET,
            POST,
            HEAD,
            PUT,
            DELETE,
            TRACE,
            CONNECT,
            OPTIONS
        };

        public void SetHeaderField(HttpRequestHeader key, string value)
        {
            string strKey = requestHeaderFieldNameToString(key);

            int capIndex = (from ch in strKey.ToArray()
                            where char.IsUpper(ch)
                            select strKey.IndexOf(ch)).First();

            if (capIndex > 1) strKey.Insert(capIndex, "-");

            Header.Add(new KeyValuePair<string, string>(strKey, value));
        }

        private Version version;

        private Url requestUrl;

        private HttpMethod method;

        public Version Version
        {
            get
            {
                return this.version;
            }
        }

        public Url RequestUrl
        {
            get
            {
                return this.requestUrl;
            }
        }

        public HttpMethod Method
        {
            get
            {
                return this.method;
            }
        }

        public HttpRequest()
        {
        }

        public HttpRequest(HttpMethod method, string Url)
        {
            this.method = method;
            this.requestUrl = new Url(Url);
            this.version = HttpVersion.Version11; // use HTTP/1.1 as default
        }

        public void Parse(string request)
        {
            string[] lines = request.Split(new[] { "\r\n" }, StringSplitOptions.None);

            // parse the request-line
            string[] requestLine = lines[0].Split(' ');
            this.method = (HttpMethod)Enum.Parse(typeof(HttpMethod), requestLine[0], true);
            this.requestUrl = new Url(requestLine[1]);
            this.version = (requestLine[2].Equals("HTTP/1.1")) ? HttpVersion.Version11 : HttpVersion.Version10;

            // get request header and body      
            bool headerEnd = false;

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                if (string.IsNullOrEmpty(line))
                {
                    headerEnd = true;
                    continue;
                }

                if (headerEnd)
                {
                    this.Body += (line + "\r\n");
                }
                else
                {
                    int separator = line.IndexOf(":");
                    this.Header.Add(new KeyValuePair<string, string>(line.Substring(0, separator),
                        line.Substring(separator + 1, line.Length - separator - 1).TrimStart()));
                }
            }
        }

        public string GetHeaderFieldValue(HttpRequestHeader key)
        {
            var query =
                from field in this.Header
                where field.Key.Replace("-", "").ToLower() == key.ToString().ToLower()
                select field;

            if (!query.Any())
                return null;
            else
                return query.First().Value;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            using (StringWriter writer = new StringWriter(builder))
            {
                writer.NewLine = "\r\n";
                writer.WriteLine(this.method.ToString() + " " +
                                 this.requestUrl.ToString() + " " +
                               ((this.version == HttpVersion.Version11) ? "HTTP/1.1" : "HTTP/1.0"));

                foreach (var field in this.Header)
                {
                    writer.WriteLine(field.Key + ": " + field.Value);
                }

                writer.WriteLine();
                if (this.Body != null)
                    writer.WriteLine(this.Body);
            }

            return builder.ToString();
        }

    }

    public class HttpResponse : HttpMessage
    {
        public static readonly Dictionary<HttpStatusCode, string> ReasonPhase
            = new Dictionary<HttpStatusCode, string> {
			    { HttpStatusCode.OK,                     "200 OK" },
                { HttpStatusCode.Created,                "201 Created" },
                { HttpStatusCode.Accepted,               "202 Accepted" },
                { HttpStatusCode.NoContent,              "204 No Content" },
                { HttpStatusCode.MovedPermanently,       "301 Moved Permanently" },
                { HttpStatusCode.Redirect,               "302 Redirection" },
                { HttpStatusCode.NotModified,            "304 Not Modified" },
                { HttpStatusCode.BadRequest,             "400 Bad Request" },
                { HttpStatusCode.Unauthorized,           "401 Unauthorized" },
                { HttpStatusCode.Forbidden,              "403 Forbidden" },
                { HttpStatusCode.NotFound,               "404 Not Found" },
                { HttpStatusCode.InternalServerError,    "500 Internal Server Error" },
                { HttpStatusCode.NotImplemented,         "501 Not Implemented" },
                { HttpStatusCode.BadGateway,             "502 Bad Gateway" },
                { HttpStatusCode.ServiceUnavailable,     "503 Service Unavailable" },
                { HttpStatusCode.Continue,               "100 Continue"},
                {HttpStatusCode.TemporaryRedirect,       "307 Temporary Redirect"}
            };

        private Version version;

        private HttpStatusCode statusCode;

        public HttpStatusCode StatusCode { get { return statusCode; } }

        private string reasonPhrase;

        public HttpResponse()
        {
        }

        public HttpResponse(HttpStatusCode statusCode)
        {
            this.statusCode = statusCode;
            this.reasonPhrase = HttpResponse.ReasonPhase[statusCode];
            this.version = HttpVersion.Version11; // use HTTP/1.1 as default
        }

        public HttpResponse(HttpStatusCode statusCode, string body)
            : this(statusCode)
        {
            this.Body = body;
        }

        public string GetHeaderFieldValue(HttpResponseHeader key)
        {
            var query =
                from field in this.Header
                where field.Key.Replace("-", "").ToLower() == key.ToString().ToLower()
                select field;

            if (!query.Any())
                return null;
            else
                return query.First().Value;
        }

        public string[] GetHeaderFieldValueArray(HttpResponseHeader key)
        {
            List<string> ret = new List<string>();
            var query =
                from field in this.Header
                where field.Key.Replace("-", "").ToLower() == key.ToString().ToLower()
                select field;

            if (!query.Any())
                return null;
            else
            {

                foreach (var q in query)
                {
                    ret.Add(q.Value);
                }
            }
            return ret.ToArray();
        }

        public void RemoveHeaderFieldValue(HttpResponseHeader key)
        {
            HashSet<KeyValuePair<string, string>>.Enumerator enumer = Header.GetEnumerator();
            while (enumer.MoveNext())
            {
                if (enumer.Current.Key.Replace("-", "").ToLower() == key.ToString().ToLower())
                {
                    Header.Remove(enumer.Current);
                    return;
                }
            }
        }

        public void Parse(string response)
        {
            string[] lines = response.Split(new[] { "\r\n" }, StringSplitOptions.None);

            // parse the request-line
            string[] responseLine = lines[0].Split(' ');
            this.version = (responseLine[0].Equals("HTTP/1.1")) ? HttpVersion.Version11 : HttpVersion.Version10;
            this.statusCode = (HttpStatusCode)int.Parse(responseLine[1]);
            this.reasonPhrase = responseLine[2];

            // get request header and body      
            bool headerEnd = false;

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                if (string.IsNullOrEmpty(line))
                {
                    headerEnd = true;
                    continue;
                }

                if (headerEnd)
                {
                    this.Body += (line + "\r\n");
                }
                else
                {
                    int separator = line.IndexOf(":");
                    this.Header.Add(new KeyValuePair<string, string>(line.Substring(0, separator),
                        line.Substring(separator + 1, line.Length - separator - 1).TrimStart()));
                }
            }
            if (this.Body != null)
                this.Body += "\r\n";
        }

        public void SetHeaderField(HttpResponseHeader key, string value)
        {
            string strKey = responseHeaderFieldNameToString(key);

            int capIndex = (from ch in strKey.ToArray()
                            where char.IsUpper(ch)
                            select strKey.IndexOf(ch)).First();

            if (capIndex > 1) strKey.Insert(capIndex, "-");

            Header.Add(new KeyValuePair<string, string>(strKey, value));
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            using (StringWriter writer = new StringWriter(builder))
            {
                writer.NewLine = "\r\n";
                writer.WriteLine(((this.version == HttpVersion.Version11) ? "HTTP/1.1" : "HTTP/1.0") + " "
                   + HttpResponse.ReasonPhase[this.statusCode]);

                foreach (var field in this.Header)
                {
                    writer.WriteLine(field.Key + ": " + field.Value);
                }

                writer.WriteLine();
                if (this.Body != null)
                    writer.Write(this.Body);
            }

            return builder.ToString();
        }
    }

}
