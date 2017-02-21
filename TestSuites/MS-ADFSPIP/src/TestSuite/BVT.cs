// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Http;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Web.Script.Serialization;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Http.WebProxy;
using System.Diagnostics;


namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    public enum JWTSignAlogrithm { HS256, RS256 }
    public class JWTGenerator
    {
        public JWTSignAlogrithm Algorithm { get; set; }
        public JavaScriptSerializer jss = new JavaScriptSerializer();

        public string GenerateWithRSA256(string header, string body, X509Certificate2 cert)
        {
            if (!cert.HasPrivateKey)
                throw new Exception("Must have private key");
            string encodedHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes(header));
            string encodedBody = Convert.ToBase64String(Encoding.ASCII.GetBytes(body));
            while (encodedBody.EndsWith("="))
                encodedBody = encodedBody.Remove(encodedBody.Length - 1);
            string combined = encodedHeader + "." + encodedBody;
            combined = combined.Replace("/", "_");
            combined = combined.Replace("+", "-");
            byte[] tmp = Encoding.ASCII.GetBytes(combined);
            System.Security.Cryptography.RSACryptoServiceProvider provider = cert.PrivateKey as System.Security.Cryptography.RSACryptoServiceProvider;

            byte[] final = provider.SignData(tmp, new System.Security.Cryptography.SHA1CryptoServiceProvider());
            string sign = Convert.ToBase64String(final);
            while (sign.EndsWith("="))
                sign = sign.Remove(sign.Length - 1);
            sign = sign.Replace("/", "_");
            sign = sign.Replace("+", "-");
            string ret = combined + "." + sign;
            return ret;
        }
    }
    [TestClass]
    public class BVT : BaseTestSuite
    {
        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            BaseTestSuite.BaseInitialize(context);

        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            BaseTestSuite.BaseCleanup();
        }
        #endregion

        protected override void TestInitialize()
        {
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            foreach (System.Threading.Thread t in managedThreads)
            {
                try
                {
                    t.Abort();
                }
                catch
                {
                }
            }
            base.TestCleanup();
        }

        System.Threading.ManualResetEvent proxyInstalled = new System.Threading.ManualResetEvent(false);

        void installProxy(object o)
        {
            try
            {
                proxyInstalled.Reset();
                sutAdapter.TriggerCleanCertificate();
                sutAdapter.TriggerProxyInstall();
                Debug.WriteLine("TriggerProxyInstall Finished");
                proxyInstalled.Set();
            }
            catch
            {
                BaseTestSite.Assert.Fail("Failed to trigger proxy to deploy");
            }
        }

        void AsyncInstallProxy()
        {
            lock (managedThreads)
            {
                System.Threading.Thread t = new System.Threading.Thread(installProxy);
                t.Start();
                managedThreads.Add(t);
            }
            
            if (EnvironmentConfig.IsWindows)
                // windows may need 15seconds to install role before start deployment
                System.Threading.Thread.Sleep(20000);
        }

        void UninstallProxy()
        {
            sutAdapter.TriggerProxyUninstall();
            if (EnvironmentConfig.IsWindows)
                // windows may need 3mins to restart
                System.Threading.Thread.Sleep(120000);
        }

        [TestMethod]
        [DeployScenario]
        [TestCategory("BVT")]
        [TestCategory("Disabled")]
        [Ignore]
        public void Deployment_InitialStore_Success()
        {
            server.ResetFilter(new List<string>() { Constraints.RenewTrustUrl, Constraints.GetSTSConfigurationUrl, Constraints.StoreUrl });
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Start uninstall proxy");
#if !NODEPLOY
            UninstallProxy();
#endif
            Debug.WriteLine("UninstallProxy");

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Start install proxy");
#if !NODEPLOY
            AsyncInstallProxy();
#endif
            Debug.WriteLine("AsyncInstallProxy");


            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Start expecting EstablishTrust message");
            server.EstablishTrust();
            Debug.WriteLine("EstablishTrust");
            server.ResetFilter(new List<string>() { Constraints.RenewTrustUrl });

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Start expecting GetSTSConfiguration message");
            server.GetSTSConfiguration();
            Debug.WriteLine("GetSTSConfiguration");

            if (EnvironmentConfig.IsWindows)
            {
                // windows proxy send 3 STS configuration get during deployment
                server.GetSTSConfiguration();
                server.GetSTSConfiguration();
            }

            server.GetStoreVersion();
            Debug.WriteLine("GetStoreVersion");

            server.GetProxyTrust();
            Debug.WriteLine("GetProxyTrust");

            server.PostProxyTrust();
            Debug.WriteLine("PostProxyTrust");

            server.GetFederationMetadata();
            Debug.WriteLine("GetFederationMetadata");

            server.GetStoreVersion();
            Debug.WriteLine("GetStoreVersion");

            if (EnvironmentConfig.IsWindows)
                server.GetStoreVersion();

            server.GetProxyGlobalConfiguration();
            Debug.WriteLine("GetProxyGlobalConfiguration");

            server.PostProxyGlobalConfiguration();
            Debug.WriteLine("PostProxyGlobalConfiguration");

            proxyInstalled.WaitOne();
            Debug.WriteLine("Test Finished");
        }

        List<Microsoft.Protocols.TestTools.StackSdk.Networking.Http.ManagedEvent> unhandled = new List<TestTools.StackSdk.Networking.Http.ManagedEvent>();

        System.Threading.AutoResetEvent signal = new System.Threading.AutoResetEvent(true);
        // Error CS0219: Warning as Error: The variable is assigned but its value is never used.
        // System.Threading.Thread autoThread = null;

        bool sourceCertValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        bool targetCertValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        X509Certificate sourceServerCertSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return new X509Certificate(EnvironmentConfig.TLSServerCertificatePath, EnvironmentConfig.TLSServerCertificatePassword);
        }

        string returnHostName()
        {
            return EnvironmentConfig.ADFSFamrDNSName;
        }

        HttpRequest establishTrustRequest = null;

        X509Certificate2 proxyCert = null;

        void retriveProxyCert()
        {
            if (proxyCert == null)
            {
                try
                {
#if !NODEPLOY
                    if (System.IO.File.Exists(EnvironmentConfig.ProxyTrustCertificatePath))
                        System.IO.File.Delete(EnvironmentConfig.ProxyTrustCertificatePath);

                    sutAdapter.TriggerExportCertificate();
#endif
                    proxyCert = new X509Certificate2(EnvironmentConfig.ProxyTrustCertificatePath, EnvironmentConfig.ProxyTrustCertificatePassword);
                    System.Security.Cryptography.X509Certificates.X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadWrite);
                    foreach (X509Certificate2 cer in store.Certificates)
                    {
                        if (cer.Thumbprint == proxyCert.Thumbprint)
                            return;
                    }
                    store.Add(proxyCert);
                    store.Close();
                }
                catch
                {
                }
            }
        }

        bool requestIs100Continue = false;
        byte[] OnReceiveSourceMessage(Microsoft.Protocols.TestTools.StackSdk.Networking.Http.WebProxy.ManagedProxyEvent e)
        {
            while (true)
            {
                lock (resourceLocker)
                {
                    if (first == null && second == null)
                    {
                        first = e;
                        break;
                    }
                }
                System.Threading.Thread.Sleep(100);
            }
            while (true)
            {
                lock (resourceLocker)
                {
                    if (first == null && second != null)
                    {
                        e = second;
                        second = null;
                        break;
                    }
                }
                System.Threading.Thread.Sleep(100);
            }
            if (e.Ignore)
                return null;
            return e.Data;
        }

        void authAsClient(IAsyncResult ar)
        {
            ar.AsyncWaitHandle.WaitOne();

            (ar.AsyncState as SslStream).EndAuthenticateAsClient(ar);

            authedSign.Set();
        }

        System.Threading.AutoResetEvent authedSign = new System.Threading.AutoResetEvent(false);

        byte[] OnReceiveTargetMessage(Microsoft.Protocols.TestTools.StackSdk.Networking.Http.WebProxy.ManagedProxyEvent e)
        {
            try
            {
                HttpResponse res = new HttpResponse();
                res.Parse(e.Text);
                while (true)
                {
                    lock (resourceLocker)
                    {
                        if (first == null && second == null)
                        {
                            first = e;
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                }
                while (true)
                {
                    lock (resourceLocker)
                    {
                        if (first == null && second != null)
                        {
                            e = second;
                            second = null;
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                }
                if (e.Ignore)
                    return null;

            }
            catch
            {
            }
            //}
            return e.Data;
        }

        void AsyncInstallAndPublishApp1AndAccess(object delayedSeconds)
        {
#if !NODEPLOY
            installProxy(null);
#endif
            string err = null;
            sutAdapter.TriggerPublishApplication(out err);
            webclientThread(null);
        }

        void AsyncPublishApp2AndAccess(int delayedSeconds)
        {
            if (delayedSeconds > 0)
                System.Threading.Thread.Sleep(delayedSeconds * 1000);
        }

        bool authTokenPresent = false;
        bool accessToWebApp = false;
        bool backendTlsCaptured = false;
        System.Threading.ManualResetEvent preauth_fed_asProxy_success_signal = new System.Threading.ManualResetEvent(false);

        void preauth_fed_workplacejoined_asProxy_success(object px)
        {
            preauth_fed_asProxy_success_signal.Reset();
            WebProxy proxy = px as WebProxy;

            authTokenPresent = false;
            accessToWebApp = false;
            backendTlsCaptured = false;
            lock (managedThreads)
            {
                System.Threading.Thread t = new System.Threading.Thread(AsyncInstallAndPublishApp1AndAccess);
                t.Start();
                managedThreads.Add(t);
            }

            DateTime start = DateTime.Now;
            int timeout = 10;
            while ((DateTime.Now - start).TotalMinutes < timeout)
            {
                lock (resourceLocker)
                {
                    if (first != null)
                    {
                        first.Text = Encoding.UTF8.GetString(first.Data);
                        if (first.ParsedHttpRequest != null)
                        {
                            if (first.ParsedHttpRequest.Method == HttpRequest.HttpMethod.POST)
                            {
                            }
                            if (first.ParsedHttpRequest.RequestUrl.Path.ToLower() == Constraints.EstablishTrustUrl.ToLower())
                            {
                                if (proxyCert == null)
                                {
                                    if (first.ParsedHttpRequest.Body == null)
                                    {
                                        requestIs100Continue = true;
                                    }
                                    establishTrustRequest = first.ParsedHttpRequest;
                                    if (!requestIs100Continue)
                                    {
                                        //already full request
                                        retriveProxyCert();
                                        proxy.SetClientCertificate(proxyCert);
                                    }
                                }
                            }
                            else if (first.ParsedHttpRequest.RequestUrl.Path.ToLower() == Constraints.RenewTrustUrl.ToLower())
                            {
                                first.Ignore = true;
                            }
                            else if (first.ParsedHttpRequest.RequestUrl.Path.ToLower().Contains(Constraints.BackEndProxyTLSUrl.ToLower()))
                            {
                                backendTlsCaptured = true;

                            }
                            else if (first.ParsedHttpRequest.RequestUrl.Path.ToLower().Contains(EnvironmentConfig.App1Name.ToLower()))
                            {
                                accessToWebApp = true;
                                break;
                            }
                        }
                        else if (first.Sender == SenderType.Target)
                        {
                            if (first.ParsedHttpResponse == null)
                            {
                                first.ParsedHttpResponse = new HttpResponse();
                                first.ParsedHttpResponse.Parse(Encoding.UTF8.GetString(first.Data));
                            }
                            HttpResponse res = first.ParsedHttpResponse;
                            
                            if (res.StatusCode == System.Net.HttpStatusCode.TemporaryRedirect)
                            {
                                string loc = res.GetHeaderFieldValue(System.Net.HttpResponseHeader.Location);
                                BaseTestSite.Assert.IsNotNull(loc, "location cannot be null for HTTP 307");
                                Url u = new Url(loc);
                                List<KeyValuePair<string, string>> pairs = u.GetQueryPairs();

                                foreach (KeyValuePair<string, string> p in pairs)
                                {
                                    if (p.Key.ToLower() == "authtoken")
                                        authTokenPresent = true;
                                }

                            }
                        }
                        else if (proxyCert == null && requestIs100Continue && first.ParsedHttpRequest == null)
                        {
                            establishTrustRequest.Body += first.Text;
                            retriveProxyCert();
                            proxy.SetClientCertificate(proxyCert);
                        }
                        second = first;
                        first = null;
                    }


                }
                System.Threading.Thread.Sleep(150);
            }

            preauth_fed_asProxy_success_signal.Set();
        }

        byte[] receiveData(System.Net.Sockets.TcpClient c, System.Net.Security.SslStream ssl)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                System.Threading.Thread.Sleep(2000);
                while (c.Available > 0)
                {
                    byte[] buf = new byte[10000];
                    int len = ssl.Read(buf, 0, buf.Length);
                    ms.Write(buf, 0, len);
                }
                return ms.ToArray();
            }
        }

        void webclientThread(object o)
        {
            //webclientSignal.WaitOne();
            //#if !NODEPLOY
            System.Threading.Thread.Sleep(30000);

            //   return;
            //#endif
            try
            {
                string appHost = new Uri(EnvironmentConfig.App1Url).Host;
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(EnvironmentConfig.App1Url);
                req.Method = "GET";
                
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                byte[] resRaw = null;
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (res.ContentLength > 0)
                    {
                        System.IO.Stream io = res.GetResponseStream();
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            int data = -1;
                            do
                            {
                                data = io.ReadByte();
                                if (data == -1)
                                    break;
                                ms.WriteByte((byte)data);
                            } while (data != -1);
                            resRaw = ms.ToArray();
                        }
                    }
                }
                else
                    throw new Exception();
                
                if (resRaw != null)
                    Encoding.UTF8.GetString(resRaw);
                
                {
                    req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(res.ResponseUri);
                    req.Method = "POST";
                    req.AllowAutoRedirect = false;
                    req.Referer = res.ResponseUri.ToString();
                    req.Accept = "text/html, application/xhtml+xml, */*";
                    
                    req.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
                    req.ContentType = "application/x-www-form-urlencoded";
                    string auth = "UserName=" + System.Web.HttpUtility.UrlEncode(EnvironmentConfig.AppUser) + "&Password=" + System.Web.HttpUtility.UrlEncode(EnvironmentConfig.AppUserPassword) + "&AuthMethod=FormsAuthentication";
                    req.GetRequestStream().Write(Encoding.UTF8.GetBytes(auth), 0, auth.Length);
                    req.GetRequestStream().Close();
                    res = (System.Net.HttpWebResponse)req.GetResponse();
                }

                resRaw = null;
                if (res.ContentLength > 0)
                {
                    System.IO.Stream io = res.GetResponseStream();
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        int data = -1;
                        do
                        {
                            data = io.ReadByte();
                            if (data == -1)
                                break;
                            ms.WriteByte((byte)data);
                        } while (data != -1);
                        resRaw = ms.ToArray();
                    }
                }

                if (resRaw != null)
                    Encoding.UTF8.GetString(resRaw);
                
                if (res.StatusCode == System.Net.HttpStatusCode.Found)
                {
                    using (System.Net.Sockets.TcpClient tc = new System.Net.Sockets.TcpClient(EnvironmentConfig.ADFSFamrDNSName, Constraints.HTTPSServiceDefaultPort))
                    {
                        using (System.Net.Security.SslStream ssl = new SslStream(tc.GetStream(), true))
                        {
                            ssl.AuthenticateAsClient(EnvironmentConfig.ADFSFamrDNSName, null, System.Security.Authentication.SslProtocols.Default, false);
                            HttpRequest hr = new HttpRequest(HttpRequest.HttpMethod.GET, res.ResponseUri.PathAndQuery);
                            hr.SetHeaderField(System.Net.HttpRequestHeader.Accept, "text/html, application/xhtml+xml, */*");
                            hr.SetHeaderField(System.Net.HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko");
                            hr.SetHeaderField(System.Net.HttpRequestHeader.Referer, res.GetResponseHeader("location"));

                            string cookStr = res.GetResponseHeader("Set-Cookie");
                            string[] cookParts = cookStr.Split(new string[] { "=", ";" }, StringSplitOptions.RemoveEmptyEntries);
                            hr.SetHeaderField(System.Net.HttpRequestHeader.Cookie, cookParts[0] + "=" + cookParts[1] + "=");
                            hr.SetHeaderField(System.Net.HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                            hr.SetHeaderField(System.Net.HttpRequestHeader.Host, EnvironmentConfig.ADFSFamrDNSName);
                            ssl.Write(Encoding.UTF8.GetBytes(hr.ToString()));

                            System.Threading.Thread.Sleep(2000);
                            byte[] temp = null;
                            if (tc.Available > 0)
                            {
                                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                                {
                                    while (tc.Available > 0)
                                    {
                                        byte[] buf = new byte[8192];
                                        int len = ssl.Read(buf, 0, buf.Length);
                                        if (len == 0)
                                            break;
                                        ms.Write(buf, 0, len);
                                    }

                                    temp = ms.ToArray();
                                }
                            }
                            if (temp != null)
                            {
                                string txt = Encoding.UTF8.GetString(temp);
                                HttpResponse hres = new HttpResponse();
                                hres.Parse(txt);
                                if (hres.StatusCode != System.Net.HttpStatusCode.TemporaryRedirect)
                                    throw new Exception();
                                using (System.Net.Sockets.TcpClient apTc = new System.Net.Sockets.TcpClient(appHost, Constraints.HTTPSServiceDefaultPort))
                                {
                                    using (System.Net.Security.SslStream apSsl = new SslStream(apTc.GetStream(), true))
                                    {
                                        try
                                        {
                                            apSsl.AuthenticateAsClient(appHost, null, System.Security.Authentication.SslProtocols.Default, false);
                                            string url = new Uri(hres.GetHeaderFieldValue(System.Net.HttpResponseHeader.Location)).PathAndQuery;
                                            HttpRequest hreq = new HttpRequest(HttpRequest.HttpMethod.GET, url);
                                            hreq.SetHeaderField(System.Net.HttpRequestHeader.Referer, res.GetResponseHeader("location"));
                                            
                                            hreq.SetHeaderField(System.Net.HttpRequestHeader.Host, appHost);
                                            apSsl.Write(Encoding.UTF8.GetBytes(hreq.ToString()));

                                            System.Threading.Thread.Sleep(2000);

                                            temp = null;
                                            if (apTc.Available > 0)
                                            {
                                                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                                                {
                                                    while (apTc.Available > 0)
                                                    {
                                                        byte[] buf = new byte[8192];
                                                        int len = apSsl.Read(buf, 0, buf.Length);
                                                        if (len == 0)
                                                            break;
                                                        ms.Write(buf, 0, len);
                                                    }

                                                    temp = ms.ToArray();
                                                }
                                            }
                                        }
                                        catch
                                        {
                                        }
                                        if (temp != null)
                                        {
                                            txt = Encoding.UTF8.GetString(temp);
                                        }
                                    }

                                }
                            }
                        }
                    }
                    req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(res.GetResponseHeader("Location"));
                    req.Method = "GET";
                    req.AllowAutoRedirect = false;
                    string cok = res.GetResponseHeader("Set-Cookie");
                    string[] cokParts = cok.Split(new string[] { "=", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    
                    req.Accept = "text/html, application/xhtml+xml, */*";
                    req.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
                    req.CookieContainer = new System.Net.CookieContainer();
                    System.Net.Cookie cook = new System.Net.Cookie(cokParts[0], cokParts[1], cokParts[3], req.RequestUri.Host);
                    cook.HttpOnly = true;
                    cook.Secure = true;
                    cook.Domain = req.RequestUri.Host;
                    cook.Discard = true;
                    req.CookieContainer.Add(cook);
                    req.Referer = res.GetResponseHeader("Location");
                    req.Host = req.RequestUri.Host;
                    
                    res = (System.Net.HttpWebResponse)req.GetResponse();
                }
                else
                    throw new Exception();

                resRaw = null;
                if (res.ContentLength > 0)
                {
                    System.IO.Stream io = res.GetResponseStream();
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        int data = -1;
                        do
                        {
                            data = io.ReadByte();
                            if (data == -1)
                                break;
                            ms.WriteByte((byte)data);
                        } while (data != -1);
                        resRaw = ms.ToArray();
                    }
                }
                if (resRaw != null)
                    Encoding.UTF8.GetString(resRaw);
                {
                    if (res.StatusCode == System.Net.HttpStatusCode.TemporaryRedirect)
                    {
                        req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(res.GetResponseHeader("Location"));
                        req.Method = "GET";
                        req.CookieContainer = new System.Net.CookieContainer();
                        foreach (System.Net.Cookie cok in res.Cookies)
                        {
                            req.CookieContainer.Add(cok);
                        }
                        res = (System.Net.HttpWebResponse)req.GetResponse();
                    }
                    else
                        throw new Exception();
                }
                {
                }
            }
            catch
            {
            }
        }

        System.Threading.AutoResetEvent webclientSignal = new System.Threading.AutoResetEvent(false);


        List<System.Threading.Thread> managedThreads = new List<System.Threading.Thread>();

        [TestMethod]
        [DeployScenario]
        [TestCategory("BVT")]
        [TestCategory("Disabled")]
        [Ignore]
        public void PreAuthentication_WorkplaceJoined_Federation_AsProxy_Success()
        {
            server.Dispose();
#if !NODEPLOY
            UninstallProxy();
#endif
            //there is bug, AD FS may need do twice to really forgot proxy's old trust certificate
            for (int i = 0; i < 2; i++)
            {
#if !NODEPLOY
                if (System.IO.File.Exists(EnvironmentConfig.ProxyTrustCertificatePath))
                    System.IO.File.Delete(EnvironmentConfig.ProxyTrustCertificatePath);

#endif
                Microsoft.Protocols.TestTools.StackSdk.Networking.Http.WebProxy.WebProxy proxy = new TestTools.StackSdk.Networking.Http.WebProxy.WebProxy();
                proxy.HostNameForSSLToTargetServerHandler = new TestTools.StackSdk.Networking.Http.WebProxy.WebProxy.OnInputHostName(returnHostName);
                proxy.ReceiveMessageFromTargetCallback = new TestTools.StackSdk.Networking.Http.WebProxy.WebProxy.OnReceiveMessage(OnReceiveTargetMessage);
                proxy.ReceiveMessageFromSourceCallback = new TestTools.StackSdk.Networking.Http.WebProxy.WebProxy.OnReceiveMessage(OnReceiveSourceMessage);
                if (System.IO.File.Exists(EnvironmentConfig.ProxyTrustCertificatePath))
                {
                    proxy.SetClientCertificate(new X509Certificate2(EnvironmentConfig.ProxyTrustCertificatePath, EnvironmentConfig.ProxyTrustCertificatePassword));
                }
                else
                    proxy.SetClientCertificate(new X509Certificate2(EnvironmentConfig.TLSServerCertificatePath, EnvironmentConfig.TLSServerCertificatePassword));
                proxy.Initialize(EnvironmentConfig.ADFSFarmIP, Constraints.HTTPSServiceDefaultPort, new RemoteCertificateValidationCallback(sourceCertValidationCallback), new LocalCertificateSelectionCallback(sourceServerCertSelectionCallback), null
                    , new RemoteCertificateValidationCallback(targetCertValidationCallback), EncryptionPolicy.AllowNoEncryption, new X509Certificate2(EnvironmentConfig.TLSServerCertificatePath, EnvironmentConfig.TLSServerCertificatePassword, X509KeyStorageFlags.Exportable));
                
                preauth_fed_workplacejoined_asProxy_success(proxy);

                if (accessToWebApp)
                {
                    break;
                }
                proxy.Dispose();
            }
            BaseTestSite.Assert.IsTrue(accessToWebApp, "Must receive HTTP GET to web app");

            if (authTokenPresent && accessToWebApp && backendTlsCaptured) { }
            return;

        }

        object resourceLocker = new object();

        ManagedProxyEvent first = null;
        ManagedProxyEvent second = null;


    }

}
