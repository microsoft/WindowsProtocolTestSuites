// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// The CCoercionRestriction structure contains the modifier and rank coercion operation.
    /// </summary>
    public struct CCoercionRestriction : IWspStructure
    {
        /// <summary>
        /// An IEEE 32-bit floating point number [IEEE754] representing the coercion value upon which the rank coercion operation happens.
        /// Note that the coercion operation is determined by the containing CRestriction structure.
        /// </summary>
        public Single _flValue;

        /// <summary>
        /// CRestriction structure that specifies a command tree.
        /// The returned rank value for results of a child restriction will be coerced as specified by the containing CRestriction structure.
        /// </summary>
        public CRestriction _childRes;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(_flValue);

            _childRes.ToBytes(buffer);
        }
    }
}
