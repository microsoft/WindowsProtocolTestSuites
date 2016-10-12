// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.CommonStack
{
    using System;

    /// <summary>
    /// The base class for marking objects that should be acted upon after a given delay.
    /// </summary>
    public abstract class Delayed : IComparable<Delayed>
    {
        /// <summary>
        /// Returns the remaining delay associated with this object in milliseconds.
        /// </summary>
        /// <returns> Returns the remaining delayin milliseconds. 
        /// Zero or negative values indicate that the delay has already elapsed.</returns>
        public abstract double GetDelay();

        /// <summary>
        /// Compare the delays of two objects.
        /// </summary>
        /// <param name="o"> Delayed objects.</param>
        /// <returns> Returns the result. 0 if equal, -1 if o has smaller delay, 1 if o has larger delay.</returns>
        public abstract int CompareTo(Delayed o);
    }
}
