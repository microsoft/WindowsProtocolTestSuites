// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    /// <summary>
    /// This class provides primitives to declare how value (parameter) combinations shall
    /// be produced. 
    /// </summary>
    /// <remarks>
    public static class Combination
    {
        /// <summary>
        /// Declares that the given variables, parameters, or general expressions are in interaction, and the full cartesian product 
        /// space should be produced for their possible values under the constraint.
        /// </summary>
        /// <param name="values">The variables, parameters, or expressions which are in interaction.</param>
        /// <remarks>
        public static void Interaction(params object[] values)
        {
            
        }

        /// <summary>
        /// Defines all given parameters to be in pairwise interaction. Pairwise(x,y,z)
        /// is a shortcut for Interaction(x, y); Interaction(y,z);Interaction(z, x).
        /// </summary>
        /// <param name="values">The values which are in pairwise interaction.</param>
        /// <exception cref="ArgumentException">
        /// throw ArgumentException if length of values is less than 2.
        /// </exception>
        public static void Pairwise(params object[] values)
        {
            
        }

        /// <summary>
        /// Defines all given parameters to be in n-wise interaction. 
        /// </summary>
        /// <param name="n">The interaction strength</param>
        /// <param name="values">The values which are in n-wise interaction.</param>
        /// <exception cref="ArgumentException">
        /// throw ArgumentException if length of values is less than interaction order.
        /// </exception>
        public static void NWise(int n, params object[] values)
        {
            
        }

        /// <summary>
        /// Declares that a combination should be generated which satisfies the
        /// given isolation condition but none of the other isolation condition. 
        /// </summary>
        /// <param name="conditions">The isolation condition.</param>
        /// <remarks>
        public static void Isolated(params bool[] conditions)
        {
            
        }

        /// <summary>
        /// Explicitly invokes generation of combinations at given execution point.
        /// </summary>
        /// <param name="values">The values.</param>
        public static void Expand(params object[] values)
        {
            
        }
    }
}
