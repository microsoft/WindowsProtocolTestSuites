// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    public class SyncLogger
    {

        System.IO.StreamWriter file = null;

        object locker = new object();

        public void Initialize(string path, bool forceOverride = false)
        {
            lock (locker)
            {
                if (file != null)
                    throw new Exception("already initialized");

                if (System.IO.File.Exists(path))
                {
                    if (!forceOverride)
                        throw new Exception("File " + file + " already exists");
                    else
                    {
                        System.IO.File.Delete(path);
                    }
                }

                file = new System.IO.StreamWriter(path);
            }
        }

        public void Log(string text)
        {
            lock (locker)
            {
                string time = DateTime.Now.ToString();
                file.WriteLine(time + " : " + text);
                Console.WriteLine(time + " : " + text);
                file.WriteLine();
                Console.WriteLine();
            }
        }
    }

    public class Processor
    {
        public Processor()
        {

        }

        readonly RequestHandler handler = new RequestHandler();

        readonly SyncLogger logger = new SyncLogger();

        const string logFolder = @"Logs\";

        const string mainLogFile = @"Logs\log.txt";

        readonly HttpServer httpServer = new HttpServer();

        readonly Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP.StsService stsService = new TestTools.StackSdk.Identity.ADFSPIP.StsService();

        bool remoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        X509Certificate localCertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return new X509Certificate("c:\\contosoadfs.pfx", "123");
        }
        public void Initialize()
        {
            if (!System.IO.Directory.Exists(logFolder))
                System.IO.Directory.CreateDirectory(logFolder);
            logger.Initialize(mainLogFile, true);

            stsService.Initialize(httpServer);
            httpServer.Initialize(443, new TimeSpan(100, 0, 0), new System.Net.Security.RemoteCertificateValidationCallback(remoteCertificateValidationCallback), new System.Net.Security.LocalCertificateSelectionCallback(localCertificateSelectionCallback), EncryptionPolicy.AllowNoEncryption, new X509Certificate("c:\\contosoadfs.pfx", "123"));
            handler.Logger = logger;
            handler.StsService = stsService;
            handler.Initialize();
            recvThread = new System.Threading.Thread(new System.Threading.ThreadStart(receiveLoop));
            recvThread.Start();
            logger.Log("Processor start working");
        }

        System.Threading.Thread recvThread = null;

        void receiveLoop()
        {
            // as a dedicated process, here we can hardcode 100 hours, no matter what kind of client implementation is and whether test suite is running
            TimeSpan maxSleep = new TimeSpan(100, 0, 0);

            ManagedEvent cached = null;

            while (true)
            {
                List<ManagedEvent> events = new List<ManagedEvent>();

                httpServer.ExpectMessages(maxSleep, ref events);

                events = HttpServerUtility.ReassembleRequests(events);

                foreach (ManagedEvent me in events)
                {
                    ManagedEvent toprocess = null;
                    try
                    {
                        switch (me.EventType)
                        {
                            case EventType.IncomingRequest:
                                {
                                    if (me.ParsedHttpRequest != null)
                                    {
                                        string expect = me.ParsedHttpRequest.GetHeaderFieldValue(System.Net.HttpRequestHeader.Expect);
                                        if (expect != null && expect.ToLower() == "100-continue")
                                        {
                                            string contentLength = me.ParsedHttpRequest.GetHeaderFieldValue(System.Net.HttpRequestHeader.ContentLength);
                                            if (contentLength != null && int.Parse(contentLength) > me.ParsedHttpRequest.Body.Length)
                                            {
                                                cached = me;
                                                continue;
                                            }
                                        }
                                        toprocess = me;
                                    }
                                }
                                break;
                            case EventType.UnknownRequest:
                                {
                                    if (cached != null)
                                    {
                                        if (cached.ParsedHttpRequest.Body == null)
                                            cached.ParsedHttpRequest.Body = "";
                                        cached.ParsedHttpRequest.Body += me.Text;
                                        if (int.Parse(cached.ParsedHttpRequest.GetHeaderFieldValue(System.Net.HttpRequestHeader.ContentLength)) > cached.ParsedHttpRequest.Body.Length)
                                            continue;
                                        else
                                            toprocess = cached;
                                    }
                                    else throw new Exception("Unknown message received");
                                }
                                break;
                            default:
                                {
                                    throw new Exception("Unknown message received");
                                }
                        }

                        handler.HandleRequest(toprocess);

                    }
                    catch (Exception e)
                    {
                        saveUnprocessedRequest(me.Data, "Failed to process request" + (e.Message == null ? "" : " due to reason: " + e.Message));
                    }
                }
            }
        }

        void saveUnprocessedRequest(byte[] data, string reason)
        {
            if (data == null)
                throw new Exception("data is null");
            using (System.IO.FileStream sw = new System.IO.FileStream(logFolder + getTimeBasedFilename(), System.IO.FileMode.CreateNew))
            {
                sw.Write(data, 0, data.Length);
                Console.WriteLine(reason + ": ");
                Console.WriteLine(Encoding.UTF8.GetString(data));
            }
        }

        string getTimeBasedFilename()
        {
            return DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + "-" + DateTime.Now.Millisecond + ".txt";
        }
    }
}
