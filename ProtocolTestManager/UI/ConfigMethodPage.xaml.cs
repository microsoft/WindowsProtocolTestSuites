// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for ConfirmPage.xaml
    /// </summary>
    public partial class ConfigMethodPage : Page
    {
        public ConfigMethodPage()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void AutoDetectButton_Click(object sender, RoutedEventArgs e)
        {
            if (AutoDetectClicked != null)
            {
                AutoDetectClicked();
            }
        }

        private void ManualConfigureButton_Click(object sender, RoutedEventArgs e)
        {
            if (ManualConfigClicked != null)
            {
                ManualConfigClicked();
            }
        }
        private void LoadSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoadSettingsClicked != null)
            {
                LoadSettingsClicked();
            }
        }
        public delegate void LinkButtonClicked();
        public LinkButtonClicked AutoDetectClicked;

        public LinkButtonClicked ManualConfigClicked;

        public LinkButtonClicked LoadSettingsClicked;


    }
}
