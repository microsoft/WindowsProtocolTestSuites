// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Rfc1035Utility is a utility class of 
    /// RFC 1035: DOMAIN NAMES - IMPLEMENTATION AND SPECIFICATION
    /// </summary>
    public static class Rfc1035Utility
    {
        /// <summary>
        /// In order to reduce the size of messages, the domain system utilizes a
        /// compression scheme which eliminates the repetition of domain names in a
        /// message.  In this scheme, an entire domain name or a list of labels at
        /// the end of a domain name is replaced with a pointer to a prior occurrence
        /// of the same name.
        /// </summary>
        /// <param name="domainName">A domain name. Labels are separated by dot.</param>
        /// <returns>The compressed UTF8 string.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when domainName is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when one label of domainName exceeded max length limit (63 bytes).
        /// </exception>
        public static byte[] ToCompressedUtf8String(string domainName)
        {
            if (domainName == null)
            {
                throw new ArgumentNullException("domainName");
            }

            const int MAX_LABEL_LENGTH = 63;

            List<byte> buf = new List<byte>();
            string[] labels = domainName.Split('.');
            for (int i = 0; i < labels.Length; i++)
            {
                byte[] labelBytes = Encoding.UTF8.GetBytes(labels[i]);
                if (labelBytes.Length > MAX_LABEL_LENGTH)
                {
                    throw new ArgumentException("Label exceeded max length.", "domainName");
                }
                buf.Add((byte)labelBytes.Length);
                buf.AddRange(labelBytes);
            }

            buf.Add(0);

            return buf.ToArray();
        }


        /// <summary>
        /// Convert from a compressed UTF8 string to C# string.
        /// </summary>
        /// <param name="compressedUtf8String">A compressed UTF8 string.</param>
        /// <returns>The string.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when compressedUtf8String is null.</exception>
        public static string FromCompressedUtf8String(byte[] compressedUtf8String)
        {
            if (compressedUtf8String == null)
            {
                throw new ArgumentNullException("compressedUtf8String");
            }

            string domainName = string.Empty;
            int offset = 0;
            while (offset < compressedUtf8String.Length)
            {
                int labelLength = compressedUtf8String[offset];
                offset += sizeof(byte);
                if (labelLength == 0)
                {
                    break;
                }

                if (domainName.Length > 0)
                {
                    domainName += '.';
                }
                domainName += Encoding.UTF8.GetString(compressedUtf8String, offset, labelLength);
                offset += labelLength;
            }

            return domainName;
        }
    }
}
