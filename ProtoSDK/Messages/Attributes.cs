// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    /// <summary>
    /// Attribute indicates that method or property has Rule configured.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor,AllowMultiple = false, Inherited = true)]
    public sealed class RuleAttribute : Attribute
    {
        public RuleAttribute()
        {
        }

        public string Action { get; set; }

        public string ModeTransition { get; set; }
    }

    /// <summary>
    /// Attribute indicates that parameter-free method or property is an accepting state condition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property,
                    AllowMultiple = false, Inherited = false)]
    public sealed class AcceptingStateConditionAttribute : Attribute
    {
    }
}
