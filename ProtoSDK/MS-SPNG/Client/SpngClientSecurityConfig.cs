// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    public class SpngClientSecurityConfig: SpngConfig
    {
        /// <summary>
        /// Constructor, use the constructor of base class
        /// </summary>
        /// <param name="attributes">Security context attributes</param>
        /// <param name="mechList">The supported MechType list</param>
        /// <exception cref="ArgumentNullException">mechList must not be null</exception>
        public SpngClientSecurityConfig(
            ClientSecurityContextAttribute attributes,
            MechTypeList mechList)
            : base((uint)attributes, mechList)
        {
            if (mechList == null)
            {
                throw new ArgumentNullException("mechList");
            }
        }
    }
}
