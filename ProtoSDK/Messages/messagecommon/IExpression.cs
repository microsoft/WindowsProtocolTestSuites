// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Interface for all types of expressions
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        /// Accept an expression visitor
        /// </summary>
        /// <param name="visitor">The expression visitor</param>
        void Accept(IExpressionVisitor visitor);
    }
}
