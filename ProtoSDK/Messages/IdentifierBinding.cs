// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Messages;
using System.Globalization;

namespace Microsoft.Protocols.TestTools
{
    /// <summary>
    /// An auxiliary class for implementing abstract identifier bindings.
    /// </summary>
    /// <typeparam name="Target">The target type to which the abstract identifier is bound.</typeparam>
    public class IdentifierBinding<Target>
    {
        Dictionary<int, Target> forwardMap = new Dictionary<int,Target>();
        Dictionary<Target, int> backwardMap = new Dictionary<Target,int>();
        IRuntimeHost host;
        int maxIdUsed;

        private readonly object identifierBindingLock = new object();

        /// <summary>
        /// Constructs an identifier binding instance. 
        /// A test site must be passed so that the binding can generate assertions and log entries.
        /// </summary>
        /// <param name="host">The message runtime host.</param>
        public IdentifierBinding(IRuntimeHost host)
        {
            this.host = host;
        }

        /// <summary>
        /// Constructs an identifier binding instance. 
        /// Only use when no test site is needed.
        /// </summary>
        public IdentifierBinding()
        {
        }

        /// <summary>
        /// Resets the identifier binding.
        /// This method clears all previous definitions.
        /// </summary>
        public void Reset()
        {
            lock (identifierBindingLock)
            {
                forwardMap.Clear();
                backwardMap.Clear();
                maxIdUsed = 0;
            }
        }

        /// <summary>
        /// Binds the given identifier to the given target.
        /// If the identifier is already mapped to a different target, 
        /// or if the target is already mapped to a different identifier,
        /// an assertion failure is raised on the test site.
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <param name="target">The target</param>
        public void Bind(int id, Target target)
        {
            lock (identifierBindingLock)
            {
                Target oldTarget;
                int oldId;
                if (forwardMap.TryGetValue(id, out oldTarget))
                {
                    if (!Object.Equals(oldTarget, target))
                    {
                        if (host != null)
                        {
                            host.Assert(false, "failed binding identifier '{0}' to '{1}': identifier already bound to '{2}'",
                                    id, target, oldTarget); 
                        }
                        else
                            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "failed binding identifier '{0}' to '{1}': identifier already bound to '{2}'",
                                    id, target, oldTarget));
                    }
                }
                else
                {
                    forwardMap[id] = target;
                    if (id > maxIdUsed)
                        maxIdUsed = id;
                }
                if (backwardMap.TryGetValue(target, out oldId))
                {
                    if (id != oldId)
                    {
                        if (host != null)
                        {
                            host.Assert(false, "failed binding identifier '{0}' to '{1}': target already uses identifier '{2}'",
                                        id, target, oldId);
                        }
                        else
                            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "failed binding identifier '{0}' to '{1}': target already uses identifier '{2}'",
                                        id, target, oldId));
                    }
                }
                else
                    backwardMap[target] = id;
            }
        }

        /// <summary>
        /// Unbinds the given identifier.
        /// This method does nothing if the identifier is not bound.
        /// </summary>
        /// <param name="id">The identifier</param>
        public void Unbind(int id)
        {
            lock (identifierBindingLock)
            {
                Target oldTarget;
                if (forwardMap.TryGetValue(id, out oldTarget))
                {
                    forwardMap.Remove(id);
                    backwardMap.Remove(oldTarget);
                }
            }
        }
 
        /// <summary>
        /// Gets the identifier which is associated with the given target. 
        /// An assertion failure is raised on test site if the target has no binding.
        /// </summary>
        /// <param name="target">The target</param>
        /// <returns>The identifier of the target</returns>
        public int GetIdentifier(Target target)
        {
            lock (identifierBindingLock)
            {
                int id = 0;
                if (!backwardMap.TryGetValue(target, out id))
                {
                    if (host != null)
                    {
                        host.Assert(false, "failed resolving target '{0}': not bound to identifier", target);
                    }
                    else
                        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "failed resolving target '{0}': not bound to identifier", target));
                }
                return id;
            }
        }

        /// <summary>
        /// Gets the identifier associated with the given target, or creates a new identifier
        /// if target not bound.
        /// </summary>
        /// <param name="target">The target</param>
        /// <returns>The identifier of the target</returns>
        public int GetOrCreateIdentifier(Target target)
        {
            lock (identifierBindingLock)
            {
                if (!IsTargetBound(target))
                    Bind(this.GetUnusedIdentifier(), target);
                return GetIdentifier(target);
            }
        }

        /// <summary>
        /// Gets the target which is associated with the given identifier. 
        /// An assertion failure is raised on test site if the identifier has no binding.
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>The target</returns>
        public Target GetTarget(int id)
        {
            lock (identifierBindingLock)
            {
                Target t = default(Target);
                if (!forwardMap.TryGetValue(id, out t))
                {
                    if (host != null)
                    {
                        host.Assert(false, "failed resolving identifier '{0}': not bound to target", id);
                    }
                    else
                        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "failed resolving identifier '{0}': not bound to target", id));
                }
                return t;
            }
        }

        /// <summary>
        /// Checks if the identifier is bound.
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>Returns true if the identifier is bound.</returns>
        public bool IsIdentifierBound(int id)
        {
            lock (identifierBindingLock)
            {
                return forwardMap.ContainsKey(id);
            }
        }

        /// <summary>
        /// Checks if the target is bound.
        /// </summary>
        /// <param name="target">The target</param>
        /// <returns>Returns true if the target is bound.</returns>
        public bool IsTargetBound(Target target)
        {
            lock (identifierBindingLock)
            {
                return backwardMap.ContainsKey(target);
            }
        }

        /// <summary>
        /// Gets a value for an identifier which is not used in the binding.
        /// This method can be used for creating a new binding.
        /// </summary>
        /// <returns>Returns the unused identifier</returns>
        // The following suppression is adopted because the method GetUnusedIdentifier() performs a conversion of maxIdUsed field.
        // In this case, method is preferable to property.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public int GetUnusedIdentifier()
        {
            lock (identifierBindingLock)
            {
                return maxIdUsed + 1;
            }
        }

        /// <summary>
        /// Gets the current binding as a dictionary.
        /// </summary>
        public IDictionary<int, Target> Dictionary
        {
            get { return forwardMap; }
        }

    }
}
