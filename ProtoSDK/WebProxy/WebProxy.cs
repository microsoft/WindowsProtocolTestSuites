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

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Http.WebProxy
{
    /// <summary>
    /// manage stream with access time provided
    /// </summary>
    public class ManagedProxyStream
    {
        public TcpClient SourceClient;

        public TcpClient TargetClient;
        public System.IO.Stream SourceStream;

        public System.IO.Stream TargetStream;

        public DateTime LastUsed;

        public readonly object IOLock = new object();

        public delegate void ClientCertificateChangeNotify();

        public ClientCertificateChangeNotify OnClientCertificateChanged = null;
    }

    public enum SenderType
    {
        Unknown,
        Source,
        Target
    }

    public enum ProxyEventType
    {
        Unknown,
        IncomingMessage,
    }

    public class ManagedProxyEvent
    {
        public ProxyEventType EventType;

        public SenderType Sender;

        public HttpRequest ParsedHttpRequest;

        public HttpResponse ParsedHttpResponse;

        public byte[] Data { get; set; }

        public string Text { get; set; }

        public bool Ignore { get; set; }
    }

    /// <summary>
    /// HTTPS server for supporting multiple upper level HTTP endpoints. Each HTTP endpoint should register its URL to this server to send and receive messages
    /// </summary>
    public class WebProxy : IDisposable
    {
        /// <summary>
        /// cache connected client
        /// </summary>
        readonly List<ManagedProxyStream> clientStreams = new List<ManagedProxyStream>();

        // thread related
        private bool running = false;

        private Thread listenThread;
        private readonly List<Thread> recieveThreadsList = new List<Thread>();

        // tcp listener related
        private int port;
        //private TimeSpan idleTimeout;
        private TcpListener listener;
        // ssl related
        private bool useSSL = false;
        private RemoteCertificateValidationCallback proxySideUserCertificateValidationCallback;
        private LocalCertificateSelectionCallback proxySideServerCertificateSelectionCallback;
        private LocalCertificateSelectionCallback targetSideUserCertificateValidationCallback;
        private RemoteCertificateValidationCallback targetSideServerCertificateSelectionCallback;
        private EncryptionPolicy encryptionPolicy;
        private X509Certificate2 serverCert;

        private string targetServerIP;
        private int targetServerPort;

        /// <summary>
        /// Must be called at very beginning when use HTTP, should only initialize once
        /// </summary>
        /// <param name="targetIP">Target Ip Address</param>
        /// <param name="targetPort">port number listens on</param>
        public void Initialize(string targetIP, int targetPort)
        {
            this.port = targetPort;
            this.targetServerPort = targetPort;
            this.targetServerIP = targetIP;
            this.running = true;
            this.listener = new TcpListener(IPAddress.Any, this.port);
            this.listenThread = new Thread(ListenLoop);
            this.listenThread.Start();
        }

        public void SetClientCertificate(X509Certificate2 cert)
        {
            lock (clientCertLocker)
            {
                clientCert = cert;
            }
        }

        /// <summary>
        /// must be called at very beginning when use HTTPS, should only initialize once
        /// </summary>
        /// <param name="port">port number listens on</param>
        /// <param name="idleTimeout">how much time can a client be idle before been force closed</param>
        public void Initialize(string targetIP, int targetPort, RemoteCertificateValidationCallback sourceCertValCallback, LocalCertificateSelectionCallback sourceServerCertSelCallback, LocalCertificateSelectionCallback targetClientCertValCallback, RemoteCertificateValidationCallback targetServerCertSelCallback, EncryptionPolicy encryptionPolicy, X509Certificate2 cert)
        {
            this.proxySideUserCertificateValidationCallback = sourceCertValCallback;
            this.proxySideServerCertificateSelectionCallback = sourceServerCertSelCallback;
            targetSideUserCertificateValidationCallback = targetClientCertValCallback;
            targetSideServerCertificateSelectionCallback = targetServerCertSelCallback;
            this.encryptionPolicy = encryptionPolicy;
            this.serverCert = cert;
            this.useSSL = true;
            Initialize(targetIP, targetPort);
        }

        /// <summary>
        /// 1 thread listen on TransportStack
        /// </summary>
        private void ListenLoop()
        {
            listener.Start();

            Trace.WriteLine("Server listener started");

            while (this.running)
            {
                TcpClient client;
                try
                {
                    client = listener.AcceptTcpClient();
                }
                catch
                {
                    if (this.running)
                        Trace.TraceError("something broke http server");
                    return;
                }

                Trace.WriteLine("Client {0} connected",
                    (client.Client.RemoteEndPoint as IPEndPoint).ToString());
                client.ReceiveBufferSize = 20000;
                Thread task = new Thread(new ParameterizedThreadStart(ReceiveLoop));
                task.Start(client);

                recieveThreadsList.Add(task);

            }
        }


        public delegate string OnInputHostName();

        public OnInputHostName HostNameForSSLToTargetServerHandler = null;

        SslStream connectToTargetSSLPort(TcpClient targetServerCon)
        {
            SslStream ssl2 = new SslStream(targetServerCon.GetStream(), true, null, this.targetSideUserCertificateValidationCallback, this.encryptionPolicy);
            string hostname = "";
            if (HostNameForSSLToTargetServerHandler != null)
                hostname = HostNameForSSLToTargetServerHandler();
            lock (clientCertLocker)
            {
                ssl2.AuthenticateAsClient(hostname, new X509CertificateCollection(new X509Certificate2[] { clientCert }), System.Security.Authentication.SslProtocols.Default, false);
            }
            return ssl2;
        }

        object clientCertLocker = new object();
        X509Certificate2 clientCert = null;

        private void ReceiveLoop(object o)
        {
            TimeSpan idleTime = TimeSpan.FromMilliseconds(0);
            TimeSpan sleepTime = TimeSpan.FromMilliseconds(200);
            Stream s;
            Stream targetS;
            System.Threading.Thread targetSideThread = new Thread(new ParameterizedThreadStart(targetSendBackLoop));
            X509Certificate2 currentTargetClientCert = null;
            lock (clientCertLocker)
            {
                currentTargetClientCert = clientCert;
            }
            using (TcpClient targetServerCon = new TcpClient(targetServerIP, targetServerPort))
            {
                using (var c = o as TcpClient)
                {
                    if (this.useSSL)
                    {

                        SslStream ssl2 = connectToTargetSSLPort(targetServerCon);
                        targetS = ssl2;


                        SslStream ssl = new SslStream(c.GetStream(), false, this.proxySideUserCertificateValidationCallback, this.proxySideServerCertificateSelectionCallback, this.encryptionPolicy);
                        ssl.AuthenticateAsServer(this.serverCert, false, System.Security.Authentication.SslProtocols.Tls, false);
                        s = ssl;

                    }
                    else
                    {
                        s = c.GetStream();
                        targetS = targetServerCon.GetStream();
                    }
                    // cache stream
                    var ms = new ManagedProxyStream();
                    this.clientStreams.Add(ms);

                    ms.SourceClient = c;
                    ms.TargetClient = targetServerCon;
                    ms.SourceStream = s;
                    ms.TargetStream = targetS;
                    if (SSLConnectedCallback != null)
                        SSLConnectedCallback(ms);
                    targetSideThread.Start(ms);
                    using (s)
                    {
                        while (this.running)
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

                                    lock (ms.IOLock)
                                    {
                                        do
                                        {
                                            var bytes = s.Read(buf, 0, buf.Length);
                                            t.Write(buf, 0, bytes);
                                            System.Threading.Thread.Sleep(50);
                                        } while (c.Available > 0);

                                        ms.LastUsed = DateTime.Now;
                                    }

                                    data = t.ToArray();
                                }

                                // pack as ManagedEvent
                                var e = new ManagedProxyEvent();
                                e.Data = data;
                                e.Sender = SenderType.Source;
                                e.EventType = ProxyEventType.IncomingMessage;

                                lock (clientCertLocker)
                                {
                                    if (0 != string.Compare(clientCert.Thumbprint.ToLower(), currentTargetClientCert.Thumbprint.ToLower()))
                                    {
                                        currentTargetClientCert = clientCert;
                                        ms.TargetClient.Close();
                                        TcpClient targetCon = new TcpClient(targetServerIP, targetServerPort);
                                        SslStream ssl2 = connectToTargetSSLPort(targetCon);
                                        targetS = ssl2;
                                        ms.TargetClient = targetCon;
                                        ms.TargetStream = ssl2;
                                        targetSideThread = new Thread(new ParameterizedThreadStart(targetSendBackLoop));
                                        if (SSLConnectedCallback != null)
                                            SSLConnectedCallback(ms);
                                        targetSideThread.Start(ms);
                                    }
                                }

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
                                // if not http message
                                catch
                                {
                                    e.EventType = ProxyEventType.Unknown;
                                }
                                finally
                                {
                                    byte[] tosend = e.Data;
                                    if (ReceiveMessageFromSourceCallback != null)
                                    {
                                        tosend = ReceiveMessageFromSourceCallback(e);
                                    }
                                    if (tosend != null)
                                    {
                                        if (!ms.TargetClient.Connected)
                                        {
                                            ms.TargetClient = new TcpClient(targetServerIP, targetServerPort);
                                            if (useSSL)
                                                ms.TargetStream = connectToTargetSSLPort(ms.TargetClient);
                                            targetSideThread = new Thread(new ParameterizedThreadStart(targetSendBackLoop));
                                            targetSideThread.Start(ms);
                                        }
                                        ms.TargetStream.Write(tosend, 0, tosend.Length);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void targetSendBackLoop(object o)
        {
            ManagedProxyStream stream = o as ManagedProxyStream;
            TimeSpan sleepTime = TimeSpan.FromMilliseconds(200);
            try
            {
                while (this.running)
                {
                    // if no data available, wait for a while
                    if (stream.TargetClient.Connected && stream.TargetClient.Available == 0)
                    {
                        Thread.Sleep(sleepTime);
                    }
                    // if data coming
                    else
                    {
                        byte[] data;

                        // receive data
                        using (var t = new MemoryStream())
                        {
                            var buf = new byte[stream.TargetClient.ReceiveBufferSize];

                            lock (stream.IOLock)
                            {
                                do
                                {
                                    var bytes = stream.TargetStream.Read(buf, 0, buf.Length);
                                    t.Write(buf, 0, bytes);

                                } while (stream.TargetClient.Available > 0);

                                stream.LastUsed = DateTime.Now;
                            }

                            data = t.ToArray();
                        }

                        // pack as ManagedEvent
                        var e = new ManagedProxyEvent();
                        e.Data = data;
                        e.Sender = SenderType.Target;
                        e.EventType = ProxyEventType.IncomingMessage;

                        //parse the endpoint
                        string str = Encoding.UTF8.GetString(data);
                        e.Text = str;
                        // try to parse as http
                        try
                        {
                            HttpResponse res = new HttpResponse();
                            res.Parse(e.Text);
                            e.ParsedHttpResponse = res;
                        }
                        // if not http message
                        catch
                        {
                            e.EventType = ProxyEventType.Unknown;
                        }
                        finally
                        {
                            byte[] tosend = e.Data;
                            if (ReceiveMessageFromTargetCallback != null)
                            {
                                byte[] replacedResponse = ReceiveMessageFromTargetCallback(e);
                                if (replacedResponse != null)
                                    tosend = replacedResponse;
                            }
                            if (tosend == null)
                                // OACR do not allow throwing exception in a final clause
                                Trace.Fail("tosend is null");

                            lock (stream)
                            {
                                stream.SourceStream.Write(tosend, 0, tosend.Length);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public delegate byte[] OnReceiveMessage(ManagedProxyEvent e);

        public delegate void OnSSLConnected(ManagedProxyStream s);

        public OnSSLConnected SSLConnectedCallback = null;

        public OnReceiveMessage ReceiveMessageFromSourceCallback = null;

        public OnReceiveMessage ReceiveMessageFromTargetCallback = null;

        /// <summary>
        /// dispose everything!
        /// </summary>
        public void Dispose()
        {
            if (running)
            {
                this.running = false;

                // wait for all threads processing client request to complete
                for (var t = recieveThreadsList.Where(n => n.ThreadState == System.Threading.ThreadState.Running); t.Any(); )
                {
                    try
                    {
                        t.First().Abort();
                    }
                    catch
                    {
                    }

                }
                // abort the listening thread
                this.listener.Stop();
                this.listenThread.Abort();

                GC.SuppressFinalize(this);
            }
        }
    }
}
