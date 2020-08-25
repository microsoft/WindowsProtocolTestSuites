// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Kernel;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for CustomizeTestSuiteWindow.xaml
    /// </summary>
    public partial class CustomizeTestSuiteWindow : Window
    {
        private const string NA = "N/A";

        private TestSuiteInfo notCustomizedInfo;

        private CustomizedTestSuiteConfigurationItem result;

        public CustomizeTestSuiteWindow()
        {
            InitializeComponent();
        }

        public CustomizedTestSuiteConfigurationItem ShowDialog(TestSuiteInfo info, TestSuiteInfo notCustomized)
        {
            CheckCore.IsChecked = false;

            CheckFx.IsChecked = false;

            notCustomizedInfo = notCustomized;

            if (info.IsCore)
            {
                CheckCore.IsChecked = true;

                CoreVersion.Text = info.TestSuiteVersion;

                Location.Text = info.TestSuiteFolder;
            }
            else
            {
                CheckFx.IsChecked = notCustomized.IsInstalled;

                CoreVersion.Text = NA;

                Location.Text = null;
            }

            if (notCustomized.IsInstalled)
            {
                CheckFx.IsChecked = !info.IsCore;

                InstalledVersion.Text = notCustomized.TestSuiteVersion;
            }
            else
            {
                // Disable the radio button if no corresponding MSI installed.
                CheckFx.IsEnabled = false;

                InstalledVersion.Text = NA;
            }

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

        private void OnOKClicked(object sender, RoutedEventArgs e)
        {
            if (IsCoreCheck())
            {
                if (CoreVersion.Text == NA)
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
                        Version = CoreVersion.Text,
                        IsCore = true,
                    };
                }
            }
            else
            {
                DialogResult = true;

                result = new CustomizedTestSuiteConfigurationItem
                {
                    Location = null,
                    IsCore = false,
                };
            }

            Close();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

            Close();
        }

        private void OnFxChecked(object sender, RoutedEventArgs e)
        {
            OnRadioChanged(false);
        }

        private void OnCoreChecked(object sender, RoutedEventArgs e)
        {
            OnRadioChanged(true);
        }

        private bool IsCoreCheck()
        {
            return CheckCore.IsChecked.HasValue && CheckCore.IsChecked.Value;
        }

        private void OnRadioChanged(bool isCore)
        {
            MSIStatusGroup.IsEnabled = !isCore;

            Configuration.IsEnabled = isCore;

            CheckCore.FontWeight = isCore ? FontWeights.Bold : FontWeights.Normal;

            CheckFx.FontWeight = !isCore ? FontWeights.Bold : FontWeights.Normal;
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

        private void LocationChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string version = Utility.GetCoreTestSuiteVersion(Location.Text);

            CoreVersion.Text = version == null ? NA : version;
        }
    }
}
