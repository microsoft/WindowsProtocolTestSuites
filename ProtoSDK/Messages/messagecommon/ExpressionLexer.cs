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
    /// The Scanner State
    /// </summary>
    public enum ScannerState
    {
        /// <summary>
        /// Scan by Space
        /// </summary>
        Space,

        /// <summary>
        /// Scan by Number
        /// </summary>
        Number,

        /// <summary>
        /// Scan by Identifier
        /// </summary>
        Identifier,

        /// <summary>
        /// Scan by Operator
        /// </summary>
        Operator,
    }

    /// <summary>
    /// The Expression Lexer
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    public class ExpressionLexer
    {
        private IEnumerator<IToken> tokens;

        private IToken token;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="expression">The expression</param>
        public ExpressionLexer(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            this.tokens = Tokenize(expression);
        }

        /// <summary>
        /// Get the next token
        /// </summary>
        /// <returns>The next token</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IToken GetNextToken()
        {
            if (tokens.MoveNext())
            {
                token = tokens.Current;
                if (token.Type == TokenType.Invalid)
                {
                    throw new ExpressionEvaluatorException(
                        string.Format(CultureInfo.InvariantCulture, "invalid token '{0}'", token.Text.Substring(0)));
                }

                return token;
            }
            else
                return new Token(TokenType.EndOfFile, String.Empty);
        }

        /// <summary>
        /// Tokenize the input expression
        /// </summary>
        /// <param name="expression">The input expression</param>
        /// <returns>Tokens</returns>
        protected IEnumerator<IToken> Tokenize(string expression)
        {
            int pos = 0;
            int n = expression.Length;
            StringBuilder currentToken = new StringBuilder();
            ScannerState state = ScannerState.Space;
            while (pos < n)
            {
                char nextCh = expression[pos++];
                switch (state)
                {
                    case ScannerState.Space:
                        if (IsWhitespace(nextCh))
                            continue;
                        if (IsIdentifier(nextCh, true))
                        {
                            currentToken.Append(nextCh);
                            state = ScannerState.Identifier;
                            continue;
                        }
                        if (IsNumber(nextCh, true))
                        {
                            currentToken.Append(nextCh);
                            state = ScannerState.Number;
                            continue;
                        }
                        if (IsOperator(nextCh))
                        {
                            currentToken.Append(nextCh);
                            state = ScannerState.Operator;
                            continue;
                        }
                        if (IsSeparator(nextCh))
                        {
                            yield return MakeSeparatorToken(nextCh.ToString());
                            continue;
                        }
                        yield return MakeInvalidToken("#" + nextCh);
                        continue;

                    case ScannerState.Identifier:
                        if (IsIdentifier(nextCh, false))
                        {
                            currentToken.Append(nextCh);
                            continue;
                        }
                        else
                        {
                            yield return MakeIdentifierToken(currentToken.ToString());
                            currentToken = new StringBuilder();
                            pos--;
                            state = ScannerState.Space;
                            continue;
                        }

                    case ScannerState.Number:
                        if (IsNumber(nextCh, false))
                        {
                            currentToken.Append(nextCh);
                            continue;
                        }
                        else
                        {
                            yield return MakeNumberToken(currentToken.ToString());
                            currentToken = new StringBuilder();
                            pos--;
                            state = ScannerState.Space;
                            continue;
                        }

                    case ScannerState.Operator:
                        if (IsOperator(nextCh)
                            && IsSupportedOperator(currentToken.ToString() + nextCh))
                        {
                            currentToken.Append(nextCh);
                            continue;
                        }
                        else
                        {
                            yield return MakeOperatorToken(currentToken.ToString());
                            currentToken = new StringBuilder();
                            pos--;
                            state = ScannerState.Space;
                            continue;
                        }
                }
            }
            if (state != ScannerState.Space)
                yield return MakeTokenFromState(currentToken.ToString(), state);
        }

        /// <summary>
        /// Check if the input char is an identifier
        /// </summary>
        /// <param name="ch">The input char</param>
        /// <param name="start">Whether the char is at the beginning of an expression</param>
        /// <returns>True if the input char is an identifier</returns>
        protected static bool IsIdentifier(char ch, bool start)
        {
            return (start ? Char.IsLetter(ch) : Char.IsLetterOrDigit(ch))
                || ch == '@'
                || ch == '_';
        }

        /// <summary>
        /// Check if the input char is a number
        /// </summary>
        /// <param name="ch">The input char</param>
        /// <param name="start">Whether the char is at the beginning of an expression</param>
        /// <returns>True if the input char is a number</returns>
        protected static bool IsNumber(char ch, bool start)
        {
            return Char.IsDigit(ch) ||
                    !start &&
                    (ch == 'x'
                    || ch == 'X'
                    || ch >= 'a' && ch <= 'f'
                    || ch >= 'A' && ch <= 'F');
        }

        /// <summary>
        /// Check if the input char is a white space
        /// </summary>
        /// <param name="ch">The input char</param>
        /// <returns>True if the input char is a white space</returns>
        protected static bool IsWhitespace(char ch)
        {
            return Char.IsWhiteSpace(ch);
        }

        /// <summary>
        /// Check if the input char is an operator
        /// </summary>
        /// <param name="ch">The input char</param>
        /// <returns>True if the input char is an operator</returns>
        protected static bool IsOperator(char ch)
        {
            string opchs = "?:&|^=!><+-*/%*~";
            return opchs.IndexOf(ch) >= 0;
        }

        /// <summary>
        /// Check if the input char is a separator
        /// </summary>
        /// <param name="ch">The input char</param>
        /// <returns>True if the input char is a separator</returns>
        protected static bool IsSeparator(char ch)
        {
            return ch == '(' || ch == ')' || ch == ',';
        }

        private static string[] supportedOperators = {
                                                           "+",
                                                           "-",
                                                           "*",
                                                           "/",
                                                           "%",
                                                           "~",
                                                           "!",
                                                           "<<",
                                                           ">>",
                                                           ">",
                                                           "<",
                                                           ">=",
                                                           "<=",
                                                           "==",
                                                           "!=",
                                                           "&&",
                                                           "||",
                                                           "&",
                                                           "|",
                                                           "^",
                                                           "?",
                                                           ":",
                                                       };
        
        /// <summary>
        /// Check if the input operator is supported
        /// </summary>
        /// <param name="op">The input operator (in string format)</param>
        /// <returns>True if the input operator is supported</returns>
        protected static bool IsSupportedOperator(string op)
        {
            foreach (string supportedOp in supportedOperators)
            {
                if (supportedOp == op)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Make token for the input operator 
        /// </summary>
        /// <param name="op">The input operator (in string format)</param>
        /// <returns>The token made based on the input operator</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected static IToken MakeOperatorToken(string op)
        {
            switch (op)
            {
                case "+":
                    return new Token(TokenType.Plus, op);
                case "-":
                    return new Token(TokenType.Minus, op);
                case "*":
                    return new Token(TokenType.Multiply, op);
                case "/":
                    return new Token(TokenType.Divide, op);
                case "%":
                    return new Token(TokenType.Multiply, op);
                case "~":
                    return new Token(TokenType.BitNot, op);
                case "!":
                    return new Token(TokenType.Not, op);
                case "<<":
                    return new Token(TokenType.ShiftLeft, op);
                case ">>":
                    return new Token(TokenType.ShiftRight, op);
                case ">":
                    return new Token(TokenType.Greater, op);
                case "<":
                    return new Token(TokenType.Lesser, op);
                case ">=":
                    return new Token(TokenType.GreaterOrEqual, op);
                case "<=":
                    return new Token(TokenType.LesserOrEqual, op);
                case "==":
                    return new Token(TokenType.Equal, op);
                case "!=":
                    return new Token(TokenType.NotEqual, op);
                case "&&":
                    return new Token(TokenType.And, op);
                case "||":
                    return new Token(TokenType.Or, op);
                case "&":
                    return new Token(TokenType.BitAnd, op);
                case "|":
                    return new Token(TokenType.BitOr, op);
                case "^":
                    return new Token(TokenType.BitXor, op);
                case "?":
                    return new Token(TokenType.Conditional, op);
                case ":":
                    return new Token(TokenType.Colon, op);
                default:
                    throw new ExpressionEvaluatorException(
                        string.Format(CultureInfo.InvariantCulture, "unknown operator : {0}", op));
            }
        }

        /// <summary>
        /// Make token for the input number
        /// </summary>
        /// <param name="number">The input number (in string format)</param>
        /// <returns>The token made based on the input number</returns>
        protected static IToken MakeNumberToken(string number)
        {
            return new Token(TokenType.Integer, number);
        }

        /// <summary>
        /// Make token for the input identifier
        /// </summary>
        /// <param name="identifier">The input identifier (in string format)</param>
        /// <returns>The token made based on the input identifier</returns>
        protected static IToken MakeIdentifierToken(string identifier)
        {
            return new Token(TokenType.String, identifier);
        }

        /// <summary>
        /// Make token for the input separator
        /// </summary>
        /// <param name="separator">The input separator (in string format)</param>
        /// <returns>The token made based on the input separator</returns>
        protected static IToken MakeSeparatorToken(string separator)
        {
            return new Token(TokenType.Separator, separator);
        }

        /// <summary>
        /// Make invalid token for the input string
        /// </summary>
        /// <param name="invalid">The input string</param>
        /// <returns>The token made based on the input string</returns>
        protected static IToken MakeInvalidToken(string invalid)
        {
            return new Token(TokenType.Invalid, invalid);
        }

        /// <summary>
        /// Make token from token string and scanner state
        /// </summary>
        /// <param name="token">The input token string</param>
        /// <param name="state">The scanner state</param>
        /// <returns>The token made based on the scanner state</returns>
        protected static IToken MakeTokenFromState(string token, ScannerState state)
        {
            switch (state)
            {
                case ScannerState.Identifier:
                    return MakeIdentifierToken(token);
                case ScannerState.Number:
                    return MakeNumberToken(token);
                case ScannerState.Operator:
                    return MakeOperatorToken(token);
                default:
                    throw new ExpressionEvaluatorException(
                        string.Format(CultureInfo.InvariantCulture, "unknown token '{0}'", token));
            }
        }
    }
}
