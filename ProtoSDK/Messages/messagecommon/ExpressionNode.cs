// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Expression Node
    /// </summary>
    public class ExpressionNode : BaseNode
    {
        private IToken token;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExpressionNode()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Token of the expression node</param>
        public ExpressionNode(IToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            this.token = token;
        }

        /// <summary>
        /// Token type
        /// </summary>
        public override TokenType Type
        {
            get
            {
                if (token == null)
                {
                    return TokenType.Invalid;
                }

                return token.Type;
            }
        }

        /// <summary>
        /// Node text
        /// </summary>
        public override string Text
        {
            get
            {
                if (token == null)
                {
                    return null;
                }
                return token.Text;
            }
        }

        /// <summary>
        /// A String that represents the current Object.
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            if (token == null)
            {
                return null;
            }

            return token.Text;
        }
    }
}
