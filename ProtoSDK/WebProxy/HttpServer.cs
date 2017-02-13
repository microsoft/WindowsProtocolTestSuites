// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Http
{
    /// <summary>
    /// event type exposed to application layer
    /// </summary>
    public class ManagedEvent
    {
        public EventType EventType { get; set; }

        public HttpEndpointBase ServiceEndpoint { get; set; }

        public IPEndPoint Sender { get; set; }

        public ManagedStream TransportContext { get; set; }

        public HttpRequest ParsedHttpRequest { get; set; }

        public byte[] Data { get; set; }

        public string Text { get; set; }

        public Exception Exception { get; set; }
    }

    public enum EventType
    {
        /// <summary>
        /// Used internally only
        /// </summary>
        Unhandled = 0,

        /// <summary>
        /// It's a request to a service
        /// </summary>
        IncomingRequest,

        /// <summary>
        /// Internal exception happens
        /// </summary>
        Exception,
    }


    /// <summary>
    /// manage stream with access time provided
    /// </summary>
    public class ManagedStream
    {
        public IPEndPoint RemoteEndpoint { get; set; }

        public TcpClient LocalTcpClient { get; set; }

        public System.IO.Stream LocalStream { get; set; }

        public DateTime LastUsed { get; set; }

        public TcpPortInfo PortInfo { get; set; }
    }

    public class TcpPortInfo
    {
        public TcpPortInfo()
        {
            EncryptionPolicy = System.Net.Security.EncryptionPolicy.AllowNoEncryption;
        }

        public int Port { get; set; }
        public bool IsSSL { get; set; }
        public X509Certificate2 ServerCertificate { get; set; }
        public EncryptionPolicy EncryptionPolicy { get; set; }
        public string Hostname { get; set; }
    }

    /// <summary>
    /// HTTP server for supporting multiple upper level HTTP endpoints. Each HTTP endpoint should register its URL to this server to send and receive messages
    /// </summary>
    public class HttpServer : IDisposable
    {
        private static HttpServer Instance = null;

        public static HttpServer GetInstance()
        {
            lock (staLock)
            {
                if (Instance == null)
                    Instance = new HttpServer();
                return Instance;
            }
        }

        private static object staLock = new object();

        /// <summary>
        /// singleton, not allow create outside, keep only one instance.
        /// </summary>
        private HttpServer()
        {
        }

        /// <summary>
        /// it's an internal class
        /// </summary>
        private class ManageListeningContext
        {
            public TcpListener Listener;

            public TcpPortInfo PortInfo;

            public Thread ListeningThread;

            public readonly List<Thread> ReceiveThreads = new List<Thread>();
        }


        private readonly Dictionary<int, ManageListeningContext> m_listeners = new Dictionary<int, ManageListeningContext>();

        private readonly Dictionary<int, List<HttpEndpointBase>> m_endpoints = new Dictionary<int, List<HttpEndpointBase>>();

        /// <summary>
        /// synchronized event pipe
        /// </summary>
        private readonly Queue<ManagedEvent> eventPipe = new Queue<ManagedEvent>();

        /// <summary>
        /// cache connected client
        /// </summary>
        private readonly List<ManagedStream> clientStreams = new List<ManagedStream>();

        /// <summary>
        /// cache client used endpoint, only used for scenarios of HTTP Bind protocols
        /// </summary>
        private readonly Dictionary<ManagedStream, HttpEndpointBase> clientEndpointMap = new Dictionary<ManagedStream, HttpEndpointBase>();

        // thread related
        private bool running = false;

        /// <summary>
        /// True if server is running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (staLock)
                {
                    return running;
                }
            }
        }

        private TimeSpan idleTimeout;

        /// <summary>
        /// How long does any TCP connection can be Idle
        /// </summary>
        public TimeSpan TcpIdleTimeout
        {
            get
            {
                return idleTimeout;
            }
        }

        /// <summary>
        /// must be called at very beginning when use HTTPS, should only initialize once
        /// </summary>
        /// <param name="port">port number listens on</param>
        /// <param name="idleTimeout">how much time can a client be idle before been force closed</param>
        public void Initialize(TimeSpan idleTimeout)
        {
            lock (staLock)
            {
                if (!running)
                {
                    this.idleTimeout = idleTimeout;
                    this.running = true;
                }
                else
                    throw new Exception("Already running");
            }
        }

        /// <summary>
        /// Called to added new port to be listened by HttpServer
        /// </summary>
        /// <param name="p">Detail information of the port</param>
        public void AddPort(TcpPortInfo p)
        {
            lock (staLock)
            {
                if (running)
                {
                    ManageListeningContext context = new ManageListeningContext();
                    context.PortInfo = p;
                    if (p.IsSSL && p.ServerCertificate == null)
                        throw new Exception("Port " + p.Port.ToString() + " is set to using SSL but server certificate is NULL");
                    // start create the listening thread
                    context.Listener = new TcpListener(IPAddress.Any, p.Port);
                    // NEVER catch exception here, we need directly know what happens when we require a Port
                    context.Listener.Start();
                    context.ListeningThread = new Thread(new ParameterizedThreadStart(ListenLoop));
                    context.ListeningThread.Start(context);
                    lock (m_listeners)
                    {
                        m_listeners.Add(p.Port, context);
                    }
                }
                else
                    throw new Exception("Not running");
            }
        }

        /// <summary>
        /// register a HTTP service, will forward message to the URL it has registered
        /// </summary>
        /// <param name="service"></param>
        /// <returns>true if success</returns>
        internal void RegisterHttpEndpoint(HttpEndpointBase service)
        {
            lock (staLock)
            {
                lock (m_endpoints)
                {
                    lock (m_listeners)
                    {
                        if (!m_listeners.ContainsKey(service.Port))
                            throw new Exception("Http Service is not initialized to listen port: " + service.Port);
                        if (service.IsSSL)
                            if (m_listeners[service.Port].PortInfo.ServerCertificate == null)
                                throw new Exception("Port " + service.Port.ToString() + " does not register a server certificate");

                    }

                    // try to find duplicattion
                    //if port is using
                    if (m_endpoints.ContainsKey(service.Port))
                    {
                        List<HttpEndpointBase> endpoints = m_endpoints[service.Port];
                        foreach (HttpEndpointBase b in endpoints)
                        {
                            foreach (string u in service.Urls)
                            {
                                foreach (string b_u in b.Urls)
                                {
                                    //found duplicated url in same port
                                    if (0 == string.Compare(u.ToLower(), b_u.ToLower()))
                                    {
                                        throw new Exception("Duplicated Url is regsitered: " + u);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //if port is not using
                        m_endpoints.Add(service.Port, new List<HttpEndpointBase>());
                    }
                    m_endpoints[service.Port].Add(service);
                }
            }
        }

        /// <summary>
        /// send message to an accepted client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        /// <returns>true if success</returns>
        public void SendMessage(IPEndPoint client, byte[] msg)
        {
            lock (staLock)
            {
                if (!this.running)
                    throw new Exception("HttpServer is not running");

                // find cached client
                var q =
                    from c in this.clientStreams
                    where c.RemoteEndpoint == client
                    select c;

                if (!q.Any())
                {
                    throw new Exception("Cannot find existing remote endpoint: " + client.ToString());
                }
                else
                {
                    var ms = q.First();
                    // try to send message
                    lock (ms)
                    {
                        ms.LocalStream.Write(msg, 0, msg.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Tcp Listen thread
        /// </summary>
        private void ListenLoop(object o)
        {
            ManageListeningContext context = o as ManageListeningContext;

            Trace.WriteLine("Server listener started");

            while (this.running)
            {
                TcpClient client = null;
                try
                {
                    client = context.Listener.AcceptTcpClient();
                }
                catch
                {
                    Trace.TraceError("client side may disconnected");
                    return;
                }

                Trace.WriteLine(string.Format("Client {0} connected",
                    (client.Client.RemoteEndPoint as IPEndPoint).ToString()));

                lock (context)
                {
                    ManagedStream ms = new ManagedStream();
                    ms.LocalTcpClient = client;
                    ms.PortInfo = context.PortInfo;
                    ms.RemoteEndpoint = client.Client.RemoteEndPoint as IPEndPoint;
                    Thread task = new Thread(new ParameterizedThreadStart(ReceiveLoop));
                    task.Start(ms);
                    lock (clientStreams)
                    {
                        clientStreams.Add(ms);
                    }
                    context.ReceiveThreads.Add(task);
                }
            }
        }

        /// <summary>
        /// Tcp Client thread
        /// </summary>
        /// <param name="o"></param>
        private void ReceiveLoop(object o)
        {
            ManagedStream ms = o as ManagedStream;
            TimeSpan idleTime = TimeSpan.FromMilliseconds(0);
            TimeSpan sleepTime = TimeSpan.FromMilliseconds(200);
            Stream s;
            HttpEndpointBase usedEndpoint = null;

            TcpClient c = ms.LocalTcpClient;
            // id it's using SSL, must authenticate self as server firstly
            if (ms.PortInfo.IsSSL)
            {
                SslStream ssl = new SslStream(c.GetStream(), true);
                try
                {
                    ssl.AuthenticateAsServer(ms.PortInfo.ServerCertificate, false,
                        System.Security.Authentication.SslProtocols.Tls, false);
                }
                catch (Exception e)
                {
                    lock (eventPipe)
                    {
                        ManagedEvent exp = new ManagedEvent();
                        exp.EventType = EventType.Exception;
                        exp.Exception = e;
                        eventPipe.Enqueue(exp);
                        return;
                    }
                }
                s = ssl;
            }
            else
            {
                s = c.GetStream();
            }
            ms.LocalStream = s;

            using (s)
            {
                while (this.running && idleTime < this.idleTimeout)
                {
                    // if no data available, wait for a while
                    if (c.Available == 0)
                    {
                        Thread.Sleep(sleepTime);
                        idleTime += sleepTime;
                    }
                    // if data coming
                    else
                    {
                        idleTime = TimeSpan.FromMilliseconds(0);

                        byte[] data;

                        // receive data
                        using (var t = new MemoryStream())
                        {
                            var buf = new byte[c.ReceiveBufferSize];

                            lock (ms)
                            {
                                do
                                {
                                    var bytes = s.Read(buf, 0, buf.Length);
                                    t.Write(buf, 0, bytes);

                                } while (c.Available > 0);

                                ms.LastUsed = DateTime.Now;
                            }

                            data = t.ToArray();
                        }

                        // pack as ManagedEvent
                        var e = new ManagedEvent();
                        e.Data = data;
                        e.Sender = c.Client.RemoteEndPoint as IPEndPoint;
                        e.EventType = EventType.IncomingRequest;

                        //parse the endpoint
                        string str = Encoding.UTF8.GetString(data);
                        e.Text = str;
                        // try to parse as http
                        try
                        {
                            HttpRequest req = new HttpRequest();
                            req.Parse(e.Text);
                            e.ParsedHttpRequest = req;
                        }
                        catch
                        {

                        }
                        if (usedEndpoint == null)
                        {
                            if (e.ParsedHttpRequest == null)
                            {
                                //first message is not http
                                e.EventType = EventType.Exception;
                                e.Exception = new Exception("The 1st request from this context is not an HTTP Request");
                            }
                            else
                            {
                                lock (m_endpoints)
                                {
                                    if (m_endpoints.ContainsKey(ms.PortInfo.Port))
                                    {
                                        List<HttpEndpointBase> hebs = m_endpoints[ms.PortInfo.Port];
                                        // find specific endpoint
                                        var q =
                                             from r in hebs
                                             where (
                                                 from u in r.Urls
                                                 where e.ParsedHttpRequest.RequestUrl.Path.ToLower().Contains(u.ToLower())
                                                 select u
                                             ).Count() > 0
                                             select r;
                                        // if found
                                        if (q.Any())
                                        {

                                            e.ServiceEndpoint = (HttpEndpointBase)q.First();
                                            usedEndpoint = e.ServiceEndpoint;
                                        }
                                    }
                                }
                            }
                        }
                        else
                            e.ServiceEndpoint = usedEndpoint;
                        // if service is auto-run, dispatch to its own pipe
                        if (e.ServiceEndpoint != null && e.ServiceEndpoint.AutoRun)
                        {
                            lock (e.ServiceEndpoint.eventPipe)
                            {
                                e.ServiceEndpoint.eventPipe.Enqueue(e);
                            }
                        }
                        else
                        {
                            lock (eventPipe)
                            {
                                eventPipe.Enqueue(e);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Expect incoming messages from non-Auto-Run mode service and system exceptions
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public int ExpectMessages(TimeSpan duration, ref List<ManagedEvent> all)
        {
            lock (staLock)
            {
                if (all == null)
                    throw new Exception("must not input a null list");
                int maxSleep;
                maxSleep = (int)duration.TotalMilliseconds;// 1000 * (duration.Minutes * 60 + duration.Seconds);
                // use a queue type here since List<> sometimes will break the order of item inside
                Queue<ManagedEvent> q = new Queue<ManagedEvent>();
                int actualSleep = 0;

                while (actualSleep < maxSleep)
                {
                    actualSleep += 20;
                    System.Threading.Thread.Sleep(20);
                    lock (eventPipe)
                    {
                        if (eventPipe.Count > 0)
                        {
                            while (eventPipe.Count > 0)
                            {
                                ManagedEvent e = eventPipe.Dequeue();
                                q.Enqueue(e);
                            }
                            break;
                        }
                    }
                }
                all = q.ToList<ManagedEvent>();
                return all.Count;
            }
        }

        /// <summary>
        /// dispose everything!
        /// </summary>
        public void Dispose()
        {
            lock (staLock)
            {
                if (running)
                {
                    this.running = false;


                    // wait for all threads processing client request to complete
                    lock (m_listeners)
                    {
                        Dictionary<int, ManageListeningContext>.Enumerator enumer = m_listeners.GetEnumerator();
                        while (enumer.MoveNext())
                        {
                            lock (enumer.Current.Value)
                            {
                                enumer.Current.Value.Listener.Stop();
                                try
                                {
                                    enumer.Current.Value.ListeningThread.Abort();
                                }
                                catch
                                {
                                }

                                foreach (Thread rec in enumer.Current.Value.ReceiveThreads)
                                {
                                    try
                                    {
                                        rec.Abort();
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        m_listeners.Clear();
                    }

                    lock (m_endpoints)
                    {
                        Dictionary<int, List<HttpEndpointBase>>.Enumerator enumer = m_endpoints.GetEnumerator();
                        while (enumer.MoveNext())
                        {
                            foreach (HttpEndpointBase heb in enumer.Current.Value)
                            {
                                lock (heb)
                                {
                                    heb.Dispose();
                                }
                            }
                        }
                        m_endpoints.Clear();
                    }

                    lock (clientStreams)
                    {
                        foreach (ManagedStream ms in clientStreams)
                        {
                            lock (ms)
                            {
                                try
                                {
                                    ms.LocalStream.Close();
                                }
                                catch
                                {
                                }

                                try
                                {
                                    ms.LocalTcpClient.Close();
                                }
                                catch
                                {
                                }
                            }
                        }
                        clientStreams.Clear();
                    }

                    lock (eventPipe)
                    {
                        eventPipe.Clear();
                    }
                    GC.SuppressFinalize(this);
                }
            }
        }
    }

    /// <summary>
    /// base class for HTTP endpoint
    /// </summary>
    public abstract class HttpEndpointBase : IDisposable
    {
        /// <summary>
        /// all URLs this endpoint need register
        /// </summary>
        public readonly List<string> Urls = new List<string>();

        /// <summary>
        /// TCP port it uses
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// True if uses SSL
        /// </summary>
        public bool IsSSL { get; set; }

        /// <summary>
        /// register itself
        /// </summary>
        public virtual void Initialize()
        {
            HttpServer.GetInstance().RegisterHttpEndpoint(this);
        }

        internal readonly Queue<ManagedEvent> eventPipe = new Queue<ManagedEvent>();

        /// <summary>
        /// send message via service
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public virtual void SendMessage(System.Net.IPEndPoint client, byte[] msg)
        {
            HttpServer.GetInstance().SendMessage(client, msg);
        }

        /// <summary>
        /// True if it's Auto-Run mode
        /// </summary>
        public bool AutoRun { get; set; }

        /// <summary>
        /// Return messages in Auto-Run mode only
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public int ExpectMessages(TimeSpan duration, ref List<ManagedEvent> all)
        {
            if (!AutoRun)
                throw new Exception("It's not an Auto-Run service, please call HttpServer.ExpectMessages");
            if (all == null)
                throw new Exception("must not input a null list");
            int maxSleep;
            maxSleep = (int)duration.TotalMilliseconds;// 1000 * (duration.Minutes * 60 + duration.Seconds);
            // use a queue type here since List<> sometimes will break the order of item inside
            Queue<ManagedEvent> q = new Queue<ManagedEvent>();
            int actualSleep = 0;

            while (actualSleep < maxSleep)
            {
                actualSleep += 20;
                System.Threading.Thread.Sleep(20);
                lock (eventPipe)
                {
                    if (eventPipe.Count > 0)
                    {
                        while (eventPipe.Count > 0)
                        {
                            ManagedEvent e = eventPipe.Dequeue();
                            q.Enqueue(e);
                        }
                        break;
                    }
                }
            }
            all = q.ToList<ManagedEvent>();
            return all.Count;
        }

        public void Dispose()
        {
            lock (eventPipe)
            {
                eventPipe.Clear();
            }
        }
    }
}
