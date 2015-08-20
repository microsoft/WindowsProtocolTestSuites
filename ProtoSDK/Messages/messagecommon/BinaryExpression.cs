// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Binary Expression Type
    /// </summary>
    public enum BinaryExpressionType
    {
        /// <summary>
        /// The AND operation
        /// </summary>
        And, // "&&"

        /// <summary>
        /// The "||" operation
        /// </summary>
        Or,

        /// <summary>
        /// The "==" operation
        /// </summary>
        Equal,

        /// <summary>
        /// The "!=" operation
        /// </summary>
        NotEqual,

        /// <summary>
        /// The compare operation (less than or equal to)
        /// </summary>
        LesserOrEqual,

        /// <summary>
        /// The compare operation (greater than or equal to)
        /// </summary>
        GreaterOrEqual,

        /// <summary>
        /// The compare operation (less than)
        /// </summary>
        Lesser,

        /// <summary>
        /// The compare operation (greater than)
        /// </summary>
        Greater,

        /// <summary>
        /// The "-" operation
        /// </summary>
        Minus,

        /// <summary>
        /// The "+" operation
        /// </summary>
        Plus,

        /// <summary>
        /// The "%" operation
        /// </summary>
        Mod,

        /// <summary>
        /// The "/" operation
        /// </summary>
        Div,

        /// <summary>
        /// The "*" operation
        /// </summary>
        Multiply,

        /// <summary>
        /// The shift left operation
        /// </summary>
        ShiftLeft,

        /// <summary>
        /// The shift right operation
        /// </summary>
        ShiftRight,

        /// <summary>
        /// The Bit AND operation
        /// </summary>
        BitAnd, // "&"

        /// <summary>
        /// The "|" operation
        /// </summary>
        BitOr,

        /// <summary>
        /// The "^" operation
        /// </summary>
        BitXor,
    }

    /// <summary>
    /// Binary Expression
    /// </summary>
    public class BinaryExpression : BaseExpression
    {
        private BinaryExpressionType type;
        private IExpression leftExpression;
        private IExpression rightExpression;

        /// <summary>
        /// Constructor
        /// </summary>
        public BinaryExpressionType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Left expression
        /// </summary>
        public IExpression LeftExpression
        {
            get { return leftExpression; }
            set { leftExpression = value; }
        }

        /// <summary>
        /// Right expression
        /// </summary>
        public IExpression RightExpression
        {
            get { return rightExpression; }
            set { rightExpression = value; }
        }

        /// <summary>
        /// Binary expression
        /// </summary>
        /// <param name="type">Binary expression type</param>
        /// <param name="leftExpression">The left expression</param>
        /// <param name="rightExpression">The right expression</param>
        public BinaryExpression(BinaryExpressionType type, IExpression leftExpression, IExpression rightExpression)
        {
            this.type = type;
            this.leftExpression = leftExpression;
            this.rightExpression = rightExpression;
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
