// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace BKUPParser
{
    /// <summary>
    /// BKUP Parser Interface, exposes the ValidateBKUPDataBuffer method.
    /// </summary>
    public interface IBKUPParser
    {
        /// <summary>
        /// This method validates the BKUP Data Buffer
        /// </summary>
        /// <param name="dataBuffer">
        /// Input Parameter, Data Buffer to be parsed.
        /// </param>
        /// <param name="flatData">
        /// Output Parameter, the filled FlatDataStream
        /// structure which contains various backup streams.
        /// </param>
        /// <returns></returns>
        bool ValidateBKUPDataBuffer(byte[] dataBuffer, out FlatDataStream flatData);
    }
}
