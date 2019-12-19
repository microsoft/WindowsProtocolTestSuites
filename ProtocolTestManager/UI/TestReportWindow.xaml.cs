// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Kernel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for TextReportWindow.xaml
    /// </summary>
    public partial class TestReportWindow : Window
    {
        public TestReportWindow()
        {
            InitializeComponent();
        }

        private void CloseDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            var caselist = Pages.util.SelectTestCases(
                Passed.IsChecked == true,
                Failed.IsChecked == true,
                Inconclusive.IsChecked == true,
                false);
            if (caselist.Count == 0)
            {
                UserPromptWindow.Show(StringResources.Error, StringResources.NoTestCaseSelected, UserPromptWindow.IconType.Error);
                return;
            }

            var selectedRadioButton = ReportFormatGroup.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true);
            if (selectedRadioButton == null)
            {
                UserPromptWindow.Show(StringResources.Error, StringResources.NoReportFormatSelected, UserPromptWindow.IconType.Error);
                return;
            }
            TestReport report = TestReport.GetInstance(selectedRadioButton.Name, caselist);
            if (report == null)
            {
                UserPromptWindow.Show(StringResources.Error, StringResources.UnknownReportFormat, UserPromptWindow.IconType.Error);
                return;
            }

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Win32.SaveFileDialog();
            saveFileDialog.Filter = report.FileDialogFilter;
            saveFileDialog.DefaultExt = report.FileExtension;
            if (saveFileDialog.ShowDialog() == true)
            {
                report.ExportReport(saveFileDialog.FileName);
                DialogResult = true;
            }
        }
    }
}
