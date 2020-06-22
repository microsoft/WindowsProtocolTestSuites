// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    /// <summary>
    /// This class declare how value (parameter) combinations shall be produced. 
    /// </summary>
    /// <remarks>
    public static class Combination
    {
        public static void Interaction(params object[] values)
        {
            
        }

        public static void Pairwise(params object[] values)
        {
            
        }

        public static void NWise(int n, params object[] values)
        {
            
        }

        public static void Isolated(params bool[] conditions)
        {
            
        }

        public static void Expand(params object[] values)
        {
            
        }
    }
}
