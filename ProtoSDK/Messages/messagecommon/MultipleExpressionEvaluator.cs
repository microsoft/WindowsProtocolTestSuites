// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Multiple Expression Evaluator
    /// </summary>
    public class MultipleExpressionEvaluator
    {
        private string expression;
        private IEvaluationContext context;
        
        /// <summary>
        /// The multiple expression
        /// </summary>
        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        /// <summary>
        /// The evaluation context
        /// </summary>
        public IEvaluationContext Context
        {
            get { return context; }
            set { context = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MultipleExpressionEvaluator()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The evaluation context</param>
        public MultipleExpressionEvaluator(IEvaluationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "evaluation context must be provided");
            }

            this.context = context;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The evaluation context</param>
        /// <param name="expression">The expression</param>
        public MultipleExpressionEvaluator(IEvaluationContext context, string expression)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "evaluation context must be provided");
            }

            if (expression == null)
            {
                throw new ArgumentNullException("expression", "expression must be provided");
            }

            this.expression = expression;
            this.context = context;
        }

        /// <summary>
        /// Evaluate the expression
        /// </summary>
        /// <returns>The evaluated objects</returns>
        public IList<object> Evaluate()
        {
            IList<object> result = new List<object>();
            string[] exprs = expression.Split(',');

            for (int i = 0; i < exprs.Length; i++)
            {
                exprs[i] = exprs[i].Trim();
                SingleExpressionEvaluator evaluator =
                    new SingleExpressionEvaluator(context, exprs[i]);
                object x = evaluator.Evaluate();
                result.Add(x);
            }

            return result;
        }
    }
}
