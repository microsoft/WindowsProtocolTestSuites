// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Mppc
{
    /// <summary>
    /// specify how many bits literal occupies
    /// </summary>
    internal enum LiteralBitSize
    {
        LessThanHex80 = 8,
        MoreThanHex7f = 9,
    }

    /// <summary>
    /// specify how many bits literal occupies without its symbol
    /// </summary>
    internal enum LiteralBitSizeWithoutSymbol
    {
        LessThanHex80 = 8,
        MoreThanHex7f = 7,
    }

    internal enum LiteralSymbolBitSize
    {
        LessThanHex80 = 0,
        MoreThanHex7f = 2,
    }

    /// <summary>
    /// literal symbol values
    /// </summary>
    internal enum LiteralSymbol
    {
        LessThanHex80 = 0x0,
        MoreThanHex7f = 0x2,
    }

    /// <summary>
    /// the value that needs to be minused or plused when encoding or decoding literal 
    /// </summary>
    internal enum LiteralAdding
    {
        LessThanHex80 = 0,
        MoreThanHex7f = 0x80,
    }

    /// <summary>
    /// specify how many bits "offset" occupies when using 8k sliding window.
    /// </summary>
    internal enum OffsetBitSize8k
    {
        LessThan64 = 10,
        Between64And320 = 12,
        Between320And8191 = 16,
    }

    /// <summary>
    /// specify how many bits the data part of "offset" occupies when using 8k sliding window
    /// </summary>
    internal enum OffsetBitSizeWithoutSymbol8k
    {
        LessThan64 = 6,
        Between64And320 = 8,
        Between320And8191 = 13,
    }

    /// <summary>
    /// specify how many bits offset's symbol occupies when using 8k sliding window
    /// </summary>
    internal enum OffsetSymbolBitSize8k
    {
        LessThan320 = 4,
        Between320And8191 = 3,
    }

    /// <summary>
    /// offset Symbol value for 8k sliding window
    /// </summary>
    internal enum OffsetSymbol8k
    {
        LessThan64 = 0xf,
        Between64And320 = 0xe,
        Between320And8191 = 0x6,
    }

    /// <summary>
    /// the value need to be minused or plused when encoding or decoding offset for 8k sliding window
    /// </summary>
    internal enum OffsetAdding8k
    {
        LessThan64 = 0,
        Between64And320 = 64,
        Between320And8191 = 320,
    }

    /// <summary>
    /// specify how many bits offset's symbol occupies when using 64k sliding window
    /// </summary>
    internal enum OffsetSymbolBitSize64k
    {
        LessThan320 = 5,
        Between320And2367 = 4,
        LargerThan2367 = 3,
    }

    /// <summary>
    /// specify how many bits "offset" occupies when using 64k sliding window.
    /// </summary>
    internal enum OffsetBitSize64k
    {
        LessThan64 = 11,
        Between64And319 = 13,
        Between320And2367 = 15,
        LargerThan2367 = 19,
    }

    /// <summary>
    /// specify how many bits "offset" occupies without its symbol when using 64k sliding window
    /// </summary>
    internal enum OffsetBitSizeWithoutSymbol64k
    {
        LessThan64 = 6,
        Between64And319 = 8,
        Between320And2367 = 11,
        LargerThan2368 = 16,
    }

    /// <summary>
    /// offset symbol value for 64k sliding window
    /// </summary>
    internal enum OffsetSymbol64k
    {
        LessThan64 = 0x1f,
        Between64And319 = 0x1e,
        Between320And2367 = 0xe,
        LargerThan2368 = 0x6,
    }

    /// <summary>
    /// the value need to be minused or plused when encoding or decoding offset for 64k sliding window
    /// </summary>
    internal enum OffsetAdding64k
    {
        LessThan64 = 0,
        Between64And319 = 64,
        Between320And2367 = 320,
        LargerThan2368 = 2368,
    }

    /// <summary>
    /// specify how many bits "length" occupies.
    /// </summary>
    internal enum LengthBitSize
    {
        Between0And3 = 1,
        Between4And7 = 4,
        Between8And15 = 6,
        Between16And31 = 8,
        Between32And63 = 10,
        Between64And127 = 12,
        Between128And255 = 14,
        Between256And511 = 16,
        Between512And1023 = 18,
        Between1024And2047 = 20,
        Between2048And4095 = 22,
        Between4096And8191 = 24,
        Between8192And16383 = 26,
        Between16384And32767 = 28,
        Between32768And65535 = 30,
    }

    /// <summary>
    /// specify how many bits length's symbol occupies
    /// </summary>
    internal enum LengthSymbolBitSize
    {
        Between0And3 = 1,
        Between4And7 = 2,
        Between8And15 = 3,
        Between16And31 = 4,
        Between32And63 = 5,
        Between64And127 = 6,
        Between128And255 = 7,
        Between256And511 = 8,
        Between512And1023 = 9,
        Between1024And2047 = 10,
        Between2048And4095 = 11,
        Between4096And8191 = 12,
        Between8192And16383 = 13,
        Between16384And32767 = 14,
        Between32768And65535 = 15,
    }

    /// <summary>
    /// length symbol value
    /// </summary>
    internal enum LengthSymbol
    {
        Between0And3 = 0,
        Between4And7 = 0x2,
        Between8And15 = 0x6,
        Between16And31 = 0xe,
        Between32And63 = 0x1e,
        Between64And127 = 0x3e,
        Between128And255 = 0x7e,
        Between256And511 = 0xfe,
        Between512And1023 = 0x1fe,
        Between1024And2047 = 0x3fe,
        Between2048And4095 = 0x7fe,
        Between4096And8191 = 0xffe,
        Between8192And16383 = 0x1ffe,
        Between16384And32767 = 0x3ffe,
        Between32768And65535 = 0x7ffe,
    }

    /// <summary>
    /// specify how many bits "length" occupies without its symbol
    /// </summary>
    internal enum LengthBitSizeWithoutSymbol
    {
        Between0And3 = 0,
        Between4And7 = 2,
        Between8And15 = 3,
        Between16And31 = 4,
        Between32And63 = 5,
        Between64And127 = 6,
        Between128And255 = 7,
        Between256And511 = 8,
        Between512And1023 = 9,
        Between1024And2047 = 10,
        Between2048And4095 = 11,
        Between4096And8191 = 12,
        Between8192And16383 = 13,
        Between16384And32767 = 14,
        Between32768And65535 = 15,
    }
}
