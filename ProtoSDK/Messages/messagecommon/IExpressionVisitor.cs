// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Interface for all expression visitors
    /// </summary>
    public interface IExpressionVisitor
    {
        /// <summary>
        /// Visit an expression
        /// </summary>
        /// <param name="expression">The expression to be visited</param>
        void Visit(IExpression expression);

        /// <summary>
        /// Visit a unary expression
        /// </summary>
        /// <param name="expression">The expression to be visited</param>
        void Visit(UnaryExpression expression);

        /// <summary>
        /// Visit a binary expression
        /// </summary>
        /// <param name="expression">The expression to be visited</param>
        void Visit(BinaryExpression expression);

        /// <summary>
        /// Visit a conditional expression
        /// </summary>
        /// <param name="expression">The expression to be visited</param>
        void Visit(ConditionalExpression expression);

        /// <summary>
        /// Visit a value expression
        /// </summary>
        /// <param name="expression">The expression to be visited</param>
        void Visit(ValueExpression expression);

        /// <summary>
        /// Visit a function expression
        /// </summary>
        /// <param name="expression">The expression to be visited</param>
        void Visit(FunctionExpression expression);
    }
}
