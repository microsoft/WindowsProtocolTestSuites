// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Contains some common fields and properties for String types.
    /// </summary>
    /// <remarks>
    /// All ASN.1 Strings except for BIT STRING must be drived from this class.
    /// </remarks>
    public abstract class Asn1String : Asn1Object
    {
        /// <summary>
        /// Initializes a new instance of Asn1String.
        /// </summary>
        /// <remarks>
        /// Constraint will be set by reflection.
        /// </remarks>
        protected Asn1String()
        {
            SetConstraintsByReflection();
            List<char> list = new List<char>();
            string baseCandidates = Constraint == null || Constraint.PermittedCharSet == null
                ? TypeBuiltInCharSet
                : Constraint.PermittedCharSet;
            if (baseCandidates != AllCharSet)
            {
                Regex r = new Regex(string.Format("[{0}]", baseCandidates));
                bool[] flags = new bool[256*256];
                for (int i = 0; i < 256*256; i++)
                {
                    char c = Convert.ToChar(i);
                    if (flags[c] == false)
                    {
                        if (r.IsMatch("" + c))
                        {
                            list.Add(c);
                        }
                    }
                    flags[c] = true;
                }
                CharSetInArray = list.ToArray();
            }
            else
            {
                CharSetInArray = null;
            }
        }

        /// <summary>
        /// Records "lb", "ub" and permitted charset mentioned in X.691: 16
        /// </summary>
        protected internal Asn1StringConstraint Constraint;

        private void SetConstraintsByReflection()
        {
            Constraint = null;

            //Gets the upper and lower bound for the structure.
            object[] allAttributes = GetType().GetCustomAttributes(true);
            foreach (object o in allAttributes)
            {
                if (o is Asn1StringConstraint)
                {
                    Constraint = o as Asn1StringConstraint;
                    break;
                }
            }
        }

        /// <summary>
        /// Gets or sets to the data of this object.
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// Gets the built in charset of the string type, in string form or regex form.
        /// </summary>
        protected virtual string TypeBuiltInCharSet
        {
            get { return AllCharSet; }
        }

        private const string AllCharSet = @"\S\s";

        /// <summary>
        /// Gets an array that contains all the permitted characters.
        /// </summary>
        /// <remarks>
        /// If CharSetInArray is null, all characters are permitted.
        /// </remarks>
        protected char[] CharSetInArray;

        /// <summary>
        /// Ensures constraint is satisfied.
        /// </summary>
        /// <returns>True if constraint is satisfied, false if not.</returns>
        protected override bool VerifyConstraints()
        {
            if (Value == null)
            {
                return Constraint == null;
            }
            Regex r = new Regex(string.Format("^[{0}]*$",
                Constraint == null || Constraint.PermittedCharSet == null ? TypeBuiltInCharSet : Constraint.PermittedCharSet));
            return base.VerifyConstraints() &&
                (Constraint == null || ((!Constraint.HasMinSize || Value.Length >= Constraint.MinSize) &&
                   (!Constraint.HasMaxSize || Value.Length <= Constraint.MaxSize)))
                   && (r.IsMatch(Value));
        }

        #region overrode methods from System.Object

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if obj has same data with this instance. False if not.</returns>
        public override bool Equals(object obj)
        {
            // If parameter is null or cannot be cast to Asn1String return false.
            Asn1String p = obj as Asn1String;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match.
            return Value == p.Value;
        }

        /// <summary>
        /// Returns the hash code of the instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        #endregion overrode methods from System.Object
    }
}
