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
                MessageBox.Show("ProtocolTestManager is already running...");
                mutex = null;
                App.Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if(mutex != null)
            {
                mutex.ReleaseMutex();
            }
            base.OnExit(e);
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs args)
        {
            Exception e = args.Exception;

            ShowErrorMessage(e.ToString());
            LogExceptionDetail(e);
            args.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowErrorMessage(e.ToString());
            LogExceptionDetail((Exception)e.ExceptionObject);
        }

        void ShowErrorMessage(string msg)
        {
            string errorMsg = string.Format("An unexpected error occurred. You have found a bug.{0} Detail Exception:{1}", Environment.NewLine, msg);
            //Show message
            MessageBox.Show(
                    errorMsg,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.None);
        }

        void LogExceptionDetail(Exception e)
        {
            Utility util = new Utility();
            string errorLog = Path.Combine(util.AppConfig.AppDataDirectory, "Exception_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".log");
            using (StreamWriter errorWriter = new StreamWriter(errorLog))
            {
                errorWriter.WriteLine("Error Message: " + e.Message);
                errorWriter.WriteLine("Stack Trace: " + e.StackTrace);
            }
        }
    }
}
