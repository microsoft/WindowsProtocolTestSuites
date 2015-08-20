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
    /// The Expression Builder
    /// </summary>
    public static class ExpressionBuilder
    {
        /// <summary>
        /// Build expression based on the input base node
        /// </summary>
        /// <param name="node">The input base node</param>
        /// <returns>The expression built</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static IExpression Build(BaseNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            switch (node.Type)
            {
                case TokenType.Integer:
                    return new ValueExpression(node.Text, ValueExpressionType.Integer);
                case TokenType.Plus:
                    return new BinaryExpression(
                        BinaryExpressionType.Plus,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.Minus:
                    return new BinaryExpression(
                        BinaryExpressionType.Minus,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.Multiply:
                    return new BinaryExpression(
                        BinaryExpressionType.Multiply,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.Divide:
                    return new BinaryExpression(
                        BinaryExpressionType.Div,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.Mod:
                    return new BinaryExpression(
                        BinaryExpressionType.Mod,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.ShiftLeft:
                    return new BinaryExpression(
                        BinaryExpressionType.ShiftLeft,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.ShiftRight:
                    return new BinaryExpression(
                        BinaryExpressionType.ShiftRight,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.GreaterOrEqual:
                    return new BinaryExpression(
                        BinaryExpressionType.GreaterOrEqual,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.LesserOrEqual:
                    return new BinaryExpression(
                        BinaryExpressionType.LesserOrEqual,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.Greater:
                    return new BinaryExpression(
                        BinaryExpressionType.Greater,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.Lesser:
                    return new BinaryExpression(
                        BinaryExpressionType.Lesser,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.Equal:
                    return new BinaryExpression(
                        BinaryExpressionType.Equal,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.NotEqual:
                    return new BinaryExpression(
                        BinaryExpressionType.NotEqual,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.And:
                    return new BinaryExpression(
                        BinaryExpressionType.And,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.Or:
                    return new BinaryExpression(
                        BinaryExpressionType.Or,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.BitAnd:
                    return new BinaryExpression(
                        BinaryExpressionType.BitAnd,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.BitOr:
                    return new BinaryExpression(
                        BinaryExpressionType.BitOr,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.BitXor:
                    return new BinaryExpression(
                        BinaryExpressionType.BitXor,
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)));
                case TokenType.SizeOf:
                    return new FunctionExpression(node.Text,
                        node.GetChild(0).Text,
                        Build((ExpressionNode)node.GetChild(0)));
                case TokenType.Variable:
                    return new ValueExpression(node.Text, ValueExpressionType.Variable);
                case TokenType.Not:
                    return new UnaryExpression(UnaryExpressionType.Not,
                        Build((ExpressionNode)node.GetChild(0)));
                case TokenType.BitNot:
                    return new UnaryExpression(UnaryExpressionType.BitNot,
                        Build((ExpressionNode)node.GetChild(0)));
                case TokenType.Negative:
                    return new UnaryExpression(UnaryExpressionType.Negative,
                        Build((ExpressionNode)node.GetChild(0)));
                case TokenType.Positive:
                    return new UnaryExpression(UnaryExpressionType.Positive,
                        Build((ExpressionNode)node.GetChild(0)));
                case TokenType.Dereference:
                    return new UnaryExpression(UnaryExpressionType.Dereference,
                        Build((ExpressionNode)node.GetChild(0)));
                case TokenType.Conditional:
                    return new ConditionalExpression(
                        Build((ExpressionNode)node.GetChild(0)),
                        Build((ExpressionNode)node.GetChild(1)),
                        Build((ExpressionNode)node.GetChild(2)));
                case TokenType.EndOfFile:
                    return new ValueExpression(String.Empty, ValueExpressionType.Null);
                default:
                    throw new ExpressionEvaluatorException(
                        string.Format(CultureInfo.InvariantCulture, "'{0}' operation is not supported.", node.Type));
            }
        }
    }
}
