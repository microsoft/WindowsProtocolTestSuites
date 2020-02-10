// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO.Pipes;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Microsoft.Protocols.TestManager.Kernel;
using System.ComponentModel;
using System.Linq;

namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for RunPage.xaml
    /// </summary>
    public partial class RunPage : Page
    {

        public RunPage()
        {
            InitializeComponent();
        }

        private Logger logger = null;
        private logView logview;

        public void SetLogger(Logger logger)
        {
            this.logger = logger;
            logview = new logView(logger);
            GroupingMethod.SelectedIndex = 0;
            SetTestCases(logger.GroupByOutcome.GetList());
            LayoutRoot.DataContext = logview;
            testCaseList = logger.AllTestCases;
        }

        /// <summary>
        /// Set the cases shown in the TreeView
        /// </summary>
        /// <param name="groups"></param>
        private void SetTestCases(List<TestCaseGroup> groups)
        {
            TestOutcome.Items.Clear();
            foreach (var i in groups)
            {
                var template = FindResource("TestCaseTemplate") as DataTemplate;
                var style = FindResource("styleTreeView") as Style;

                // Header 
                var headerCheckbox = new CheckBox()
                {
                    IsThreeState = false,
                    IsChecked = i.IsChecked,
                    Focusable = false,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center
                };
                var headerLabel = new Label()
                {
                    Content = i.HeaderText,
                    VerticalAlignment = System.Windows.VerticalAlignment.Bottom
                };
                var header = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Children = {
                        headerCheckbox,
                        headerLabel
                    }
                };
                headerCheckbox.Checked += (s, arg) =>
                {
                    i.CheckAllSubItems(true);
                    foreach (var g in groups) g.UpdateHeader();
                };
                headerCheckbox.Unchecked += (s, arg) =>
                {
                    i.CheckAllSubItems(false);
                    foreach (var g in groups) g.UpdateHeader();
                };

                // TreeView Items
                var item = new TreeViewItem()
                {
                    Header = header,
                    ItemContainerStyle = style,
                    ItemTemplate = template,
                    Visibility = (Visibility)Enum.Parse(typeof(Visibility), i.Visibility),
                    ItemsSource = i.TestCaseList
                };
                i.PropertyChanged += (s, arg) =>
                {
                    UpdateRunSelectedText(groups);

                    if (arg.PropertyName == "Visibility")
                    {
                        var dispatcher = item.Dispatcher;
                        if (dispatcher.CheckAccess())
                        {
                            item.Visibility = (Visibility)Enum.Parse(typeof(Visibility), i.Visibility);
                        }
                        else
                        {
                            item.Dispatcher.Invoke(
                                new Action(() => item.Visibility = (Visibility)Enum.Parse(typeof(Visibility), i.Visibility))
                            );

                        }
                    }
                    if (arg.PropertyName == "IsChecked")
                    {
                        var dispatcher = headerCheckbox.Dispatcher;
                        if (dispatcher.CheckAccess())
                        {
                            headerCheckbox.IsChecked = i.IsChecked;
                        }
                        else
                        {
                            headerCheckbox.Dispatcher.Invoke(
                                new Action(() => headerCheckbox.IsChecked = i.IsChecked)
                        );
                        }
                    }
                    if (arg.PropertyName == "HeaderText")
                    {
                        var dispatcher = headerCheckbox.Dispatcher;
                        if (dispatcher.CheckAccess())
                        {
                            headerLabel.Content = i.HeaderText;
                        }
                        else
                        {
                            headerLabel.Dispatcher.Invoke(
                                new Action(() => headerLabel.Content = i.HeaderText)
                                );
                        }
                    }
                    if (arg.PropertyName == "Status")
                    {
                        var dispatcher = item.Dispatcher;
                        if (dispatcher.CheckAccess())
                        {
                            item.ItemsSource = i.TestCaseList;
                        }
                        else
                        {
                            item.Dispatcher.Invoke(
                                new Action(() => item.ItemsSource = i.TestCaseList)
                                );
                        }
                    }
                };
                TestOutcome.Items.Add(item);
            }
            UpdateRunSelectedText(groups);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private TestCase selectedTestCase = null;

        private void RunTest_Click(object sender, RoutedEventArgs e)
        {
            if (!enableControls) return;
            List<TestCase> testcaselist = new List<TestCase>();
            List<string> currentPageList = logger.CurrentPageCaseList;

            foreach (var test in testCaseList)
            {
                if (test.IsChecked && currentPageList != null && currentPageList.Contains(test.Name))
                {
                    testcaselist.Add(test);
                }
            }
            if (RunTestClicked != null) RunTestClicked(testcaselist);
        }

        private List<TestCase> testCaseList;

        public delegate void RunTestClickedCallback(List<TestCase> testcase);
        public RunTestClickedCallback RunTestClicked;

        public delegate void RunAllTestCallback();
        public RunAllTestCallback RunAllTestClicked;

        private bool enableControls = true;
        public bool EnableControls
        {
            get { return enableControls; }
            set
            {
                if (!isFiltered)
                {
                    RunAllLink.IsEnabled = value;
                }
                RunAll.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                RunAllLink.Focusable = value;
                RunSelected.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                RunSelectedLink.Focusable = value;
                Keywords.IsEnabled = value;
                RunSelectedLink.IsEnabled = value;
                enableControls = value;
                FilterButton.IsEnabled = value;
                ExportImportMenu.IsEnabled = value;
                AbortExecutionLink.IsEnabled = !value;
                AbortExecutionLink.Focusable = !value;
                AbortExecution.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void RunAll_Click(object sender, RoutedEventArgs e)
        {
            if (RunAllTestClicked != null) RunAllTestClicked();
        }

        private void TreeViewItem_GotFocus(object sender, RoutedEventArgs e)
        {
            var myListBoxItem = sender as TreeViewItem;
            if (myListBoxItem == null) return;
            BindingExpression binding = myListBoxItem.GetBindingExpression(TreeViewItem.TagProperty);
            selectedTestCase = binding.DataItem as TestCase;
            if (selectedTestCase != null)
                logview.SelectTestCase(selectedTestCase);
            else
                logview.IsCurrent = true;
        }

        private void UncheckAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (TestCase testcase in testCaseList)
            {
                testcase.IsChecked = false;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (logger == null || e.AddedItems.Count == 0) return;
            string item = ((System.Windows.Controls.ComboBoxItem)e.AddedItems[0]).Name;
            if (item == "Category")
            {
                SetTestCases(logger.GroupByCategory.GetList());
            }
            else if (item == "Outcome")
            {
                SetTestCases(logger.GroupByOutcome.GetList());
            }
        }

        private bool isFiltered = false;

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (isFiltered)
            {
                RemoveFilter();
                Keywords.Text = "";
            }
            else
            {
                ApplyFilter();
            }
            switch (((System.Windows.Controls.ComboBoxItem)GroupingMethod.SelectedItem).Name)
            {
                case "Category":
                    SetTestCases(logger.GroupByCategory.GetList());
                    break;
                case "Outcome":
                    SetTestCases(logger.GroupByOutcome.GetList());
                    break;
            }
        }

        private void Keywords_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string text = Keywords.Text.Trim();
                if (string.IsNullOrEmpty(text))
                {
                    RemoveFilter();
                }
                else
                {
                    ApplyFilter();
                }
                switch (((System.Windows.Controls.ComboBoxItem)GroupingMethod.SelectedItem).Name)
                {
                    case "Category":
                        SetTestCases(logger.GroupByCategory.GetList());
                        break;
                    case "Outcome":
                        SetTestCases(logger.GroupByOutcome.GetList());
                        break;
                }
            }
        }

        private void ApplyFilter()
        {
            Pages.util.FilterByKeyword(Keywords.Text.Trim());
            ButtonIcon.Source = new BitmapImage(new Uri("pack://application:,,,/images/cross.png"));
            isFiltered = true;
            RunAllLink.IsEnabled = false;
        }

        private void RemoveFilter()
        {
            Pages.util.RemoveFilter();
            RunAllLink.IsEnabled = true;
            isFiltered = false;
            ButtonIcon.Source = new BitmapImage(new Uri("pack://application:,,,/images/find.png"));
        }

        private void ExportAllPlaylist_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog savefile = new Win32.SaveFileDialog();
                savefile.Filter = StringResources.PlaylistFilter;
                if (savefile.ShowDialog() == true)
                {
                    Pages.util.ExportPlaylist(savefile.FileName, false);
                }
            }
            catch (Exception exception)
            {
                UserPromptWindow.Show(StringResources.Error, exception.Message, UserPromptWindow.IconType.Error);

                return;
            }
        }

        private void ExportCheckedPlaylist_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog savefile = new Win32.SaveFileDialog();
                savefile.Filter = StringResources.PlaylistFilter;
                if (savefile.ShowDialog() == true)
                {
                    Pages.util.ExportPlaylist(savefile.FileName, true);
                }
            }
            catch (Exception exception)
            {
                UserPromptWindow.Show(StringResources.Error, exception.Message, UserPromptWindow.IconType.Error);

                return;
            }
        }

        private void ImportPlaylist_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog openfile = new Win32.OpenFileDialog();
                openfile.Filter = StringResources.PlaylistFilter;
                if (openfile.ShowDialog() == true)
                {
                    int checkedNumber, notFound;
                    Pages.util.ImportPlaylist(openfile.FileName);
                    Pages.util.ApplyPlaylist(out checkedNumber, out notFound);
                    if (notFound > 0)
                    {
                        UserPromptWindow.Show(StringResources.Error, string.Format(StringResources.NotFoundCaseMessage, notFound), UserPromptWindow.IconType.Error);
                    }
                }
            }
            catch (Exception exception)
            {
                UserPromptWindow.Show(StringResources.Error, exception.Message, UserPromptWindow.IconType.Error);

                return;
            }
        }

        private void SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog savefile = new Win32.SaveFileDialog();
                savefile.Filter = StringResources.TestProfileFilter;
                if (savefile.ShowDialog() == true)
                {
                    Pages.util.SaveProfileSettings(savefile.FileName);
                }
            }
            catch (Exception exception)
            {
                UserPromptWindow.Show(StringResources.Error, exception.Message, UserPromptWindow.IconType.Error);

                return;
            }
        }

        private void GenerateTestReport_Click(object sender, RoutedEventArgs e)
        {
            TestReportWindow testReport = new TestReportWindow();
            testReport.Owner = Pages.mainWindow;
            testReport.ShowDialog();
        }

        private void AbortExecution_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
                StringResources.AbortWarning,
                "Warning",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Pages.util.AbortExecution();
            }
        }

        private void TestOutcome_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space) return;
            var item = TestOutcome.SelectedItem;
            if (item is TestCase)
            {
                var testcase = item as TestCase;
                testcase.IsChecked = !testcase.IsChecked;
            }
            if (item is TreeViewItem)
            {
                TreeViewItem tvi = item as TreeViewItem;
                CheckBox checkbox = null;
                foreach (var i in ((StackPanel)tvi.Header).Children)
                {
                    if (i is CheckBox)
                    {
                        checkbox = (CheckBox)i;
                        break;
                    }
                }
                checkbox.IsChecked = !(checkbox.IsChecked == true);
            }
        }

        private void UpdateRunSelectedText(List<TestCaseGroup> groups)
        {
            var selectedCaseCount = groups
                .SelectMany(g => g.TestCaseList)
                .Where(c => c.IsChecked)
                .GroupBy(c => c.FullName)
                .Count();

            string text = selectedCaseCount == 0 ? "Run Selected Cases" : $"Run Selected Cases ({selectedCaseCount})";

            var dispatcher = RunSelectedLinkText.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                RunSelectedLinkText.Text = text;
            }
            else
            {
                RunSelectedLinkText.Dispatcher.Invoke(
                    new Action(() => RunSelectedLinkText.Text = text)
                );
            }

            dispatcher = RunSelectedMenuItem.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                RunSelectedMenuItem.Header = text;
            }
            else
            {
                RunSelectedLinkText.Dispatcher.Invoke(
                    new Action(() => RunSelectedMenuItem.Header = text)
                );
            }
        }
    }

    class logView
    {
        private TestCase testcase;
        public TestCase BindTestCase
        {
            get { return testcase; }
        }

        private bool isCurrent;
        public bool IsCurrent
        {
            get { return isCurrent; }
            set
            {
                isCurrent = value;
                if (isCurrent)
                {
                    testcase = logger.RunningTestCase;
                    SelectedCaseChanged();
                }
            }
        }

        private Logger logger;

        public logView(Logger logger)
        {
            this.logger = logger;
            logger.PropertyChanged += logger_PropertyChanged;
        }

        public void SelectTestCase(TestCase testcase)
        {
            IsCurrent = false;
            this.testcase = testcase;
            SelectedCaseChanged();
        }

        void logger_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RunningTestCase" && IsCurrent)
            {
                testcase = logger.RunningTestCase;
                SelectedCaseChanged();
            }
        }
        private void SelectedCaseChanged()
        {
            // testcase could be null because logger.RunningTestCase could be null.
            // So add check here to avoid null reference exception.
            if (testcase != null)
            {
                Pages.RunPage.WebBrowserLog.Url = testcase.LogUri;
            }
        }

    }
}
