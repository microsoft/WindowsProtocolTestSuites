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
using System.Threading;
using Microsoft.Protocols.TestManager.Kernel;
using System.Diagnostics;

namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for TestSuiteWindow.xaml
    /// </summary>
    public partial class TestSuiteWindow : Page
    {
        public TestSuiteFamilies introduction;
        public void SetIntroduction(TestSuiteFamilies testsuiteintro)
        {
            introduction = testsuiteintro;
            StackPanel stack = new StackPanel();
            bool noTestSuiteInstalled = true;
            foreach (TestSuiteFamily family in introduction)
            {
                Grid grid = new Grid();
                GroupBox gTestSuiteFamily = new GroupBox()
                {
                    Header = family.Name,
                    FontSize = 14,
                    Content = grid
                };
                int row = 0;
                
                foreach (TestSuiteInfo info in family)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                    Label name = new Label() { Content = info.TestSuiteName, Margin = new Thickness(7, 2, 0, 2) };
                    grid.Children.Add(name);
                    Grid.SetRow(name, row);
                    Label version = new Label() { Content = info.TestSuiteVersion, HorizontalAlignment = System.Windows.HorizontalAlignment.Right, Margin = new Thickness(0, 2, 200, 2) };
                    grid.Children.Add(version);
                    Grid.SetRow(version, row);
                    if (info.IsInstalled)
                    {
                        noTestSuiteInstalled = false;
                        if (info.IsConfiged)
                        {
                            Run runDirectly = new Run(StringResources.Run);
                            Hyperlink runLink = new Hyperlink(runDirectly);
                            runLink.Click += (sender, arg) =>
                            {
                                if (Run_Click != null) Run_Click(info);
                            };
                            Label runLabel = new Label()
                            {
                                Content = runLink,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                                Margin = new Thickness(0, 2, 150, 2),
                                ToolTip = StringResources.RunToolTip
                            };
                            grid.Children.Add(runLabel);
                            Grid.SetRow(runLabel, row);
                        }
                        Run configWizard = new Run(StringResources.ConfigureWizard);
                        Hyperlink link = new Hyperlink(configWizard);
                        link.Click += (sender, arg) =>
                            {
                                if (Launch_Click != null) Launch_Click(info);
                            };
                        Label cfgLabel = new Label()
                        {
                            Content = link,
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                            Margin = new Thickness(0, 2, 10, 2)
                        };
                        grid.Children.Add(cfgLabel);
                        Grid.SetRow(cfgLabel, row);
                    }
                    row++;
                }
                stack.Children.Add(gTestSuiteFamily);
            }

            if (noTestSuiteInstalled)
            {
                stack.Children.Add(new TextBlock()); // Empty line

                // First sentence
                var hint = new TextBlock()
                {
                    Foreground = Brushes.Red,
                    Text = StringResources.InstallAtLeastOneTestSuite
                };
                stack.Children.Add(hint);

                // Second and the third sentence, they're in the same line.
                hint = new TextBlock();
                hint.Foreground = Brushes.Red;
                hint.Text = StringResources.GetLatestRelease;
                var link = new Hyperlink();
                link.Inlines.Add(StringResources.ThisLink);
                link.NavigateUri = new Uri(StringResources.ReleaseLink);
                link.RequestNavigate += Hyperlink_RequestNavigate;
                hint.Inlines.Add(link);
                hint.Inlines.Add(StringResources.RestartPTM);
                stack.Children.Add(hint);
            }

            ScrollContent.Content = stack;
        }

        public TestSuiteWindow()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        // Fired when clicking the hyper link, it should open the link by the web browser.
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        public delegate void testSuiteLaunched(TestSuiteInfo testSuiteInfo);
        public testSuiteLaunched Launch_Click;
        public testSuiteLaunched Run_Click;

    }
}
