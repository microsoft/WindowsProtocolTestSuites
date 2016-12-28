// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Protocols.TestTools;

namespace FileStreamDataParser
{
    /// <summary>
    /// This interface exposes the ValidateDataBuffer method which
    /// validates the data buffer obtained from InitializeFileTransferAsync
    /// opnum of MS-FRS2.
    /// </summary>
    public interface IFSCCAdapter : IAdapter
    {
        /// <summary>
        /// This method validates the data buffer obtained from 
        /// InitializeFileTransferAsync opnum of MS-FRS2.
        /// </summary>
        /// <param name="dataBuffer">
        /// Input byte array to be parsed.
        /// </param>
        /// <param name="objDataBuffer">
        /// Returns a ReplicatedFileStructure.
        /// </param>
        /// <returns></returns>
        bool ValidateDataBuffer(byte[] dataBuffer, out ReplicatedFileStructure objDataBuffer);      
    }
}
