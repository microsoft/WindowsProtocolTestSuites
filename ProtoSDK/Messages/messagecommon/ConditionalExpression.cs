// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Conditional Expression
    /// </summary>
    public class ConditionalExpression : BaseExpression
    {
        private IExpression firstExpression;
        private IExpression secondExpression;
        private IExpression thirdExpression;

        /// <summary>
        /// First expression
        /// </summary>
        public IExpression FirstExpression
        {
            get { return firstExpression; }
            set { firstExpression = value; }
        }
        
        /// <summary>
        /// Second expression
        /// </summary>
        public IExpression SecondExpression
        {
            get { return secondExpression; }
            set { secondExpression = value; }
        }
        
        /// <summary>
        /// Third expression
        /// </summary>
        public IExpression ThirdExpression
        {
            get { return thirdExpression; }
            set { thirdExpression = value; }
        }

        /// <summary>
        /// Conditional expression
        /// </summary>
        /// <param name="firstExpression">First expression</param>
        /// <param name="secondExpression">Second expression</param>
        /// <param name="thirdExpression">Third expression</param>
        public ConditionalExpression(IExpression firstExpression, IExpression secondExpression, IExpression thirdExpression)
        {
            this.firstExpression = firstExpression;
            this.secondExpression = secondExpression;
            this.thirdExpression = thirdExpression;
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
