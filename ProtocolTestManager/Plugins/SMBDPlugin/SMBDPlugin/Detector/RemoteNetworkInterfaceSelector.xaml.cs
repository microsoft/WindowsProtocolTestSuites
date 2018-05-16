// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.SMBDPlugin.Detector;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    /// <summary>
    /// Interaction logic for RemoteNetworkInterfaceSelector.xaml
    /// </summary>
    public partial class RemoteNetworkInterfaceSelector : Window
    {
        public RemoteNetworkInterfaceInformation[] LocalNetworkInterfaceInformation;

        private object selectedItem;
        private DispatcherTimer timer;
        private uint ticks;
        private string titleHint;


        public RemoteNetworkInterfaceSelector(RemoteNetworkInterfaceInformation[] networkInterfaces)
        {
            LocalNetworkInterfaceInformation = networkInterfaces;
            InitializeComponent();
            Viewer.ItemsSource = LocalNetworkInterfaceInformation;
            OkButton.IsEnabled = false;
            timer = null;
        }


        public RemoteNetworkInterfaceInformation ShowDialog(string title, string prompt, uint showSeconds = 30)
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            ticks = showSeconds;
            titleHint = title;
            Prompt.Text = prompt;
            UpdateTitle();
            timer.Start();

            var ret = ShowDialog();
            if (ret.HasValue && ret.Value)
            {
                return selectedItem as RemoteNetworkInterfaceInformation;
            }
            else
            {
                return null;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ticks--;
            UpdateTitle();
            if (ticks == 0)
            {
                Close();
            }
        }

        private void UpdateTitle()
        {
            Title = String.Format("{0} ({1})", titleHint, ticks);
        }


        private void Viewer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OkButton.IsEnabled = true;
            selectedItem = Viewer.SelectedItem;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
            }
        }
    }
}
