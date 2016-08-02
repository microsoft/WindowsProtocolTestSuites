// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
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
    }
}
