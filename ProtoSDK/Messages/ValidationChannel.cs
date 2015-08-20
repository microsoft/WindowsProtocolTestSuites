// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    /// <summary>
    /// Derived Channel which will validate the value when read and write values
    /// </summary>
    public class ValidationChannel : Channel
    {
        private MessageUtils messageUtil;

        /// <summary>
        /// Constructs a typed stream which uses underlying stream and default marshaling configuration
        /// for block protocols.
        /// </summary>
        /// <param name="host">The message runtime host.</param>
        /// <param name="stream">The NetworkStream object.</param>
        public ValidationChannel(IRuntimeHost host, Stream stream)
            : this(host, stream, BlockMarshalingConfiguration.Configuration)
        {
        }

        /// <summary>
        /// Constructs a channel which uses underlying stream and given marshaler configuration.
        /// </summary>
        /// <param name="host">The message runtime host.</param>
        /// <param name="stream">The general stream object.</param>
        /// <param name="marshalingConfig">The marshaling configuration.</param>
        public ValidationChannel(IRuntimeHost host, Stream stream, MarshalingConfiguration marshalingConfig)
            : base(host, stream, marshalingConfig)
        {
            messageUtil = new MessageUtils(host);
        }

        /// <summary>
        /// Reads a value of the given type T from the stream which uses the underlying marshaler to unmarshal it.
        /// And the value will be validated.
        /// </summary>
        /// <typeparam name="T">The type of the value to be read.</typeparam>
        /// <returns>The value read from the channel.</returns>
        public override T Read<T>()
        {
            T value = base.Read<T>();
            messageUtil.Validate(value);
            return value;
        }

        /// <summary>
        /// Writes a value of given type T to the stream which uses the underlying marshaler to marshal it.
        /// And the value will be validated.
        /// </summary>
        /// <typeparam name="T">The type of the value which is written to the stream.</typeparam>
        /// <param name="value">The value which is written to the stream.</param>
        public override void Write<T>(T value)
        {
            messageUtil.Validate(value);
            base.Write<T>(value);
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        /// <remarks>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If the parameter 'disposing' equals true, the method is called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If the parameter 'disposing' equals false, the method is called by the 
        /// runtime from the inside of the finalizer and you should not refer to 
        /// other objects. Therefore, only unmanaged resources can be disposed.
        /// </remarks>
        /// <param name="disposing">Indicates if Dispose is called by user.</param>
        [SecurityPermission(SecurityAction.Demand)]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (messageUtil != null)
                    {
                        messageUtil.Dispose();
                    }
                }
            }
            catch
            {
                throw new InvalidOperationException(
                    "Fail to dispose resources.");
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
