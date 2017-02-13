// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using KentCe.Sample;
using Microsoft.Protocols.TestTools;
using System.Globalization;

namespace Microsoft.Protocol.TestSuites.ADOD.Adapter.Util
{
    /// <summary>
    /// Telnet client to communicate with telnet server
    /// </summary>
    public class TelnetClient
    {
        private TelnetLineStream stream;
        private ITestSite testSite;
        public TelnetClient(string ip, int port, string user, string password)
        {
            TcpClient client = new TcpClient(ip, port);
            this.stream = new TelnetLineStream(client.GetStream());
            this.testSite = TestClassBase.BaseTestSite;
            Login(user, password);
        }

        /// <summary>
        /// Login to telnet server with given username and password
        /// </summary>
        /// <param name="loginUser">Specifies the login user name.</param>
        /// <param name="password">Specifies the password of user.</param>
        public void Login(string loginUser, string password)
        {
            StringBuilder sb = new StringBuilder();
            string response = ReadResponse();
            sb.Append(response);
            WriteCommand(loginUser);
            response = ReadResponse();
            sb.Append(response);
            WriteCommand(password);
            response = ReadResponse();
            sb.Append(response);
        }

        /// <summary>
        /// Read response output from server
        /// </summary>
        /// <returns>Return redirected output</returns>
        public string ReadResponse()
        {
            byte[] buffer = new byte[1024];
            int readBytes;
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                readBytes = stream.Read(buffer, 0, buffer.Length);
                for (int i = 0; i < readBytes; i++)
                {
                    sb.Append((char)buffer[i]);
                }

                //End char for telnet command
                char last = (char)buffer[readBytes - 2];
                if (last == '#' || last == '>' || last == '$' || last == ':')
                    break;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Used for functions that only returns true or false
        /// </summary>
        /// <param name="response">Specifies the telnet response collected from the immediate request.</param>
        /// <returns>true/false</returns>
        public bool CheckResponse(string response)
        {
            string splitter = "\r\n";
            int start = response.IndexOf(splitter, StringComparison.OrdinalIgnoreCase) + splitter.Length;
            int stop = response.LastIndexOf(splitter, StringComparison.OrdinalIgnoreCase);
            if (start >= stop)
            {
                return false;
            }
            else
            {
                string result = response.Substring(start, stop - start);
                if (result.ToLower(CultureInfo.InvariantCulture).Contains("true"))
                {
                    return true;
                }
                if (result.ToLower(CultureInfo.InvariantCulture).Contains("false"))
                {
                    return false;
                }
                return false;
            }
        }

        /// <summary>
        /// Send a command to telnet server
        /// </summary>
        /// <param name="str">Specifies the command string.</param>
        public void WriteCommand(string str)
        {
            stream.Write(ConvertStringToTelnetBytes(str), 0, str.Length + 1);
        }

        /// <summary>
        /// Convert command string to byte array, add \n(char 13) at the end of the command
        /// </summary>
        /// <param name="str">Specifies the string to be converted.</param>
        private byte[] ConvertStringToTelnetBytes(string str)
        {
            byte[] bytes = new byte[str.Length + 1];
            for (int i = 0; i < str.Length; i++)
            {
                bytes[i] = (byte)str[i];
            }
            //13 represent press "enter"
            bytes[str.Length] = 13;
            return bytes;
        }
    }
}
