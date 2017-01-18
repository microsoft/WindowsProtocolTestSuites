// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Http;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    public class RequestHandler
    {
        System.IO.Pipes.NamedPipeClientStream pipe = new System.IO.Pipes.NamedPipeClientStream(Constraints.PipeName);

        public void HandleRequest(ManagedEvent e)
        {
            if (e == null || e.ParsedHttpRequest == null)
                throw new Exception("cannot be null");
            string path = e.ParsedHttpRequest.RequestUrl.Path.ToLower();
            if (path == Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP.Constraints.GetSTSConfigurationUrl.ToLower())
            {
                if (e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.GET)
                {
                    if (!System.IO.File.Exists(getSTSConfigurationResponseFile))
                        throw new Exception("GetSTSConfiguration Response File does not exists");
                    using (System.IO.FileStream fs = new System.IO.FileStream(getSTSConfigurationResponseFile, System.IO.FileMode.Open))
                    {
                        byte[] data = new byte[fs.Length];
                        fs.Read(data, 0, data.Length);
                        StsService.SendMessage(e.Sender, data);
                    }
                    Logger.Log("Handled GetSTSConfiguration request");
                    return;
                }

            }
            else if (path.Contains(Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP.Constraints.StoreUrl.ToLower()))
            {
                if (e.ParsedHttpRequest.Method == HttpRequest.HttpMethod.GET)
                {
                    if (path.Length == Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP.Constraints.StoreUrl.Length)
                    {
                        if (!System.IO.File.Exists(getStoreVersionResponseFile))
                            throw new Exception("GetStoreVersion Response File does not exists");
                        using (System.IO.FileStream fs = new System.IO.FileStream(getStoreVersionResponseFile, System.IO.FileMode.Open))
                        {
                            byte[] data = new byte[fs.Length];
                            fs.Read(data, 0, data.Length);
                            StsService.SendMessage(e.Sender, data);
                        }
                        Logger.Log("Handled GetStoreVersion request");
                        return;
                    }
                    else
                    {
                        string filename = getStoreEntryResponseFile.Replace("*", path.Substring(Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP.Constraints.StoreUrl.Length + 1));
                        if (System.IO.File.Exists(filename))
                        {
                            HttpResponse res = new HttpResponse(System.Net.HttpStatusCode.OK);
                            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open))
                            {
                                byte[] data = new byte[fs.Length];
                                fs.Read(data, 0, data.Length);
                                res.Body = Encoding.UTF8.GetString(data);
                            }
                            res.SetHeaderField(System.Net.HttpResponseHeader.ContentLength, res.Body.Length.ToString());
                            StsService.SendMessage(e.Sender, Encoding.UTF8.GetBytes(res.ToString()));
                            Logger.Log("Handled GetStoreEntry request");
                            return;
                        }
                    }
                }

            }
            Logger.Log("Message not in scope with Url: " + path + ". Will dispatch to pipe");
            lock (iolocker)
            {
                if (tsConnected)
                {
                    byte[] buf = new byte[2000000];
                    byte[] ret = null;
                    try
                    {
                        pipe.Write(e.Data, 0, e.Data.Length);
                        int len = pipe.Read(buf, 0, buf.Length);
                        ret = new byte[len];
                        Array.Copy(buf, 0, ret, 0, len);
                    }
                    catch (Exception err)
                    {
                        Logger.Log("Failed to write/read the connected pipe, reason:" + (err.Message == null ? "" : err.Message));
                        Logger.Log("Will reconnect to pipe");
                        System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(connectThread));
                    }
                    StsService.SendMessage(e.Sender, ret);
                }
            }
        }

        public void Initialize()
        {
#pragma warning disable 618
            // Error CS0618: System.Configuration.ConfigurationSettings.AppSettings is obsolete.
            getSTSConfigurationResponseFile = System.Configuration.ConfigurationSettings.AppSettings["GetSTSConfigurationResponseFile"];
            getStoreVersionResponseFile = System.Configuration.ConfigurationSettings.AppSettings["GetStoreVersionResponseFile"];
            getStoreEntryResponseFile = System.Configuration.ConfigurationSettings.AppSettings["GetStoreEntryResponseFile"];
#pragma warning restore 618            
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(connectThread));
        }


        void connectThread(object o)
        {
            while (true)
            {
                try
                {
                    pipe = new System.IO.Pipes.NamedPipeClientStream(Constraints.PipeName);
                    // by default, timeout for both read and write set to 5seconds
                    pipe.WriteTimeout = 5000;
                    pipe.ReadTimeout = 5000;
                    // there is a .net bug, this call will use up all CPU resource so it should only has a small timeout then sleep a bigger interval
                    pipe.Connect(100);
                    lock (iolocker)
                    {
                        tsConnected = true;
                        return;
                    }
                }
                catch
                {
                }
                System.Threading.Thread.Sleep(1000);
            }
        }


        object iolocker = new object();

        bool tsConnected = false;

        public string getSTSConfigurationResponseFile;

        public string getStoreVersionResponseFile;

        public string getStoreEntryResponseFile;

        public SyncLogger Logger { set; get; }

        public StsService StsService { set; get; }
    }
}
