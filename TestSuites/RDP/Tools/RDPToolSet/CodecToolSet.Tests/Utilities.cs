// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodecToolSet.Tests
{
    internal static class Utility
    {
        public static byte[] RandomByteArray()
        {
            var array = new byte[64 * 64];
            var random = new Random(DateTime.Now.Second);
            random.NextBytes(array);
            return array;
        }
    }
}
