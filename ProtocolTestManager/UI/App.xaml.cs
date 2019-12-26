// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Kernel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex = null;

        public App() : base()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            bool isNewInstance = false;
            mutex = new Mutex(true, "{FE998190-5B44-4816-9A65-295E8A1EBBA1}", out isNewInstance);

            if (!isNewInstance)
            {
                UserPromptWindow.Show(StringResources.Error, StringResources.PTMRunning, UserPromptWindow.IconType.Error);
                mutex = null;
                App.Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (mutex != null)
            {
                mutex.ReleaseMutex();
            }
            base.OnExit(e);
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs args)
        {
            Exception e = args.Exception;

            LogExceptionAndPrompt(StringResources.Error, new Exception[] { e });

            args.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogExceptionAndPrompt(StringResources.Error, new Exception[] { (Exception)e.ExceptionObject });
        }

        public static void LogExceptionAndPrompt(string title, IEnumerable<Exception> exceptions)
        {
            string logPath = Utility.LogException(exceptions.ToList());

            string errorMsg = string.Format(StringResources.ExceptionsHappendWithLogsRecorded, logPath);

            UserPromptWindow.ShowWithLinks(title, errorMsg, UserPromptWindow.IconType.Error);
        }
    }
}
