// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// The exception that is thrown if an Pickle internal error occurs.
    /// </summary>
    [Serializable]
    public class PickleException : Exception
    {
        /// <summary>
        /// The return value of failed Pickle function.
        /// </summary>
        private int errorCode;


        /// <summary>
        /// Initializes a new instance of the PickleException class.
        /// </summary>
        public PickleException()
            : base()
        {
        }


        /// <summary>
        /// Initializes a new instance of the PickleException class with specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PickleException(string message)
            : base(message)
        {
        }

        
        /// <summary>
        /// Initializes a new instance of the PickleException class with specified error code.
        /// </summary>
        /// <param name="error">The return value of failed Pickle function.</param>
        public PickleException(int error)
            : this()
        {
            errorCode = error;
        }


        /// <summary>
        /// Initializes a new instance of the PickleException class with a specified error code and
        /// error message.
        /// </summary>
        /// <param name="error">The return value of failed Pickle function.</param>
        /// <param name="message">The message that describes the error.</param>
        public PickleException(int error, string message)
            : this(message)
        {
            errorCode = error;
        }


        /// <summary>
        /// Initializes a new instance of the PickleException class with serialized
        /// data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized
        /// object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual
        /// information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">
        /// The class name is null or System.Exception.HResult is zero (0).</exception>
        protected PickleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            errorCode = (int)info.GetValue("errorCode", typeof(int));
        }


        /// <summary>
        /// Initializes a new instance of the PickleException class with a specified
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// (Nothing in Visual Basic) if no inner exception is specified.</param>
        public PickleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the PickleException class with a specified error code,
        /// error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="error">The return value of failed Pickle function.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// (Nothing in Visual Basic) if no inner exception is specified.</param>
        public PickleException(int error, string message, Exception innerException)
            : this(message, innerException)
        {
            errorCode = error;
        }


        /// <summary>
        /// Add errorCode value into SerializationInfo.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized
        /// object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual
        /// information about the source or destination.</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("errorCode", errorCode);
        }


        /// <summary>
        /// The return value of failed Pickle function.
        /// </summary>
        public int ErrorCode
        {
            get
            {
                return errorCode;
            }
        }
    }
}
