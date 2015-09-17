// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// SspiException is used as specified exception in SSPI library.
    /// </summary>
    [Serializable]
    public class SspiException : Exception
    {
        /// <summary>
        /// Error Code.
        /// </summary>
        private uint errorCode;

        /// <summary>
        /// Constructor with serialization information and streaming context.
        /// </summary>
        /// <param name="info">Serialization information</param>
        /// <param name="context">Streaming context</param>
        /// <exception cref="ArgumentNullException">If info is null, this exception will be thrown.</exception>
        protected SspiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.errorCode = info.GetUInt32("errorCode");
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public SspiException()
        {

        }


        /// <summary>
        /// Constructor with message
        /// </summary>
        /// <param name="message">Message text</param>
        public SspiException(string message)
            : base(message)
        {
        }


        /// <summary>
        /// Constructor with message and innerException.
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="innerException">Inner exception</param>
        public SspiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Constructor with message and error code.
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="errorCode">Error code</param>
        public SspiException(string message, uint errorCode)
            : base(message)
        {
            this.errorCode = errorCode;
        }


        /// <summary>
        /// System.Exception method overrides
        /// </summary>
        /// <param name="info">Serialization information</param>
        /// <param name="context">Streaming context</param>
        /// <exception cref="ArgumentNullException">If info is null, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]        
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            base.GetObjectData(info, context);

            info.AddValue("errorCode", this.errorCode);
        }


        /// <summary>
        /// Error Code is returned by SSPI.
        /// </summary>
        public uint ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }


        /// <summary>
        /// System.Exception property override.
        /// </summary>
        public override string Message
        {
            get
            {
                string message = string.Format(CultureInfo.InvariantCulture, 
                    "{0}. Error Code = '{1:X}'.", 
                    base.Message, 
                    this.ErrorCode);
                return message;
            }
        }
    }
}
