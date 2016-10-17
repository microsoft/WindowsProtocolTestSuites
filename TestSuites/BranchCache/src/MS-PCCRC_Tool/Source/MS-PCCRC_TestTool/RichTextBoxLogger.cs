// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MS_PCCRC_TestTool
{
    public class RichTextBoxLogger
    {
        private RichTextBox richTextBox;
        private int indent;
        private bool hasError;

        public bool HasError
        {
            get
            {
                return hasError;
            }
        }

        public RichTextBoxLogger(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox;
        }

        public void Indent()
        {
            indent++;
        }

        public void Unindent()
        {
            indent--;
        }

        public void Clear()
        {
            richTextBox.Clear();
            indent = 0;
            hasError = false;
        }

        public void LogInfo(string text, params object[] parameters)
        {
            Log(text, Color.Black, parameters);
        }

        public void LogError(string text, params object[] parameters)
        {
            Log(text, Color.Red, parameters);
            hasError = true;
        }

        public void Log(string text, Color color, params object[] parameters)
        {
            string padding = new string(' ', indent * 2);
            string log = padding + string.Format(text, parameters);
            int offset = richTextBox.TextLength;
            int length = log.Length;
            richTextBox.AppendText(log + "\r\n");
            richTextBox.SelectionStart = offset;
            richTextBox.SelectionLength = length;
            richTextBox.SelectionColor = color;
        }

        public void NewLine()
        {
            richTextBox.AppendText("\r\n");
        }
    }
}
