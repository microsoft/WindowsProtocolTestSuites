// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Value Expression Type
    /// </summary>
    public enum ValueExpressionType
    {
        /// <summary>
        /// Integer
        /// </summary>
        Integer,

        /// <summary>
        /// Variable
        /// </summary>
        Variable,

        /// <summary>
        /// Null
        /// </summary>
        Null,
    }

    /// <summary>
    /// Value Expression
    /// </summary>
    public class ValueExpression : BaseExpression
    {
        private string text;
        private ValueExpressionType type;
        
        /// <summary>
        /// Expression text
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Expression type
        /// </summary>
        public ValueExpressionType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">Expression text</param>
        /// <param name="type">Expression type</param>
        public ValueExpression(string text, ValueExpressionType type)
        {
            this.text = text;
            this.type = type;
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
