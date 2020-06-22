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
        #region Fields and Constructors

        /// <summary>
        /// Constructs a rule attribute with default action pattern and empty mode transformation.
        /// The default action pattern is automatically built from method signature.
        /// </summary>
        public RuleAttribute()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The action pattern in cord syntax.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Indicates the mode transition.
        /// A mode transformation in a form like "s1,...,sn -> t1,...,tm", where s and t are mode tags.
        /// </summary>
        public string ModeTransition { get; set; }

        #endregion
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
