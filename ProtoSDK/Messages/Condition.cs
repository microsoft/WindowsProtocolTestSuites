// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    /// <summary>
    /// Conditions are evaluated as part of exploration.
    /// They determine whether a model rule is enabled and they impose constraints on parameter values to be generated.
    /// </summary>
    public static class Condition
    {
        public static void IsTrue(bool condition)
        {
            if (!condition)
                throw new AssumeFailedException();
        }

        public static void IsTrue(bool condition, string requirement)
        {
            if (null == requirement)
                throw new ArgumentNullException("requirement");
            if (!condition)
                throw new AssumeFailedException();
            Requirement.Capture(requirement);
        }

        public static void IsFalse(bool condition)
        {
            if (condition)
                throw new AssumeFailedException();
        }

        public static void Fail()
        {
            throw new AssumeFailedException();
        }

        public static void IfThen(bool antecedent, bool consequent)
        {
            IsTrue(!antecedent | consequent);
        }

        public static void In<T>(T element, params T[] values)
        {
            for (int i = 0; i < values.Length; i++)
                if (BuiltinEquals(false, element, values[i]))
                    return;

            throw new AssumeFailedException();
        }

        public static void IsNull(object value)
        {
            if (null != value)
                throw new AssumeFailedException();
        }

        public static void IsNotNull(object value)
        {
            if (null == value)
                throw new AssumeFailedException();
        }

        public static bool BuiltinEquals(bool respectBoxIdentity, object value1, object value2)
        {
            if (value1 == null)
                return value2 == null;
            else if (value2 == null)
                return false;
            Type t = value1.GetType();
            if (t != value2.GetType())
                return false;
            else
                return Object.ReferenceEquals(value1, value2);
        }
    }

    /// <summary>
    /// An exception which is thrown when an assumption fails.
    /// </summary>
    [Serializable]
    public class AssumeFailedException : Exception
    {
        public AssumeFailedException() { }

        public AssumeFailedException(string message)
            : base("assumption failure: " + message) { }

        public AssumeFailedException(string message, Exception innerException)
            : base("assumption failure: " + message, innerException)
        {
        }
    }
}
