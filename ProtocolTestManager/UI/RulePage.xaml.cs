// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Kernel;
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
namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for RulePage.xaml
    /// </summary>
    public partial class RulePage : Page
    {
		public RulePage()
		{
			InitializeComponent();
           
		}

        public void SetFilter(TestCaseFilter filter)
        {
            RuleList.Items.Clear();
            foreach (var group in filter)
            {
                TreeView ruleTree = new TreeView();
                CheckBox selectAll = new CheckBox()
                {
                    Content = "(Select All)",
                    IsChecked = group.IsSelected,
                    Focusable = false,
                    Tag = group
                };
                selectAll.Checked += (sender, arg) =>
                {
                    if (group.IsSelected != true) group.IsSelected = true;
                };
                selectAll.Unchecked += (sender, arg) =>
                {
                    if (group.IsSelected != false) group.IsSelected = false;
                };
                group.PropertyChanged += (sender, arg) =>
                {
                    if (arg.PropertyName == "IsSelected")
                    {
                        selectAll.IsChecked = group.IsSelected;
                    }
                };

                ruleTree.Items.Add(selectAll);

                AddItems(ruleTree.Items, group);
                Expander expander = new Expander()
                {
                    Header = group.Name,
                    Content = ruleTree,
                    IsExpanded = true
                };
                RuleList.Items.Add(expander);
                ruleTree.KeyDown += (sender, arg) =>
                {
                    if (arg.Key != Key.Space) return;
                    var tv = sender as TreeView;
                    if (tv == null) return;
                    ToggleTreeview(tv);
                };
                ruleTree.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
                ruleTree.MouseDoubleClick += (sender, arg) =>
                {
                    if (arg.ChangedButton != MouseButton.Left) return;
                    var tv = sender as TreeView;
                    if (tv == null) return;
                    ToggleTreeview(tv);
                };
            }
        }


        private void ToggleTreeview(TreeView tree)
        {
            var item = tree.SelectedItem as FrameworkElement;
            if (item == null) return;
            var rule = item.Tag as Rule;
            if (rule != null)
            {
                rule.IsSelected = !rule.IsSelected;
                return;
            }
            var group = item.Tag as RuleGroup;
            if (group != null)
            {
                group.IsSelected = !group.IsSelected;
            }
        }

        private void AddItems(ItemCollection parent, List<Rule> rules)
        {
            foreach (var rule in rules)
            {
                if (rule.Count > 0)
                {
                    TreeViewItem treeViewItem = new TreeViewItem()
                    {
                        Header = CreateRuleItem(rule),
                        IsExpanded = true,
                        Tag = rule
                    };
                    AddItems(treeViewItem.Items, rule);
                    parent.Add(treeViewItem);
                }
                else
                {
                    StackPanel panel = CreateRuleItem(rule);
                    panel.Tag = rule;
                    parent.Add(panel);
                }
            }
        }

        private StackPanel CreateRuleItem(Rule rule)
        {
            StackPanel panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            CheckBox checkbox = new CheckBox()
            {
                Focusable = false,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                IsChecked = rule.IsSelected
            };
            checkbox.Checked += (sender, arg) =>
            {
                if (rule.IsSelected != true) rule.IsSelected = true;
            };
            checkbox.Unchecked += (sender, arg) =>
            {
                if (rule.IsSelected != false) rule.IsSelected = false;
            };
            rule.PropertyChanged += (sender, arg) =>
            {
                if (arg.PropertyName == "IsSelected")
                {
                    checkbox.IsChecked = rule.IsSelected;
                }
            };
            panel.Children.Add(checkbox);
            Label label = new Label()
                {
                    Content = rule.Name,
                    Focusable = false,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center
                };
            if (rule.Status == RuleSupportStatus.NotSupported)
            {
                label.FontStyle = FontStyles.Italic;
                label.ToolTip = StringResources.FeatureNotSupported;
            }
            panel.Children.Add(label);
            return panel;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }

	}
}
