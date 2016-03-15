// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc.Utility
{
    internal class ParamValidator
    {
        internal static void NotNull(object o, string paramName)
        {
            if (null == o)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        internal static void NotNullOrEmpty(Array a, string paramName)
        {
            if (null == a)
            {
                throw new ArgumentNullException(paramName);
            }

            if (a.Length <= 0)
            {
                throw new ArgumentException(String.Format("The length of {0} must be larger than 0.", paramName));
            }
        }
    }

    internal class LogHelper
    {
        internal static string ByteArrayToString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

            // TODO: Check performance and results for this function.
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0}", b.ToString("X"));
            }

            return sb.ToString();
        }
    }
}
