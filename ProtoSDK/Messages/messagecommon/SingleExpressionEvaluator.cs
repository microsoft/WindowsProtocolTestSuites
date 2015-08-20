// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Single Expression Evaluator
    /// </summary>
    public class SingleExpressionEvaluator
    {
        private string expression;
        private IEvaluationContext context;
        
        /// <summary>
        /// The single expression
        /// </summary>
        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }
        
        /// <summary>
        /// Evaluation context
        /// </summary>
        public IEvaluationContext Context
        {
            get { return context; }
            set { context = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SingleExpressionEvaluator()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The evaluation context</param>
        public SingleExpressionEvaluator(IEvaluationContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The evaluation context</param>
        /// <param name="expression">The expression</param>
        public SingleExpressionEvaluator(IEvaluationContext context, string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            this.expression = expression;
            this.context = context;
        }

        /// <summary>
        /// Evaluate the expression
        /// </summary>
        /// <returns>The evaluated object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public object Evaluate()
        {
            if (expression == null)
            {
                throw new ExpressionEvaluatorException("the expression should not be empty or null");
            }

            object result;
            try
            {
                ExpressionLexer lexer = new ExpressionLexer(expression);
                ExpressionParser parser = new ExpressionParser(
                    new TokenStream(lexer),
                    context);

                ExpressionNode tree = parser.Parse();
                ExpressionVisitor visitor = new ExpressionVisitor(context);
                IExpression expr = ExpressionBuilder.Build(tree);
                expr.Accept(visitor);

                result = visitor.EvaluationResult;
            }
            catch (Exception e) // reports all unexpected exceptions
            {
                context.ReportError(
                    String.Format(
                    CultureInfo.InvariantCulture,
                    "failed to evaluate expression '{0}' : '{1}'",
                    expression,
                    e.Message));
                result = expression;
            }

            return result;
        }
    }
}
