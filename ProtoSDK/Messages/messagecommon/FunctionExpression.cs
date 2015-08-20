// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Function Expression
    /// </summary>
    public class FunctionExpression : BaseExpression
    {
        private string functionName;
        private IExpression expression;
        private string text;

        /// <summary>
        /// Function name
        /// </summary>
        public string FunctionName
        {
            get { return functionName; }
            set { functionName = value; }
        }

        /// <summary>
        /// Function expression
        /// </summary>
        public IExpression Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        /// <summary>
        /// Expression text
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="functionName">Function name</param>
        /// <param name="text">Function text</param>
        /// <param name="expression">Function expression</param>
        public FunctionExpression(string functionName, string text, IExpression expression)
        {
            this.functionName = functionName;
            this.expression = expression;
            this.text = text;
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
