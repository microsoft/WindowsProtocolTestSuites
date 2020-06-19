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
        /// <summary>
        /// Establishes a condition that must be true for a step to be produced.
        /// </summary>
        /// <param name="condition">Condition that needs to be true.</param>
        public static void IsTrue(bool condition)
        {
            if (!condition)
                throw new AssumeFailedException();
        }

        /// <summary>
        /// Establishes a condition that must be true for a step to be produced and a requirement to be captured.
        /// </summary>
        /// <param name="condition">Condition that needs to be true</param>
        /// <param name="requirement">Requirement to be captured if the condition holds.</param>
        public static void IsTrue(bool condition, string requirement)
        {
            if (null == requirement)
                throw new ArgumentNullException("requirement");
            if (!condition)
                throw new AssumeFailedException();
            Requirement.Capture(requirement);
        }

        /// <summary>
        /// Establishes a condition that must be false for a step to be produced.
        /// </summary>
        /// <param name="condition">Condition that needs to be false.</param>
        public static void IsFalse(bool condition)
        {
            if (condition)
                throw new AssumeFailedException();
        }

        /// <summary>
        /// Prevents a step from being produced.
        /// </summary>
        public static void Fail()
        {
            throw new AssumeFailedException();
        }

        /// <summary>
        /// Establishes the condition that the consequent must be true whenever the antecedent is true for a step to be produced. 
        /// Equivalent to IsTrue(!antecedent | consequent).
        /// </summary>
        /// <param name="antecedent">Condition to be evaluated.</param>
        /// <param name="consequent">Condition that must be true when the antecedent is true.</param>
        public static void IfThen(bool antecedent, bool consequent)
        {
            IsTrue(!antecedent | consequent);
        }

        /// <summary>
        /// Establishes the condition that one value must be a member of an enumeration for a step to be produced.
        /// </summary>
        /// <example>
        /// The following example associates the name parameter with the set of values that follow:
        /// <code>
        /// Condition.In&lt;string&gt;(name, "@$^", "t.cmd", "t.exe");
        /// </code>
        /// </example>
        /// <typeparam name="T">Type of the element.</typeparam>
        /// <param name="element">Element that needs to be in the enumeration.</param>
        /// <param name="values">Enumeration that needs to contain the element.</param>
        public static void In<T>(T element, params T[] values)
        {
            for (int i = 0; i < values.Length; i++)
                if (BuiltinEquals(false, element, values[i]))
                    return;

            throw new AssumeFailedException();
        }

        /// <summary>
        /// Establishes the condition that the specified object must be a null reference for a step to be produced.
        /// </summary>
        /// <param name="value">Value to verify is a null reference.</param>
        public static void IsNull(object value)
        {
            if (null != value)
                throw new AssumeFailedException();
        }

        /// <summary>
        /// Establishes the condition that the specified object must not be a null reference for a step to be produced.
        /// </summary>
        /// <param name="value">Value to verify is not a null reference.</param>
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
        /// <summary>
        /// Create an assumption failure exception.
        /// </summary>
        public AssumeFailedException() { }

        /// <summary>
        /// Create an assumption failure exception.
        /// </summary>
        /// <param name="message"></param>
        public AssumeFailedException(string message)
            : base("assumption failure: " + message) { }

        /// <summary>
        /// Create an assumption failure exception with with a specified 
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AssumeFailedException(string message, Exception innerException)
            : base("assumption failure: " + message, innerException)
        {
        }
    }
}
