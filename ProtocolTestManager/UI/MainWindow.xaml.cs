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
using System.IO;
using Microsoft.Protocols.TestManager.Kernel;
using System.Diagnostics;
using System.Threading;

namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int maxSelectedIndex = 8;
        private const int testSuiteSelectionIndex = 0;
        private const int welcomeIndex = 1;
        private const int configMethodIndex = 2;
        private const int detectionIndex = 3;
        private const int resultIndex = 4;
        private const int ruleIndex = 5;
        private const int configIndex = 6;
        private const int adapterIndex = 7;
        private const int runIndex = 8;

        Utility util = new Utility();
        bool isAutoDetected = false;

        bool enableConfigureAdapter = false;

        private void RegisterEvents()
        {
            Pages.TestSuiteWindow.Launch_Click = (u) =>
                {
                    if (u.IsInstalled)
                    {
                        try
                        {
                            util.LoadTestSuiteConfig(u);
                            util.LoadTestSuiteAssembly();
                        }
                        catch (Exception e)
                        {
                            UserPromptWindow.Show(StringResources.Error, e.Message, UserPromptWindow.IconType.Error);

                            return;
                        }
                        ListBox_Step.SelectedIndex = welcomeIndex;
                        Pages.WelcomePage.LoadPage(new Uri(util.AppConfig.UserGuide));
                        DisableFollowingItems();
                        util.InitializeDetector();
                        if (util.AppConfig.PredefinedAdapters != null && util.AppConfig.PredefinedAdapters.Count > 0)
                        {
                            enableConfigureAdapter = true;
                            Item_Adapter.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            Item_Adapter.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        Title = string.Format(StringResources.TitleFormat, util.AppConfig.TestSuiteName, util.AppConfig.TestSuiteVersion);
                    }
                    if (u.InstallStatus == InstallStatus.MSIAvailable)
                    {
                        System.Diagnostics.Process p = new System.Diagnostics.Process()
                        {
                            StartInfo = new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = u.Installer
                            }
                        };
                        p.Start();
                    }
                };
            Pages.TestSuiteWindow.Run_Click = (u) =>
                {
                    try
                    {
                        util.LoadTestSuiteConfig(u);
                        util.LoadTestSuiteAssembly();

                        if (util.AppConfig.PredefinedAdapters != null && util.AppConfig.PredefinedAdapters.Count > 0)
                        {
                            enableConfigureAdapter = true;
                            Item_Adapter.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            Item_Adapter.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        isAutoDetected = false;
                        util.InitializeDetector();

                        // Last profile should always on the correct version, no need to upgrade this profile
                        LoadProfile(util.LastRuleSelectionFilename);
                    }
                    catch (Exception e)
                    {
                        UserPromptWindow.Show(StringResources.Error, e.Message, UserPromptWindow.IconType.Error);

                        return;
                    }
                };
            Pages.ConfigMethodPage.AutoDetectClicked = () =>
                {
                    if (MessageBox.Show(StringResources.AutoDetectWarning, StringResources.AutoDetectWarningTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        isAutoDetected = true;
                        util.LoadPtfconfig();
                        Pages.AutoDetectionPage.SetPrerequisits(util.GetPrerequisits());
                        Pages.AutoDetectionPage.SetDetectionSteps(util.GetDetectSteps());
                        ListBox_Step.SelectedIndex = detectionIndex;
                        DisableFollowingItems();
                    }
                };

            Pages.ConfigMethodPage.ManualConfigClicked = () =>
                {
                    isAutoDetected = false;
                    util.LoadPtfconfig();
                    util.InitializeDetector();
                    ListBox_Step.SelectedIndex = ruleIndex;
                    UpdateCaseFilter();
                    DisableFollowingItems();
                    DisableDetectSteps();
                };
            Pages.ConfigMethodPage.LoadSettingsClicked = LoadProfile;
            Pages.AutoDetectionPage.ContentChanged += (s, e) =>
                {
                    if (detectionFinished)
                    {
                        isAutoDetected = true;
                        detectionFinished = false;
                        this.ButtonNext.Content = StringResources.DetectButton;
                        Pages.AutoDetectionPage.ResetSteps();
                        DisableFollowingItems();
                    }
                };
            Pages.RunPage.RunTestClicked = RunTestCaseClicked;
            Pages.RunPage.RunAllTestClicked = RunTest;
            Pages.RunPage.TotalResultsLink.Click += (s, e) =>
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "explorer.exe";
                    if (!string.IsNullOrEmpty(util.GetLogger().IndexHtmlFilePath))
                    {
                        startInfo.Arguments = util.GetLogger().IndexHtmlFilePath;
                    }
                    else
                    {
                        startInfo.Arguments = System.IO.Path.Combine(util.AppConfig.TestSuiteDirectory, AppConfig.HtmlResultFolderName);
                    }
                    Process.Start(startInfo);
                };
        }

        private bool detectionFinished = false;
        private bool detectionRunning = false;
        private void BeginDetection()
        {
            this.ButtonNext.Content = StringResources.StopDetectButton;
            this.detectionRunning = true;
            Pages.AutoDetectionPage.ResetSteps();
            detectionFinished = false;

            SetButtonsStatus(false, true);
            this.ListBox_Step.IsEnabled = false;
            Pages.AutoDetectionPage.PropertyListBox.IsEnabled = false;

            try
            {
                if (util.SetPrerequisits() != true) throw new Exception(StringResources.InvalidValue);
            }
            catch (Exception e)
            {
                UserPromptWindow.Show(StringResources.Error, e.Message, UserPromptWindow.IconType.Error);

                SetButtonsStatus(true, true);
                this.ListBox_Step.IsEnabled = true;
                Pages.AutoDetectionPage.PropertyListBox.IsEnabled = true;
                return;
            }

            this.Dispatcher.Invoke((Action)(() =>
            {
                this.ListBox_Step.IsEnabled = true;
            }));

            util.StartDetection((o) =>
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    detectionRunning = false;
                    if (o.Status == DetectionStatus.Finished) this.ButtonNext.Content = StringResources.NextButton;
                    else
                    {
                        this.ButtonNext.Content = StringResources.DetectButton;

                        UserPromptWindow.Show(StringResources.Error, o.Exception.Message, UserPromptWindow.IconType.Error);
                    }
                    SetButtonsStatus(true, true);

                    Pages.AutoDetectionPage.PropertyListBox.IsEnabled = true;
                    if (o.Status == DetectionStatus.Finished) detectionFinished = true;
                }));
            });
        }

        /// <summary>
        /// Detection finished, goto the result page.
        /// </summary>
        private void DetectionFinished()
        {
            Pages.SUTInfoPage.SUTContentCtrl.Content = util.GetDetectionSummary();
            util.ApplyDetectedValues();
        }

        private void UpdateCaseFilter()
        {
            if (detectionFinished && isAutoDetected)
            {
                // Reset select status before applying detected rules
                foreach (var group in util.GetFilter())
                {
                    group.IsSelected = false;
                }
                util.ApplyDetectedRules();
                detectionFinished = false;
            }
            Pages.RulePage.SetFilter(util.GetFilter());
            util.GetFilter().ContentModified = null;
            util.GetFilter().ContentModified += () =>
                {
                    DisableFollowingItems();
                    UpdateSelectedPageStatus();
                };
            UpdateSelectedPageStatus();
        }

        // Update case number and Next button status according to the selected case number.
        private void UpdateSelectedPageStatus()
        {
            var selectedCaseList = util.GetSelectedCaseList();
            Pages.RulePage.CaseList.ItemsSource = selectedCaseList;
            int caseCount = selectedCaseList.Count;
            Pages.RulePage.CaseNumberTextBlock.Text = caseCount.ToString();
            this.ButtonNext.IsEnabled = IsCaseSeleted(caseCount);
        }

        private bool IsCaseSeleted(int? caseCount = null)
        {
            if (caseCount == null)
            {
                caseCount = util.GetSelectedCaseList().Count;
            }

            return caseCount > 0 ? true : false;
        }

        private void GotoConfigPage()
        {
            util.HideProperties();
            var propertyView = util.CreatePtfPropertyView();
            PtfPropertyView.Modified = DisableFollowingItems;
            Pages.ConfigPage.BindItem(propertyView);
        }

        private void GotoAdapterPage()
        {
            util.AdapterConfigurationChanged += DisableFollowingItems;
            Pages.AdapterPage.SetDatasource(util.GetAdaptersView());

        }

        private void ApplyPtfConfigChanges()
        {
            if (enableConfigureAdapter) util.ApplyAdaptersConfig();
            util.SavePtfconfigToBinFolder();
            util.SavePtfconfigToSourceCode();
        }

        private void GotoRunPage()
        {
            util.InitializeTestEngine();
            util.TestFinished += (s, e) =>
            {
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    Logger log = util.GetLogger();
                    Pages.RunPage.TotalResults.Visibility = System.Windows.Visibility.Visible;
                    Pages.RunPage.TotalResultsLinkText.Focusable = true;
                    Pages.RunPage.TotalResultsLinkText.Text = String.Format("{0} / {1} Passed",
                       e.Passed, util.SelectedCaseCount);
                    if (e.Failed != 0)
                    {
                        Pages.RunPage.TotalResultsLinkText.Text += String.Format(", {0} Failed", e.Failed);
                    }
                    if (e.Inconclusive != 0)
                    {
                        Pages.RunPage.TotalResultsLinkText.Text += String.Format(", {0} Inconclusive", e.Inconclusive);
                    }
                    SetButtonsStatus(true, false);
                    this.ListBox_Step.IsEnabled = true;
                    Pages.RunPage.EnableControls = true;
                    Pages.AutoDetectionPage.PropertyListBox.IsEnabled = true;

                    if (e.Exception.Count > 0)
                    {
                        App.LogExceptionAndPrompt(StringResources.Error, e.Exception);
                    }
                    else
                    {
                        var counts = new int[] { e.Passed, e.Failed, e.Inconclusive };

                        if (counts.All(count => count == 0))
                        {
                            UserPromptWindow.Show(StringResources.Error, StringResources.NoTestExecuted, UserPromptWindow.IconType.Error);
                        }
                    }
                }));
            };
            Pages.RunPage.TotalResults.Visibility = System.Windows.Visibility.Hidden;
            Pages.RunPage.TotalResultsLinkText.Text = "";
            Logger logger = util.GetLogger();
            logger.GroupByOutcome.UpdateTestCaseStatus = (from, to, testcase) =>
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    from.RemoveTestCase(testcase);
                    to.AddTestCase(testcase);
                }));
            };
            logger.GroupByOutcome.UpdateTestCaseList = (group, runningcase) =>
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    int index = group.TestCaseList.IndexOf(runningcase);
                    if (index > 0)
                    {
                        group.TestCaseList.Move(index, 0);
                    }
                }));
            };
            Pages.RunPage.SetLogger(logger);
        }

        private void RunTest()
        {
            util.SaveLastProfile();
            Pages.RunPage.TotalResults.Visibility = System.Windows.Visibility.Hidden;
            Pages.RunPage.TotalResultsLinkText.Text = "";
            this.ButtonPrevious.IsEnabled = false;
            this.ButtonNext.IsEnabled = false;
            this.ListBox_Step.IsEnabled = false;
            Pages.RunPage.EnableControls = false;
            util.RunAllTestCases();
        }

        private void RunTestCaseClicked(List<TestCase> testcases)
        {
            if (testcases == null || testcases.Count == 0)
            {
                UserPromptWindow.Show(StringResources.Warning, StringResources.NoTestCaseSelected, UserPromptWindow.IconType.Warning);

                return;
            }
            util.SaveLastProfile();
            Pages.RunPage.TotalResults.Visibility = System.Windows.Visibility.Hidden;
            Pages.RunPage.TotalResultsLinkText.Text = "";
            SetButtonsStatus(false, false);
            this.ListBox_Step.IsEnabled = false;
            Pages.RunPage.EnableControls = false;
            util.RunByCases(testcases);
        }

        private void LoadProfile()
        {
            try
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Win32.OpenFileDialog();
                openFileDialog.Filter = StringResources.TestProfileFilter;
                string initialDir = System.IO.Path.Combine(util.AppConfig.AppDataDirectory, StringResources.TestProfileFolder);
                if (Directory.Exists(initialDir)) openFileDialog.InitialDirectory = initialDir;
                if (openFileDialog.ShowDialog() != true) return;

                string profile = openFileDialog.FileName, newProfile;
                if (util.TryUpgradeProfileSettings(profile, out newProfile))
                {
                    UserPromptWindow.Show(StringResources.Information, String.Format(StringResources.PtmProfileUpgraded, newProfile), UserPromptWindow.IconType.Information);

                    profile = newProfile;
                }
                LoadProfile(profile);
            }
            catch (Exception e)
            {
                UserPromptWindow.Show(StringResources.Error, e.Message, UserPromptWindow.IconType.Error);

                return;
            }
        }

        private void LoadProfile(string filename)
        {
            detectionFinished = false;
            util.LoadProfileSettings(filename);
            util.LoadPtfconfig();
            UpdateCaseFilter();
            GotoConfigPage();
            ((ListBoxItem)ListBox_Step.Items[welcomeIndex]).IsEnabled = true;
            ((ListBoxItem)ListBox_Step.Items[configMethodIndex]).IsEnabled = true;
            ((ListBoxItem)ListBox_Step.Items[ruleIndex]).IsEnabled = true;
            ((ListBoxItem)ListBox_Step.Items[configIndex]).IsEnabled = true;
            ((ListBoxItem)ListBox_Step.Items[runIndex]).IsEnabled = true;
            if (enableConfigureAdapter)
            {
                ((ListBoxItem)ListBox_Step.Items[adapterIndex]).IsEnabled = true;
                GotoAdapterPage();
            }
            else
            {
                ((ListBoxItem)ListBox_Step.Items[adapterIndex]).IsEnabled = false;
            }
            ApplyPtfConfigChanges();
            GotoRunPage();
            DisableDetectSteps();
            ListBox_Step.SelectedIndex = runIndex;
        }

        public MainWindow()
        {
            InitializeComponent();
            Pages.Initialize(this, util);
            RegisterEvents();
            Pages.TestSuiteWindow.SetIntroduction(util.TestSuiteIntroduction);
            SetButtonsVisibility(false, false);
            ListBox_Step.SelectedIndex = testSuiteSelectionIndex;
            DisableFollowingItems();
            ContentFrame.Navigate(Pages.TestSuiteWindow);

        }
        #region Navigation
        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            int nextpage = ListBox_Step.SelectedIndex;
            switch (ListBox_Step.SelectedIndex)
            {
                case testSuiteSelectionIndex:
                    nextpage = welcomeIndex;
                    break;
                case welcomeIndex:
                    nextpage = configMethodIndex;
                    break;
                case configMethodIndex:
                    break;
                case detectionIndex:
                    if (detectionFinished)
                    {
                        DetectionFinished();
                        nextpage = resultIndex;
                    }
                    else if (detectionRunning)
                    {
                        //stop detect
                        SetButtonsStatus(false, false);
                        detectionFinished = false;

                        Task.Factory.StartNew(() =>
                        {
                            util.StopDetection(() =>
                            {
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    detectionRunning = false;
                                    SetButtonsStatus(true, true);
                                    this.ButtonNext.Content = StringResources.DetectButton;
                                    //Set control enabled
                                    Pages.AutoDetectionPage.PropertyListBox.IsEnabled = true;
                                }));
                            });
                        });
                    }
                    else
                        BeginDetection();
                    break;
                case resultIndex:
                    UpdateCaseFilter();
                    nextpage = ruleIndex;
                    break;
                case ruleIndex:
                    GotoConfigPage();
                    nextpage = configIndex;
                    break;
                case configIndex:
                    if (enableConfigureAdapter)
                    {
                        GotoAdapterPage();
                        nextpage = adapterIndex;
                    }
                    else
                    {
                        ApplyPtfConfigChanges();
                        GotoRunPage();
                        nextpage = runIndex;
                    }
                    break;
                case adapterIndex:
                    ApplyPtfConfigChanges();
                    GotoRunPage();
                    nextpage = runIndex;
                    break;
                case runIndex:
                    RunTest();
                    break;
                default:
                    break;
            }
            ListBox_Step.SelectedIndex = nextpage;
            DisableFollowingItems();
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonNext.Content = StringResources.NextButton;

            for (int i = ListBox_Step.SelectedIndex - 1; i >= 0; i--)
            {
                ListBoxItem lbi = (ListBoxItem)ListBox_Step.Items[i];
                if (lbi.IsEnabled)
                {
                    ListBox_Step.SelectedIndex = i;
                    break;
                }
            }
            if (ListBox_Step.SelectedIndex == 0)
            {
                SetButtonsStatus(false, false);
            }
        }

        private void ListBox_Step_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox stepSelection = sender as ListBox;
            if (stepSelection.SelectedIndex != detectionIndex
                && stepSelection.SelectedIndex != runIndex)
            {
                this.ButtonNext.Content = StringResources.NextButton;
            }
        }

        private void Item_Welcome_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(Pages.WelcomePage);
            PageInfoTextBlock.Text = StringResources.TestSuiteIntroduction;
            SetButtonsVisibility(true, true);
            SetButtonsStatus(true, true);
        }

        private void Item_Configure_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(Pages.ConfigPage);
            PageInfoTextBlock.Text = StringResources.ConfigureTestCase;
            SetButtonsVisibility(true, true);
            SetButtonsStatus(true, true);
        }

        private void Item_TestSuiteSelection_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(Pages.TestSuiteWindow);
            PageInfoTextBlock.Text = StringResources.SelectTestSuite;
            SetButtonsVisibility(false, false);
            SetButtonsStatus(false, false);
        }

        private void Item_DetectedSUT_Selected(object sender, RoutedEventArgs e)
        {
            detectionRunning = false;
            ContentFrame.Navigate(Pages.SUTInfoPage);
            PageInfoTextBlock.Text = StringResources.DetectionResult;
            SetButtonsVisibility(true, true);
            SetButtonsStatus(true, true);
        }

        private void Item_TestCase_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(Pages.RulePage);
            PageInfoTextBlock.Text = StringResources.FilterTestCases;
            SetButtonsVisibility(true, true);
            SetButtonsStatus(true, IsCaseSeleted());
        }

        private void Item_Run_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(Pages.RunPage);
            PageInfoTextBlock.Text = StringResources.RunSelectedTestCases;
            ButtonNext.Content = StringResources.NextButton;
            SetButtonsVisibility(true, true);
            SetButtonsStatus(true, false);
        }

        private void Item_Confirm_Auto_Detection_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(Pages.ConfigMethodPage);
            PageInfoTextBlock.Text = StringResources.ChooseConfigMethod;
            SetButtonsVisibility(true, true);
            SetButtonsStatus(true, false);
        }

        private void Item_Prerequisite_Selected(object sender, RoutedEventArgs e)
        {
            detectionFinished = false;
            this.ButtonNext.Content = StringResources.DetectButton;
            ContentFrame.Navigate(Pages.AutoDetectionPage);
            PageInfoTextBlock.Text = StringResources.Autodetection;
            SetButtonsVisibility(true, true);
            SetButtonsStatus(true, true);
        }

        private void Item_Adapter_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(Pages.AdapterPage);
            PageInfoTextBlock.Text = StringResources.ConfigureAdapters;
            SetButtonsVisibility(true, true);
            SetButtonsStatus(true, true);
        }

        private void SetButtonsStatus(bool previousButtonStatus, bool nextButtonStatus)
        {
            this.ButtonPrevious.IsEnabled = previousButtonStatus;
            this.ButtonPrevious.Focusable = previousButtonStatus;
            this.ButtonNext.IsEnabled = nextButtonStatus;
            this.ButtonNext.Focusable = nextButtonStatus;
        }

        private void SetButtonsVisibility(bool previousButtonStatus, bool nextButtonStatus)
        {
            if (previousButtonStatus)
                this.ButtonPrevious.Visibility = Visibility.Visible;
            else
                this.ButtonPrevious.Visibility = Visibility.Hidden;
            if (nextButtonStatus)
                this.ButtonNext.Visibility = Visibility.Visible;
            else
                this.ButtonNext.Visibility = Visibility.Hidden;
        }

        private void DisableFollowingItems()
        {
            int selectedIndex = ListBox_Step.SelectedIndex;
            ListBoxItem selectedItem = (ListBoxItem)ListBox_Step.Items[selectedIndex];
            selectedItem.IsEnabled = true;
            for (int i = selectedIndex + 1; i <= maxSelectedIndex; i++)
            {
                ListBoxItem lbi = (ListBoxItem)ListBox_Step.Items[i];
                lbi.IsEnabled = false;
            }
        }
        private void DisableDetectSteps()
        {
            ((ListBoxItem)ListBox_Step.Items[detectionIndex]).IsEnabled = false;
            ((ListBoxItem)ListBox_Step.Items[resultIndex]).IsEnabled = false;
        }
        private void EnableFollowingItems()
        {
            int selectedIndex = ListBox_Step.SelectedIndex;
            ListBoxItem selectedItem = (ListBoxItem)ListBox_Step.Items[selectedIndex];
            selectedItem.IsEnabled = true;
            for (int i = selectedIndex + 1; i <= maxSelectedIndex; i++)
            {
                ListBoxItem lbi = (ListBoxItem)ListBox_Step.Items[i];
                lbi.IsEnabled = true;
            }
        }

        private void DisableUnusedProcess(int StartPageIndex)
        {
            int selectedIndex = ListBox_Step.SelectedIndex;
            for (int i = StartPageIndex; i < selectedIndex; i++)
            {
                ListBoxItem lbi = (ListBoxItem)ListBox_Step.Items[i];
                lbi.IsEnabled = false;
            }
        }

        private void DisableUnusedProcess(int StartPageIndex, int EndPageIndex)
        {
            int selectedIndex = ListBox_Step.SelectedIndex;
            for (int i = StartPageIndex; i < EndPageIndex; i++)
            {
                ListBoxItem lbi = (ListBoxItem)ListBox_Step.Items[i];
                lbi.IsEnabled = false;
            }
            for (int i = EndPageIndex; i <= selectedIndex; i++)
            {
                ListBoxItem lbi = (ListBoxItem)ListBox_Step.Items[i];
                lbi.IsEnabled = true;
            }
        }

        #endregion

        private void ListBox_Step_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F6)
            {
                e.Handled = true;
                Queue<DependencyObject> objqueue = new Queue<DependencyObject>();
                objqueue.Enqueue(ContentFrame.Content as DependencyObject);

                while (objqueue.Count > 0)
                {
                    var o = objqueue.Dequeue();
                    IInputElement ele = o as IInputElement;
                    if (ele.Focusable)
                    {
                        ele.Focus();
                        break;
                    }
                    foreach (Object child in LogicalTreeHelper.GetChildren(o))
                    {
                        var childobj = child as DependencyObject;
                        if (childobj != null) objqueue.Enqueue(childobj);
                    }
                }

            }
        }

        private void ContentFrame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F6)
            {
                e.Handled = true;
                if (ButtonPrevious.Focusable) ButtonPrevious.Focus();
                else if (ButtonNext.Focusable) ButtonNext.Focus();
                else FocusListBox();
            }
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F6)
            {
                FocusListBox();
                e.Handled = true;
            }
        }

        private void FocusListBox()
        {
            if (ListBox_Step.SelectedItem != null)
            {
                ((IInputElement)ListBox_Step.SelectedItem).Focus();
            }
            else
            {
                ((IInputElement)ListBox_Step.Items[0]).Focus();
            }
        }

    }
}
