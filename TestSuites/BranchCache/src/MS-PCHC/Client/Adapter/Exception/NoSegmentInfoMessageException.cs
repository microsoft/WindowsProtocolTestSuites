// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    using System;
    using System.Runtime.Serialization;
    using System.Security;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;

    /// <summary>
    /// NoSegmentInfoMessageException is inherited from TimeoutException, which is used to specify
    /// the hosted cache server do not receive SEGMENT_INFO_MESSAGE from the client.
    /// </summary>
    [Serializable]
    public class NoSegmentInfoMessageException : NoSEGMENTINFOMESSAGEReceivedException
    {
        /// <summary>
        /// Initializes a new instance of the NoSegmentInfoMessageException class.
        /// </summary>
        public NoSegmentInfoMessageException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the NoSegmentInfoMessageException class with a specified
        /// error message is the cause of this exception. 
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NoSegmentInfoMessageException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NoSegmentInfoMessageException class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception. 
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, or a null reference
        /// (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public NoSegmentInfoMessageException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NoSegmentInfoMessageException class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The System.Runtime.Serialization.SerializationInfo that holds the serialized
        /// object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The System.Runtime.Serialization.StreamingContext that contains contextual
        /// information about the source or destination.
        /// </param>
        [SecuritySafeCritical]
        protected NoSegmentInfoMessageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
