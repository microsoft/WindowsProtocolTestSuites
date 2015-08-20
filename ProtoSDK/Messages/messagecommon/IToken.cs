// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Token Type
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// End of file
        /// </summary>
        EndOfFile,

        /// <summary>
        /// Token invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Token unknown
        /// </summary>
        Unknown,

        /// <summary>
        /// Token empty
        /// </summary>
        Empty,

        /// <summary>
        /// Plus "+"
        /// </summary>
        Plus,

        /// <summary>
        /// Minus "-"
        /// </summary>
        Minus,

        /// <summary>
        /// Multiply "*"
        /// </summary>
        Multiply,

        /// <summary>
        /// Divide "/"
        /// </summary>
        Divide,

        /// <summary>
        /// Mod "%"
        /// </summary>
        Mod,

        /// <summary>
        /// Shift left
        /// </summary>
        ShiftLeft,

        /// <summary>
        /// Shift right
        /// </summary>
        ShiftRight,

        /// <summary>
        /// Equals to "=="
        /// </summary>
        Equal,

        /// <summary>
        /// Not equals to "!="
        /// </summary>
        NotEqual,

        /// <summary>
        /// Less than
        /// </summary>
        Lesser,

        /// <summary>
        /// Greater than
        /// </summary>
        Greater,

        /// <summary>
        /// Less than or equals to
        /// </summary>
        LesserOrEqual,

        /// <summary>
        /// Greater than or equals to
        /// </summary>
        GreaterOrEqual,

        /// <summary>
        /// Bit XOR "^"
        /// </summary>
        BitXor,

        /// <summary>
        /// Bit AND
        /// </summary>
        BitAnd, // "&"

        /// <summary>
        /// Bit OR "|"
        /// </summary>
        BitOr,

        /// <summary>
        /// Conditional AND
        /// </summary>
        And, // "&&"

        /// <summary>
        /// Conditional OR "||"
        /// </summary>
        Or,

        /// <summary>
        /// Conditional question mark "?"
        /// </summary>
        Conditional, 

        /// <summary>
        /// Colon ":"
        /// </summary>
        Colon,

        /// <summary>
        /// Integer
        /// </summary>
        Integer,

        /// <summary>
        /// String
        /// </summary>
        String,

        /// <summary>
        /// Separator
        /// </summary>
        Separator,

        /// <summary>
        /// SizeOf
        /// </summary>
        SizeOf,

        /// <summary>
        /// Bracket
        /// </summary>
        Bracket,

        /// <summary>
        /// Comma
        /// </summary>
        Comma,

        /// <summary>
        /// Variable
        /// </summary>
        Variable,

        /// <summary>
        /// Primary
        /// </summary>
        Primary,

        /// <summary>
        /// Function
        /// </summary>
        Function,

        /// <summary>
        /// Not "!"
        /// </summary>
        Not,

        /// <summary>
        /// Bit Not "~"
        /// </summary>
        BitNot,

        /// <summary>
        /// Negative
        /// </summary>
        Negative,

        /// <summary>
        /// Positive
        /// </summary>
        Positive,

        /// <summary>
        /// Dereference "*"
        /// (the dereference operator, which allows reading and writing to a pointer)
        /// </summary>
        Dereference,
    }

    /// <summary>
    /// Interface for all tokens
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// The token type
        /// </summary>
        TokenType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Token text
        /// </summary>
        string Text
        {
            get;
            set;
        }
    }
}
