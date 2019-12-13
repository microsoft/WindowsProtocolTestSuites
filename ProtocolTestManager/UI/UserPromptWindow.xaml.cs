// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.Protocols.TestManager.UI
{
    public partial class UserPromptWindow : Window
    {
        private UserPromptWindow()
        {
            InitializeComponent();
        }

        private void GenerateContent(string contentString)
        {
            // Replace text surrounded by square brackets with link.

            string pattern = @"(\[[^\[\]]*\])";

            var parts = Regex.Split(contentString, pattern);

            var children = parts.Select<string, Inline>(part =>
            {
                bool isLink = Regex.IsMatch(part, pattern);

                if (isLink)
                {
                    var link = new Hyperlink();

                    var target = part.Substring(1, part.Length - 2);

                    link.Inlines.Add(new Run(target));

                    link.NavigateUri = new Uri(target);

                    link.Click += Link_Click;

                    return link;
                }
                else
                {
                    return new Run(part);
                }
            });

            content.Inlines.AddRange(children);
        }

        private void Link_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink link)
            {
                ShellOpen(link.NavigateUri.ToString());
            }
        }

        private static ImageSource GetIcon(Icon icon)
        {
            var bitmap = icon.ToBitmap();

            var hBitmap = bitmap.GetHbitmap();

            var result = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            return result;
        }

        private void ShellOpen(string path)
        {
            var startInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = path,
            };

            var process = new Process()
            {
                StartInfo = startInfo,
            };

            process.Start();
        }

        public enum IconType
        {
            Error,
            Warning,
            Information,
        }

        public static void Show(string title, string content, IconType iconType)
        {
            var window = new UserPromptWindow();

            window.Title = title;

            window.GenerateContent(content);

            var iconMap = new Dictionary<IconType, Icon>
            {
                [IconType.Error] = SystemIcons.Error,
                [IconType.Warning] = SystemIcons.Warning,
                [IconType.Information] = SystemIcons.Information,
            };

            window.icon.Source = GetIcon(iconMap[iconType]);

            window.button.Click += (s, e) =>
            {
                window.Close();
            };

            window.Owner = App.Current.MainWindow;

            window.ShowDialog();
        }
    }
}
