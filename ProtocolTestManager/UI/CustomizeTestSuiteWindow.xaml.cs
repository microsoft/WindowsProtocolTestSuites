// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Kernel;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using Timer = System.Windows.Forms.Timer;

namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for CustomizeTestSuiteWindow.xaml
    /// </summary>
    public partial class CustomizeTestSuiteWindow : Window
    {
        private const string NA = "N/A";

        private const string DETECTING = "Detecting...";

        private TestSuiteInfo notCustomizedInfo;

        private CustomizedTestSuiteConfigurationItem result;

        private Timer timer;

        private bool isDetecting;

        private bool needUpdate;

        private bool changedDuringDetecting;

        public CustomizeTestSuiteWindow()
        {
            InitializeComponent();
        }

        public CustomizedTestSuiteConfigurationItem ShowDialog(TestSuiteInfo info, TestSuiteInfo notCustomized)
        {
            notCustomizedInfo = notCustomized;

            CoreVersion.Text = info.TestSuiteVersion;

            Location.Text = info.TestSuiteFolder;

            // Initialize timer for detecting the path user specified.
            isDetecting = false;

            needUpdate = false;

            changedDuringDetecting = false;

            timer = new Timer();

            timer.Interval = 100;

            timer.Tick += Timer_Tick;

            timer.Start();

            var ret = ShowDialog();

            if (ret.HasValue && ret.Value)
            {
                result.Name = info.TestSuiteName;

                return result;
            }
            else
            {
                return null;
            }
        }

        private async void OnOKClicked(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            string path = Location.Text;

            string version = await Task.Run(() => Utility.GetCoreTestSuiteVersion(path));

            if (version == null)
            {
                DialogResult = false;

                System.Windows.MessageBox.Show(StringResources.InvalidLocationSpecified, StringResources.Warning, MessageBoxButton.OK);

                return;
            }
            else
            {
                DialogResult = true;

                result = new CustomizedTestSuiteConfigurationItem
                {
                    Location = Location.Text,
                    Version = version,
                    IsCore = true,
                };
            }


            Close();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

            Close();
        }

        private void OnBrowseClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            var result = dialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            Location.Text = dialog.SelectedPath;
        }

        private void OnLocationChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            needUpdate = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isDetecting)
            {
                if (needUpdate)
                {
                    needUpdate = false;

                    changedDuringDetecting = true;
                }

                return;
            }
            else if (needUpdate || changedDuringDetecting)
            {
                needUpdate = false;

                changedDuringDetecting = false;
            }
            else
            {
                return;
            }

            isDetecting = true;

            CoreVersion.Text = DETECTING;

            string text = Location.Text;

            Task.Run(() =>
            {
                string version = Utility.GetCoreTestSuiteVersion(text);

                // Add sleep to avoid triggering detection too frequently.
                Thread.Sleep(50);

                Dispatcher.Invoke(() =>
                {
                    CoreVersion.Text = version == null ? NA : version;

                    isDetecting = false;
                });
            });
        }
    }
}
