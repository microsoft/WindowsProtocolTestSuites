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
using System.Reflection;
using System.IO;
using System.Windows.Media.Animation;
using Microsoft.Protocols.TestManager.Kernel;
using System.Windows.Controls.Primitives;

namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for ConfigPage.xaml
    /// </summary>
    public partial class ConfigPage : Page
    {
        private static int NameCollumnWidth = 200;

        public ConfigPage()
        {
            this.InitializeComponent();

        }
        List<ItemsControl> panels = null;
        public void BindItem(PtfPropertyView propertyView)
        {
            if (panels != null)
            {
                //Remove old ItemsControl
                foreach (UIElement i in LayoutRoot.Children)
                {
                    var itemsControl = i as ItemsControl;
                    if (itemsControl != null && itemsControl.Tag is string && (string)itemsControl.Tag == "PropertyPanel")
                    {
                        LayoutRoot.Children.Remove(i);
                        break;
                    }
                }

            }
            panels = new List<ItemsControl>();
            GroupList.ItemsSource = propertyView;
            foreach (var p in propertyView)
            {
                panels.Add(CreatePropertyView(p));
            }
            LayoutRoot.Children.Add(panels[0]);
            current = panels[0];
            GroupList.SelectedIndex = 0;
        }

        private ItemsControl CreatePropertyView(PtfPropertyView propertyView)
        {
            var template = FindResource("ItemsControlTemplate") as ControlTemplate;
            ItemsControl itemsControl = new ItemsControl()
            {
                Tag = "PropertyPanel",
                Margin = new Thickness(170, 21, 0, 0),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                Template = template
            };
            foreach (var item in propertyView)
            {
                if (item.ControlType == Microsoft.Protocols.TestManager.Kernel.ControlType.Group)
                {
                    ExpandGroup eg = new ExpandGroup();
                    eg.Name = item.Name;
                    foreach (var i in item)
                    {
                        eg.AddItem(CreateItem(i, 14));
                    }
                    eg.Associate(itemsControl);
                }
                else
                {
                    itemsControl.Items.Add(CreateItem(item)); 
                }
            }

            return itemsControl;
        }



        private Brush GetBgBrush(bool isHighlight)
        {
            System.Drawing.Color hl = System.Drawing.SystemColors.Highlight;
            System.Drawing.Color nothl = System.Drawing.SystemColors.Window;
            if (isHighlight)
                return new SolidColorBrush(Color.FromArgb(hl.A, hl.R, hl.G, hl.B));
            else
                return new SolidColorBrush(Color.FromArgb(nothl.A, nothl.R, nothl.G, nothl.B));
        }

        private Brush GetFgBrush(bool isHighlight)
        {
            System.Drawing.Color hl = System.Drawing.SystemColors.HighlightText;
            System.Drawing.Color nothl = System.Drawing.SystemColors.ControlText;
            if (isHighlight)
                return new SolidColorBrush(Color.FromArgb(hl.A, hl.R, hl.G, hl.B));
            else
                return new SolidColorBrush(Color.FromArgb(nothl.A, nothl.R, nothl.G, nothl.B));
        }

        private Grid CreateItem(PtfPropertyView propertyView, int indent = 0)
        {
            UIElement content = new Label() { Content = "Unknown" };
            Brush bgbrush = GetBgBrush(propertyView.Value != propertyView.DefaultValue);
            Brush fgbrush = GetFgBrush(propertyView.Value != propertyView.DefaultValue);
            Label label = new Label()
            {
                Margin = new Thickness(7 + indent, 0, 0, 0),
                Content = propertyView.Name + (propertyView.Value == propertyView.DefaultValue ? "" : " *"),
                Width = NameCollumnWidth - indent,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Background = bgbrush,
                Foreground = fgbrush
            };
            switch (propertyView.ControlType)
            {
                case ControlType.Text:
                    var textBox = new TextBox()
                    {
                        Margin = new Thickness(NameCollumnWidth + 7, 0, 7, 0),
                        Width = double.NaN,
                        Text = propertyView.Value,
                    };
                    var contextnemu = new TextBoxContextMenu();
                    contextnemu.UseDefaultValueClicked += (sender, arg) =>
                    {
                        textBox.Text = propertyView.DefaultValue;
                    };
                    textBox.ContextMenu = contextnemu;
                    textBox.TextChanged += (sender, arg) =>
                        {
                            propertyView.Value = textBox.Text;
                        };
                    content = textBox;
                    break;
                case ControlType.Password:
                    var passwdTextBox = new TextBox()
                    {
                        Margin = new Thickness(NameCollumnWidth + 7, 0, 7, 0),
                        Width = double.NaN,
                        Text = propertyView.Value
                    };
                    passwdTextBox.TextChanged += (sender, arg) =>
                    {
                        propertyView.Value = passwdTextBox.Text;
                    };
                    content = passwdTextBox;
                    break;
                case ControlType.Choice:
                    var comboBox = new ComboBox()
                    {
                        Margin = new Thickness(NameCollumnWidth + 7, 0, 7, 0),
                        Width = double.NaN,
                        IsEditable = true
                    };
                    comboBox.SetBinding(ComboBox.ItemsSourceProperty, "ChoiceItems");
                    comboBox.SetBinding(ComboBox.TextProperty, "Value");
                    comboBox.DataContext = propertyView;
                    content = comboBox;
                    break;
            }
            propertyView.PropertyChanged += (sender, arg) =>
            {
                bool modified = propertyView.Value != propertyView.DefaultValue;
                label.Content = propertyView.Name + (modified ? " *" : "");
                label.Background = GetBgBrush(modified);
                label.Foreground = GetFgBrush(modified);
            };
            Grid grid = new Grid()
                {
                    Margin = new Thickness(0, 1, 0, 1),
                    Children =
                    {
                        label,
                        content
                    },
                    ToolTip = propertyView.ToolTip + Environment.NewLine + propertyView.Description
                };

            return grid;
        }

        private ItemsControl current;
        private void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int n = GroupList.SelectedIndex;
            if (n >= 0 && n < panels.Count)
            {
                LayoutRoot.Children.Remove(current);
                LayoutRoot.Children.Add(panels[n]);
                current = panels[n];
            }
        }


    }

    public class ExpandGroup
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                expander.Header = value;

            }
        }

        private bool previewExpanded;
        public bool IsExpanded
        {
            get { return expander.IsExpanded; }
            set
            {
                expander.IsExpanded = value;
            }
        }
        public ExpandGroup()
        {
            expander = new Expander()
            {
                FontWeight = FontWeights.Bold
            };
            expander.Expanded += expander_Expanded;
            expander.Collapsed += expander_Collapsed;
            itemsCtrl = null;
            items = new List<Grid>();
        }

        void expander_Collapsed(object sender, RoutedEventArgs e)
        {
            if (itemsCtrl == null || !previewExpanded) return;
            int index = itemsCtrl.Items.IndexOf(expander);
            for (int i = 0; i < items.Count; i++)
            {
                itemsCtrl.Items.RemoveAt(index + 1);
            }
            previewExpanded = false;
        }

        void expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (itemsCtrl == null || previewExpanded) return;
            int index = itemsCtrl.Items.IndexOf(expander);
            foreach (var item in items)
            {
                itemsCtrl.Items.Insert(index + 1, item);
            }
            previewExpanded = true;
        }
        private Expander expander;
        private ItemsControl itemsCtrl;
        public void Associate(ItemsControl itemsControl)
        {
            itemsCtrl = itemsControl;
            itemsCtrl.Items.Add(expander);
            expander.IsExpanded = true;
        }

        private List<Grid> items;
        public void AddItem(Grid stackPanel)
        {
            items.Add(stackPanel);
            if (IsExpanded)
            {
                int index = itemsCtrl.Items.IndexOf(expander);
                itemsCtrl.Items.Insert(index + items.Count, stackPanel);
            }
        }
    }


    class TextBoxContextMenu : ContextMenu
    {
        private MenuItem useDefaultValueMenuItem;
        public TextBoxContextMenu()
        {
            MenuItem mi = new MenuItem() { Command = ApplicationCommands.Cut };
            this.Items.Add(mi);
            mi = new MenuItem() { Command = ApplicationCommands.Copy };
            this.Items.Add(mi);
            mi = new MenuItem() { Command = ApplicationCommands.Paste };
            this.Items.Add(mi);
            mi = new MenuItem() { Command = ApplicationCommands.Undo };
            this.Items.Add(mi); 
            mi = new MenuItem() { Command = ApplicationCommands.SelectAll };
            this.Items.Add(mi);
            this.Items.Add(new Separator());
            useDefaultValueMenuItem = new MenuItem() { Header = StringResources.DefaultValue };
            this.Items.Add(useDefaultValueMenuItem);

        }

        public event RoutedEventHandler UseDefaultValueClicked
        {
            add
            {
                useDefaultValueMenuItem.Click += value;
            }
            remove
            {
                useDefaultValueMenuItem.Click -= value;
            }
        }


    }

 
}
