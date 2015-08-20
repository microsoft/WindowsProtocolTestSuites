// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// A ReadOnlyDictionary wrappers Dictionary, but it only can be read.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the ReadOnlyDictionary</typeparam>
    /// <typeparam name="TValue">The type of the values in the ReadOnlyDictionary</typeparam>
    public class ReadOnlyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private Dictionary<TKey, TValue> wrappedDictionary;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dictionary">The dictionary to be wrapped</param>
        public ReadOnlyDictionary(Dictionary<TKey, TValue> dictionary)
        {
            this.wrappedDictionary = dictionary;
        }


        /// <summary>
        /// Get the value indexed by the key
        /// </summary>
        /// <param name="key">The key used to lookup the value</param>
        /// <returns>The found value</returns>
        public TValue this[TKey key]
        {
            get
            {
                return wrappedDictionary[key];
            }
        }

        /// <summary>
        /// Test if the key exist in this collection
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>True indicate the key exist in the collection, otherwise false</returns>
        public bool ContainsKey(TKey key)
        {
            return wrappedDictionary.ContainsKey(key);
        }


        /// <summary>
        /// Returns an enumerator that iterates through the System.Collections.Generic.Dictionary(TKey,TValue).
        /// </summary>
        /// <returns> A System.Collections.Generic.Dictionary(TKey,TValue).Enumerator structure
        /// for the System.Collections.Generic.Dictionary(TKey,TValue).</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return wrappedDictionary.GetEnumerator();
        }


        /// <summary>
        /// Hide the IEnumerable.GetEnumrator interface
        /// </summary>
        /// <returns>The method is hidden</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return wrappedDictionary.GetEnumerator();
        }


        /// <summary>
        /// get the number of key/value pairs contained in the dictionary.
        /// </summary>
        public int Count
        {
            get
            {
                return wrappedDictionary.Count;
            }
        }


        /// <summary>
        /// gets a collection containing the values in the dictionary.
        /// </summary>
        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                return wrappedDictionary.Values;
            }
        }
    }
}
