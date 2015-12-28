// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestManager.Kernel;

namespace Microsoft.Protocols.TestManager.UI
{
    static class Pages
    {
        public static Utility util;
        //Mainwindow reference
        public static MainWindow mainWindow;
        public static TestSuiteWindow TestSuiteWindow;
        public static WelcomePage WelcomePage;
        public static RulePage RulePage;
        public static ConfigPage ConfigPage;
        public static RunPage RunPage;
        public static AutoDetectionPage AutoDetectionPage;
        public static ConfigMethodPage ConfigMethodPage;
        public static SUTInfoPage SUTInfoPage;
        public static AdapterPage AdapterPage;

        public static void Initialize(MainWindow mainWindow, Utility utility)
        {
            Pages.mainWindow = mainWindow;
            util = utility;
            TestSuiteWindow = new TestSuiteWindow();
            WelcomePage = new WelcomePage();
            RulePage = new RulePage();
            ConfigMethodPage = new ConfigMethodPage();
            ConfigPage = new ConfigPage();
            RunPage = new RunPage();
            AutoDetectionPage = new AutoDetectionPage();
            SUTInfoPage = new SUTInfoPage();
            AdapterPage = new AdapterPage();
        }
    }
}
