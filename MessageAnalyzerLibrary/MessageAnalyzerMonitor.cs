// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Opn.Runtime.Metadata;
using Microsoft.Opn.Runtime.Monitoring;
using Microsoft.Opn.Runtime.Services;
using Microsoft.Opn.Runtime.Utilities;
using Microsoft.Protocols.Tools.Compiler;
using Microsoft.Protocols.Tools.Framework;
using Microsoft.Protocols.Tools.Modeling;
using Microsoft.Protocols.Tools.UI.Services;
using Microsoft.Protocols.Tools.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Microsoft.Protocols.TestTools.MessageAnalyzer
{
    public class MessageAnalyzerMonitor : IDisposable
    {
        public IMonitor PEFRuntimeMonitor { get { return monitor; } }

        #region Variables
        private bool isMAInstalled = false;

        private IMonitor monitor;
        // Track whether Dispose has been called.
        private bool disposed = false;
        ///opn file list that MA load
        private List<string> opnlist = null;

        #endregion

        #region Static Methods

        private static MessageAnalyzerMonitor instance = null;
        /// <summary>
        /// Constructor
        /// Initial MA environment.
        /// Make sure only one instance exists during runtime. CreateMonitor cannot be invoked until before instance is disposed
        /// </summary>
        public static MessageAnalyzerMonitor CreateMonitor(List<string> groupList = null, bool isMAInstalled = false)
        {
            if (instance != null)
            {
                instance.Dispose();
            }

            instance = new MessageAnalyzerMonitor(groupList, isMAInstalled);
            return instance;
        }

        #endregion Static Constructor

        #region Properties
        public List<string> OpnList { get { return opnlist; } }

        private static ICertificateStore CertificateStore
        {
            get
            {
                return ModelCatalog.Host.GetService<Microsoft.Opn.Runtime.Services.ICertificateStore>();
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Create Live Capture
        /// </summary>
        /// <param name="providerList">ETW Provider list</param>
        /// <returns></returns>
        public LiveTraceSession CreateLiveTraceSession(IList<string> providerList = null)
        {
            LiveTraceSession liveCapture = new LiveTraceSession(providerList);
            return liveCapture;
        }


        /// <summary>
        /// Create a file Capture
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isCaptureJournal">if true, get data from captureJournal, else get data from viewJournal</param>
        /// <returns></returns>
        public Session CreateCaptureFileSession(string filePath, bool disablePersistence = false, bool catchAllMessages = false)
        {
             CaptureFileSession fileCapture = new CaptureFileSession(this.monitor, filePath, disablePersistence, catchAllMessages);
             return fileCapture;
        }

        /// <summary>
        /// add a certificate to certificate store
        /// </summary>
        /// <param name="certPath">certificate path</param>
        /// <param name="password">password of the certificate</param>
        public void AddCertificate(string certPath, string password)
        {
            if (!File.Exists(certPath))
            {
                throw new FileNotFoundException(string.Format(
                    "The specified file \"{0}\" was not found.", certPath));
            }

            if (CertificateStore == null)
            {
                throw new InvalidOperationException(
                    "Cannot get certificate store. It may be a bug of OPN Functional Test Framework.");
            }

            SecureString secString = new SecureString();
            foreach (char c in password)
            {
                secString.AppendChar(c);
            }

            if (!CertificateStore.AddCertificate(certPath, secString))
            {
                throw new InvalidOperationException(
                    "Password for the cert is incorrect.");
            }
        }

        /// <summary>
        /// clear all the certificates in certificate store
        /// </summary>
        public void ClearCertificates()
        {
            if (CertificateStore != null)
            {
                CertificateStore.Clear();
            }
        }

        #region Dispose 
        /// <summary>
        /// Dispose this Monitor.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
                
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    if (monitor != null)
                    {
                        monitor.Dispose();
                        MessageAnalyzerMonitor.instance = null;
                    }
                }
                disposed = true;
            }
        }

        /// <summary>
        /// Use C# destructor syntax for finalization code
        /// </summary>
        ~MessageAnalyzerMonitor()
        {
            Dispose(false);
        }
        #endregion Dispose

        #endregion Public Methods

        #region Private methods
        /// <summary>
        /// Create MA monitor
        /// </summary>
        private MessageAnalyzerMonitor(List<string> GroupList = null, bool isMAInstalled = false)
        {
            this.isMAInstalled = isMAInstalled;

            ConfigureEnvironment();
            opnlist = new List<string>();
            IMonitorSettings monitorSettings = MonitorFactory.CreateDefaultSettings();

            string modelpath = null;
            if (isMAInstalled)
            {
                LiveTraceSession.EnsurePowerShellInitiation();
                // If MMA installed, use OPNs under local app data, same as MMA UI, otherwise it will re-generate the CompilationCache if using MMA UI simutaneously.
                monitorSettings.Host = CreateHostForMAInstalled();
                string OPNAndConfigurationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "MessageAnalyzer", "OPNAndConfiguration");
                modelpath = Path.Combine(OPNAndConfigurationPath, "Opns"); //OpnAndConfiguration\Opns\ in local app data                       
            }
            else
            {
                monitorSettings.Host = CreateHost();
                modelpath = PlatformManager.ModelsDirectory; // PlatformManager.ModelsDirectory is: "OpnAndConfiguration\Opns\"
                string opnForEtw = Path.Combine(Path.GetDirectoryName(modelpath), "OpnForEtw"); // OpnAndConfiguration\OpnForEtw
                monitorSettings.ModelLoadPath.Add(opnForEtw);
            }

            if (GroupList == null)
            {
                monitorSettings.ModelLoadPath.Add(modelpath);
                opnlist.Add(modelpath);
            }
            else
            {
                modelpath = Path.GetDirectoryName((PlatformManager.ModelsDirectory)); // modelpath is set to the path of folder "OpnAndConfiguration"
                GroupList.ForEach(e => monitorSettings.ModelLoadPath.Add(modelpath + "\\" + e));
                GroupList.ForEach(e => opnlist.Add(modelpath + "\\" + e));
            }
            monitorSettings.ExtensionLoadPath.Add(PlatformManager.ExtensionsDirectory);
            monitor = MonitorFactory.CreateLocalMonitor(monitorSettings);
            monitor.Initialize();

            ModelCatalog.WaitForInitialization();

            monitor.CatchExceptionOnGetMessageData = true;
        }

        /// <summary>
        /// Create Host for MA environment
        /// </summary>
        /// <returns></returns>
        private IHost CreateHost()
        {
            string modelCachePath = PlatformManager.DefaultModelCompilationCacheDirectory;
            string codecCachePath = Path.Combine(PlatformManager.BaseDirectory, "CodecCache");
            //Create and register a host
            var host = new SimpleHost();
            SimpleModelManager modelManager = new SimpleModelManager(modelCachePath);
            host.SetModelManager(modelManager);
            new RuntimeTechnology().Register(host);
            new CoreTechnology().Register(host);
            new OPNTechnology().Register(host);
            new CompilerTechnology().Register(host);
            // host.RegisterService(new CertificateStore());

            var ext = new ExtensionContainer(PlatformManager.ExtensionsDirectory);
            foreach (var tech in ext.technologies)
            {
                tech.Register(host);
            }

            var runtimeProps = host.GetService<RuntimeProperties>();
            runtimeProps.AssemblyCachePath = modelCachePath;
            var compilerProps = host.GetService<CompilerProperties>();
            compilerProps.CodecCacheFolder = codecCachePath;

            host.EndRegistration();
            return host;
        }

        /// <summary>
        /// Create Host for MA environment when MA has been installed
        /// </summary>
        /// <returns></returns>
        private IHost CreateHostForMAInstalled()
        {
            var compilationCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "MessageAnalyzer", "CompilationCache");
            var codecCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "MessageAnalyzer", "CodecCache");

            //Create and register a host
            SimpleHost host = new SimpleHost();
            SimpleModelManager modelManager = new SimpleModelManager(compilationCachePath);
            host.SetModelManager(modelManager);

            new CoreTechnology(StandardStages.Runtime).Register(host);
            new RuntimeTechnology().Register(host);
            new OPNTechnology().Register(host);
            new CompilerTechnology().Register(host);

            var ext = new ExtensionContainer(PlatformManager.ExtensionsDirectory);
            foreach (var tech in ext.technologies)
            {
                tech.Register(host);
            }

            var runtimeProps = host.GetService<RuntimeProperties>();
            runtimeProps.AssemblyCachePath = compilationCachePath;

            var compilerProps = host.GetService<CompilerProperties>();
            compilerProps.CodecCacheFolder = codecCachePath;

            host.EndRegistration();
            return host;
        }

        /// <summary>
        /// delete files to make sure MA library support parsing .matp file
        /// </summary>
        // TODO: it's kind of workaround method, if MA API changes and will not incur duplicate files, delete part of this code 
        private void ConfigureEnvironment()
        {
            string currentAppPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string devPath = Environment.GetEnvironmentVariable("INETROOT");
            string baseDirForUT = Environment.GetEnvironmentVariable("BaseDirForUT");
            // Only call PlatformManager.Initialize() when MA is not installed.
            // If MA is installed, default initialization will happen.
            if (!isMAInstalled)
            {
                // 1) If application running in corext environment, use %INETROOT%\target\debug\i386 to initialize PlatformManager.
                // 2) If %BaseDirForUT% is set, use it to initialize. This variable is used for unit test project to instruct the MA base directory.
                //    When running a unit test, mstest will create new directory (which is current AppDomain's base directory) to run it. 
                //    So, we cannot use current AppDomain's base directory to initialize PlatformManager. Instead, we should use the path where
                //    the original assemblies locate (by setting %BaseDirForUT%).
                // 3) Otherwise, use current AppDomain's base directory to initialize.
                if (devPath != null)
                {
                    devPath = Path.Combine(devPath, @"target\debug\i386");
                }
                if (Directory.Exists(devPath))
                {
                    PlatformManager.Initialize(devPath);
                }
                else if(Directory.Exists(baseDirForUT))
                {
                    PlatformManager.Initialize(baseDirForUT);
                }
                else
                {
                    PlatformManager.Initialize(currentAppPath);
                }
            }
            string[] files = { 
                "EtwProviderAdapter.dll", 
                "EtwProviderInformationApis.dll",
                "Microsoft.Opn.Runtime.Messaging.Etw.dll",
                "ClientInterfaces.dll",  
                "Microsoft.Opn.Runtime.Messaging.RemoteCapture.dll",
            };
            foreach (string file in files)
            {
                string path = Path.Combine(currentAppPath, file);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
        #endregion Private methods
    }
}
