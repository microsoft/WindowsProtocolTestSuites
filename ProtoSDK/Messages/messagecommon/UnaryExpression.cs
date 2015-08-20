// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Unary Expression Type
    /// </summary>
    public enum UnaryExpressionType
    {
        /// <summary>
        /// Not operation
        /// </summary>
        Not,

        /// <summary>
        /// Bit Not operation
        /// </summary>
        BitNot,

        /// <summary>
        /// Negative
        /// </summary>
        Negative,

        /// <summary>
        /// Positive
        /// </summary>
        Positive,

        /// <summary>
        /// Dereference
        /// (the dereference operator, which allows reading and writing to a pointer)
        /// </summary>
        Dereference,
    }

    /// <summary>
    /// Unary Expression
    /// </summary>
    public class UnaryExpression : BaseExpression
    {
        private UnaryExpressionType type;
        private IExpression expression;
        
        /// <summary>
        /// Expression type
        /// </summary>
        public UnaryExpressionType Type
        {
            get { return type; }
            set { type = value; }
        }
        
        /// <summary>
        /// Expression
        /// </summary>
        public IExpression Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Expression type</param>
        /// <param name="expression">The expression</param>
        public UnaryExpression(UnaryExpressionType type, IExpression expression)
        {
            this.type = type;
            this.expression = expression;
        }

        /// <summary>
        /// Accept an expression visitor
        /// </summary>
        /// <param name="visitor">The expression visitor</param>
        public override void Accept(IExpressionVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException("visitor");
            }
            visitor.Visit(this);
        }
    }
}
