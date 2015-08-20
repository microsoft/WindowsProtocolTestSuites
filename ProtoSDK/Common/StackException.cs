// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// the exception class of StackSdk. it will be thrown when exception 
    /// occers in StackSdk
    /// .
    /// </summary>
    [Serializable]
    public class StackException : Exception
    {
         /// <summary>
        /// Constructor stack exception
        /// </summary>
        public StackException()
            : base()
        {
        }

        /// <summary>
        /// Constructor stack exception
        /// </summary>
        /// <param name="message">The message text.</param>
        public StackException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor stack exception
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="innerException">The inner exception.</param>
        public StackException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized
        /// object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual
        /// information about the source or destination.</param>
        protected StackException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
   }
}
