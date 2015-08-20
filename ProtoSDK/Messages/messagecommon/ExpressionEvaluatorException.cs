// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Expression Evaluator Exception
    /// </summary>
    [Serializable]
    public class ExpressionEvaluatorException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ExpressionEvaluatorException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message description</param>
        public ExpressionEvaluatorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message description</param>
        /// <param name="innerException">The inner exception</param>
        public ExpressionEvaluatorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">The serialization info</param>
        /// <param name="context">The streaming context</param>
        protected ExpressionEvaluatorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
