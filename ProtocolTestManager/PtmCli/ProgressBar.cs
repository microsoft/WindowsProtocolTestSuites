// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Text;
using System.Threading;

namespace Microsoft.Protocols.TestManager.CLI
{
    class ProgressBar: IDisposable
    {
        private readonly TimeSpan animationInterval = TimeSpan.FromSeconds(1.0 / 8);
        private const string animation = @"|/-\";

        private readonly Timer timer;

        private string nextText = "";
        private string currentText = "";
        private int animationIndex = 0;
        private bool disposed = false;

        public ProgressBar()
        {
            timer = new Timer(TimerHandler);

            // A progress bar is only for temporary display in a console window.
            // If the console output is redirected to a file, draw nothing.
            // Otherwise, we'll end up with a lot of garbage in the target file.
            if (!Console.IsOutputRedirected)
            {
                EnableTimer();
            }
        }

        public void Update(double value, string step)
        {
            value = Math.Max(0, Math.Min(1, value)); // Make sure value is in [0..1] range
            int percent = (int)(value * 100);
            string next = string.Format("{0,3}% {{0}} {1}", percent, step);
            Interlocked.Exchange(ref nextText, next);
        }

        private void TimerHandler(object state)
        {
            lock (timer)
            {
                if (disposed)
                {
                    return;
                }

                animationIndex = (animationIndex + 1) % animation.Length;
                string text = string.Format(nextText,animation[animationIndex]);
                UpdateText(text);
            }
        }

        private void UpdateText(string text)
        {
            // Get length of common portion
            int commonPrefixLength = 0;
            int commonLength = Math.Min(currentText.Length, text.Length);
            while (commonPrefixLength < commonLength && text[commonPrefixLength] == currentText[commonPrefixLength])
            {
                commonPrefixLength++;
            }

            // Backtrack to the first differing character
            StringBuilder outputBuilder = new StringBuilder();
            outputBuilder.Append('\b', currentText.Length - commonPrefixLength);

            // Output new suffix
            outputBuilder.Append(text.Substring(commonPrefixLength));

            // If the new text is shorter than the old one: delete overlapping characters
            int overlapCount = currentText.Length - text.Length;
            if (overlapCount > 0)
            {
                outputBuilder.Append(' ', overlapCount);
                outputBuilder.Append('\b', overlapCount);
            }

            Console.Write(outputBuilder);
            currentText = text;
        }

        private void EnableTimer()
        {
            timer.Change(new TimeSpan(0), animationInterval);
        }

        public void Dispose()
        {
            lock (timer)
            {
                disposed = true;
                timer.Dispose();
                UpdateText(string.Empty);
            }
        }

    }
}
