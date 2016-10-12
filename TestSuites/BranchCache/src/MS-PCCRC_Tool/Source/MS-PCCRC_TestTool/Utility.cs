// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MS_PCCRC_TestTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Net;
    using System.Drawing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Transfer a byte array to a hex string. 
        /// Example from a byte array { 0x0a, 0x0b, 0x0c }, 
        /// the return is a string 0A0B0C.
        /// </summary>
        /// <param name="data">The input byte array</param>
        /// <param name="start">The start index.</param>
        /// <param name="length">The data length of convert to string.</param>
        /// <returns>Hex String from the buffer</returns>
        public static string ToHexString(byte[] data, int start, int length)
        {
            StringBuilder oRet = new StringBuilder();
            string tempString = string.Empty;

            if (data == null)
            {
                return string.Empty;
            }
            if (length == 0)
            {
                length = data.Length;
            }
            for (int i = start; i < start + length; i++)
            {
                oRet.Append(data[i].ToString("X2"));
            }
            return oRet.ToString();
        }

        /// <summary>
        /// Transfer a byte array to a hex string. 
        /// Example from a byte array { 0x0a, 0x0b, 0x0c }, 
        /// the return is a string 0A0B0C.
        /// </summary>
        /// <param name="data">The input byte array</param>
        /// <returns>Hex String from the byte array</returns>
        public static string ToHexString(byte[] data)
        {
            string hexStr = string.Empty;
            if (data != null)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("X2"));
                }
                hexStr = builder.ToString();
            }
            return hexStr;
        }

        /// <summary>
        /// Show message box.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="icon"></param>
        public static void ShowMessageBox(string msg, MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
                case MessageBoxIcon.Information:
                    MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
                case MessageBoxIcon.Warning:
                    MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
            }
        }

        public static byte[] DownloadHTTPFile(string server, string file)
        {
            WebClient client = new WebClient();
            return client.DownloadData(string.Format("http://{0}/{1}", server, file));
        }
    }
}
