// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    /// <summary>
    /// An attribute which indicates that an instance method of a derived class is an initializer. 
    /// This initializer is going to be called by MessageUtils.Create.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class InitializerAttribute : Attribute
    { }

    /// <summary>
    /// An attribute which indicates that an instance method of a derived class is a validator. 
    /// This validator is going to be called by MessageUtils.Validate.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ValidatorAttribute : Attribute
    { }
}
