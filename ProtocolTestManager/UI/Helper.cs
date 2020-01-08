// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;

namespace Microsoft.Protocols.TestManager.UI
{
    public class Helper
    {
        /// <summary>
        /// Generate Inline objects from source with simple rules.
        /// Rules:
        ///   1. Replace text surrounded by square brackets with link.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The Inline objects generated.</returns>
        public static IEnumerable<Inline> GenerateInlines(string source)
        {
            // Replace text surrounded by square brackets with link.

            string pattern = @"(\[[^\[\]]*\])";

            var parts = Regex.Split(source, pattern);

            var results = parts.Select<string, Inline>(part =>
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

            return results;
        }

        private static void Link_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink link)
            {
                ShellOpen(link.NavigateUri.ToString());
            }
        }

        private static void ShellOpen(string path)
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
    }
}
