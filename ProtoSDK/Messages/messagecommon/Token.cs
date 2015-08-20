// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Token
    /// </summary>
    public class Token : IToken
    {
        private string text;
        private TokenType type;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Token type</param>
        public Token(TokenType type)
        {
            this.type = type;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Token type</param>
        /// <param name="text">Token text</param>
        public Token(TokenType type, string text)
        {
            this.type = type;
            this.text = text;
        }

        /// <summary>
        /// Token text
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        /// <summary>
        /// Token type
        /// </summary>
        public virtual TokenType Type
        {
            get
            {
                return type;
            }

            set
            {
                this.type = value;
            }
        }
    }
}
