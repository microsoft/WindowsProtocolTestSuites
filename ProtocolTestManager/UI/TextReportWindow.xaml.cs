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
    public partial class TextReportWindow : Window
    {
        public TextReportWindow()
        {
            InitializeComponent();
            UpdatePreview(this, new RoutedEventArgs());
        }


        private void CloseDialog_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void UpdatePreview(object sender, RoutedEventArgs e)
        {

            List<CaseListItem> preview = new List<CaseListItem>();
            if (Passed.IsChecked == true)
            {
                preview.Add(new CaseListItem("TestCase01", TestCaseStatus.Passed));
                preview.Add(new CaseListItem("TestCase03", TestCaseStatus.Passed));
            }
            if (Failed.IsChecked == true)
            {

                preview.Add(new CaseListItem("TestCase02", TestCaseStatus.Failed));
                preview.Add(new CaseListItem("TestCase04", TestCaseStatus.Failed));
            }
            if (Inconclusive.IsChecked == true)
            {
                preview.Add(new CaseListItem("TestCase05", TestCaseStatus.Other));
                preview.Add(new CaseListItem("TestCase06", TestCaseStatus.Other));
            }
            if (NotRun.IsChecked == true)
            {
                preview.Add(new CaseListItem("TestCase07", TestCaseStatus.NotRun));
                preview.Add(new CaseListItem("TestCase08", TestCaseStatus.NotRun));
            }
            PreviewText.Text = Utility.GeneratePlainTextReport(
                preview,
                CollumnOutcome.IsChecked == true,
                SortByName.IsChecked == true ? Utility.SortBy.Name : Utility.SortBy.Outcome,
                SeparatorSpace.IsChecked == true ? CaseListItem.Separator.Space : CaseListItem.Separator.Comma);
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            var caselist = Pages.util.GenerateTextCaseListItems(
                Passed.IsChecked == true,
                Failed.IsChecked == true,
                Inconclusive.IsChecked == true,
                NotRun.IsChecked == true);
            if (caselist.Count == 0)
            {
                MessageBox.Show(StringResources.NoTestCaseSelected);
                return;
            }
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Win32.SaveFileDialog();
            saveFileDialog.Filter = StringResources.TextFilter;
            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    string caseListText = Utility.GeneratePlainTextReport(
                        caselist,
                        CollumnOutcome.IsChecked == true,
                        SortByName.IsChecked == true ? Utility.SortBy.Name : Utility.SortBy.Outcome,
                        SeparatorSpace.IsChecked == true ? CaseListItem.Separator.Space : CaseListItem.Separator.Comma);
                    sw.Write(caseListText);
                }
                DialogResult = true;
            }
        }
    }
}
