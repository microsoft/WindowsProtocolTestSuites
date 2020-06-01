// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib
{
    public class SspiUtility
    {
        /// <summary>
        /// Verify Message Header
        /// </summary>
        /// <param name="message">Signed message to be verified</param>
        /// <returns>If true, verify successful, otherwise failed.</returns>
        public static bool VerifyMessageHeader(byte[] message)
        {
            if (message == null)
            {
                return false;
            }

            //Verify message header
            if (message.Length < sizeof(int))
            {
                return false;
            }
            else
            {
                int messageLength = BitConverter.ToInt32(message, 0);
                //The value of header should be less than or equal length of message body and signature(optional).
                if (messageLength > message.Length - sizeof(int))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Concatenate Read or Write SecurityBuffers
        /// </summary>
        /// <param name="securityBuffers">
        /// the security buffer array to decrypt.<para/>
        /// it can contain none or some data security buffer, that are combine to one message to decrypt.<para/>
        /// it can contain none or some token security buffer, in which the signature is stored.
        /// </param>
        /// <param name="targetTypes">flags that indicate the type of buffer.</param>
        /// <returns>Concatenate SecurityBuffers</returns>
        public static byte[] ConcatenateReadWriteSecurityBuffers(
            SecurityBuffer[] securityBuffers,
            params SecurityBufferType[] targetTypes)
        {
            if (securityBuffers == null || securityBuffers.Length == 0)
            {
                throw new ArgumentNullException("securityBuffers");
            }
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException("securityBuffers");
                }
            }

            return ConcatenateSecurityBuffers(securityBuffers, targetTypes, false);
        }

        private static byte[] ConcatenateSecurityBuffers(
            SecurityBuffer[] securityBuffers,
            SecurityBufferType[] targetTypes,
            bool bothReadOnlyAndReadWrite)
        {
            byte[] buf = new byte[0];
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                SecurityBufferType securityBufferType = (securityBuffers[i].BufferType & ~SecurityBufferType.AttrMask);
                bool typeMatch = false;
                for (int j = 0; j < targetTypes.Length; j++)
                {
                    if (securityBufferType == targetTypes[j])
                    {
                        typeMatch = true;
                        break;
                    }
                }
                if (typeMatch)
                {
                    bool skip = !bothReadOnlyAndReadWrite
                        && (((securityBuffers[i].BufferType & SecurityBufferType.ReadOnly) != 0)
                        || ((securityBuffers[i].BufferType & SecurityBufferType.ReadOnlyWithChecksum) != 0));

                    if (!skip && securityBuffers[i].Buffer != null)
                    {
                        buf = ArrayUtility.ConcatenateArrays(buf, securityBuffers[i].Buffer);
                    }
                }
            }
            return buf;
        }

        /// <summary>
        /// Concatenate all buffers of a specified type in the list.
        /// </summary>
        /// <param name="securityBuffers">Input security buffers.</param>
        /// <param name="targetTypes">Specified types.</param>
        /// <returns>A single byte array with all buffers concatenated.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        public static byte[] ConcatenateSecurityBuffers(SecurityBuffer[] securityBuffers, params SecurityBufferType[] targetTypes)
        {
            if (securityBuffers == null || securityBuffers.Length == 0)
            {
                throw new ArgumentNullException("securityBuffers");
            }
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException("securityBuffers");
                }
            }

            return ConcatenateSecurityBuffers(securityBuffers, targetTypes, true);
        }

        /// <summary>
        /// Update buffers of a specified type in the list. 
        /// Buffer will be separated automatically to fit the original length of a security buffer. 
        /// If Buffer field of an input security buffer is null, it means the length is unlimited 
        /// (that is all remaining data will be copied into it). 
        /// Only read-write (READONLY flag is not set) security buffer will be updated.
        /// </summary>
        /// <param name="securityBuffers">Input security buffers.</param>
        /// <param name="targetType">A specified type.</param>
        /// <param name="buffer">The buffer to be updated into security buffers.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers or buffer is null.
        /// </exception>
        /// <exception cref="SspiException">
        /// Total length of security buffers is not enough.
        /// </exception>
        public static void UpdateSecurityBuffers(SecurityBuffer[] securityBuffers, SecurityBufferType targetType, byte[] buffer)
        {
            UpdateSecurityBuffers(securityBuffers, new SecurityBufferType[] { targetType }, buffer);
        }


        /// <summary>
        /// Update buffers of a specified type in the list. 
        /// Buffer will be separated automatically to fit the original length of a security buffer. 
        /// If Buffer field of an input security buffer is null, it means the length is unlimited 
        /// (that is all remaining data will be copied into it). 
        /// Only read-write (READONLY flag is not set) security buffer will be updated.
        /// </summary>
        /// <param name="securityBuffers">Input security buffers.</param>
        /// <param name="targetTypes">Specified types.</param>
        /// <param name="buffer">The buffer to be updated into security buffers.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers or buffer is null.
        /// </exception>
        /// <exception cref="SspiException">
        /// Total length of security buffers is not enough.
        /// </exception>
        public static void UpdateSecurityBuffers(SecurityBuffer[] securityBuffers, SecurityBufferType[] targetTypes, byte[] buffer)
        {
            if (securityBuffers == null || securityBuffers.Length == 0)
            {
                throw new ArgumentNullException("securityBuffers");
            }
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException("securityBuffers");
                }
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            int offset = 0;
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                SecurityBufferType securityBufferType = (securityBuffers[i].BufferType & ~SecurityBufferType.AttrMask);
                bool isReadOnly = ((securityBuffers[i].BufferType & SecurityBufferType.ReadOnly) != 0)
                               || ((securityBuffers[i].BufferType & SecurityBufferType.ReadOnlyWithChecksum) != 0);
                bool typeMatch = false;
                for (int j = 0; j < targetTypes.Length; j++)
                {
                    if (securityBufferType == targetTypes[j])
                    {
                        typeMatch = true;
                        break;
                    }
                }
                if (typeMatch && !isReadOnly)
                {
                    int length = buffer.Length - offset;
                    if (securityBuffers[i].Buffer != null && securityBuffers[i].Buffer.Length < length)
                    {
                        length = securityBuffers[i].Buffer.Length;
                    }

                    securityBuffers[i].Buffer = ArrayUtility.SubArray(
                        buffer,
                        offset,
                        length);

                    offset += length;
                }
            }

            if (offset < buffer.Length)
            {
                throw new SspiException("Total length of security buffers is not enough.");
            }
            else if (offset > buffer.Length)
            {
                //Unlikely to happen
                throw new InvalidOperationException("Extra data were written to security buffers.");
            }
        }

        /// <summary>
        /// Indicates the uppercase representation of string.
        /// </summary>
        /// <param name="str">the string to get uppercase</param>
        /// <returns>the uppercase of string</returns>
        /// <exception cref="ArgumentNullException">str can not be null</exception>
        /// <remarks>this method is defined in section: 6 Appendix A: Cryptographic Operations Reference</remarks>
        public static string UpperCase(
            string str
            )
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            return str.ToUpper();
        }
    }
}
