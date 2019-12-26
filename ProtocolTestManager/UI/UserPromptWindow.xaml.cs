// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Drawing;
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

        private void GenerateContent(string contentString, bool hasLinks)
        {
            if (hasLinks)
            {
                var children = Helper.GenerateInlines(contentString);

                content.Inlines.AddRange(children);
            }
            else
            {
                content.Inlines.Add(new Run(contentString));
            }
        }

        private static ImageSource GetIcon(Icon icon)
        {
            var bitmap = icon.ToBitmap();

            var hBitmap = bitmap.GetHbitmap();

            var result = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            return result;
        }

        public enum IconType
        {
            Error,
            Warning,
            Information,
        }

        public static void Show(string title, string text, IconType iconType)
        {
            ShowInternal(title, text, false, iconType);
        }

        public static void ShowWithLinks(string title, string source, IconType iconType)
        {
            ShowInternal(title, source, true, iconType);
        }

        private static void ShowInternal(string title, string content, bool hasLinks, IconType iconType)
        {
            var window = new UserPromptWindow();

            window.Title = title;

            window.GenerateContent(content, hasLinks);

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
