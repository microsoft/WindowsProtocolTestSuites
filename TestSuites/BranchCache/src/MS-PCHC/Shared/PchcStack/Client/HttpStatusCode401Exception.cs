// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    using System;
    using System.Runtime.Serialization;
    using System.Security;

    /// <summary>
    /// HttpStatusCode401Exception is inherited from Exception, which is used to specify the client is not authenticated 
    /// when the hosted cache is configured enable the client authentication.  
    /// </summary>
    [Serializable]
    public class HttpStatusCode401Exception : Exception
    {
        /// <summary>
        /// Initializes a new instance of the HttpStatusCode401Exception class.
        /// </summary>
        public HttpStatusCode401Exception()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the HttpStatusCode401Exception class with a specified
        /// error message is the cause of this exception. 
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public HttpStatusCode401Exception(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the HttpStatusCode401Exception class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception. 
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, or a null reference
        /// (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public HttpStatusCode401Exception(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the HttpStatusCode401Exception class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The System.Runtime.Serialization.SerializationInfo that holds the serialized
        /// object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The System.Runtime.Serialization.StreamingContext that contains contextual
        /// information about the source or destination.
        /// </param>
        protected HttpStatusCode401Exception(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
