// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Token stream
    /// </summary>
    public class TokenStream : ITokenStream
    {
        private ExpressionLexer lexer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lexer">The expression lexer</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        public TokenStream(ExpressionLexer lexer)
        {
            this.lexer = lexer;
        }

        /// <summary>
        /// Get next token
        /// </summary>
        /// <returns>The next token</returns>
        public IToken NextToken()
        {
            return lexer.GetNextToken();
        }
    }
}
