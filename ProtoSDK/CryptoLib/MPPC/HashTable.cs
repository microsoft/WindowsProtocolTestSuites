// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Mppc
{
    /// <summary>
    /// A hash table for fast searching in sliding window.
    /// it will establish index for every three characters in slidingWindow
    /// </summary>
    internal class HashTable
    {
        /// <summary>
        /// sliding window, for fast access and space saving, we put it as a member of the class
        /// </summary>
        private SlidingWindow slidingWindow;

        /// <summary>
        /// 3 bytes string has at most 256*256*256 keys
        /// choosing 8192 has no special meaning.
        /// </summary>
        private const int hashTableSize = 8192;

        private List<int>[] table = new List<int>[hashTableSize];

        //the bit count the input data will shift when computing hash
        private const int hashComputingShiftCount = 3;

        //the length of the hash key
        private const int hashKeyLength = 3;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="slidingWindow">the target sliding window</param>
        public HashTable(SlidingWindow slidingWindow)
        {
            this.slidingWindow = slidingWindow;
            for (int i = 0; i < table.Length; i++)
            {
                table[i] = new List<int>();
            }
        }


        /// <summary>
        /// Update the hash table when some data need to be add to sliding window
        /// </summary>
        /// <param name="moveInCharacters">the data needed to be add to sliding window</param>
        public void Update(byte[] moveInCharacters)
        {
            //do nothing if it is null
            if (moveInCharacters == null)
            {
                return;
            }

            //doesn't update until there will be at least 3 characters in slidingWindow after updating.
            if ((slidingWindow.Count + moveInCharacters.Length) < hashKeyLength)
            {
                return;
            }

            // when slidingWindow's removedCharactor reach its upper limit ,we need to update all values, and set 
            // removedCharactor to zero, otherwise it will product unexpect problem.
            if (slidingWindow.removedBytesCount > (int.MaxValue - (2 * slidingWindow.MaxCount)))
            {
                UpdateAllValues();
            }

            int unHashedDataCount = 0;

            //we will add hash for last two Characters and the moveInCharacters if last two Characters exist.
            //otherwise we add hash for all the characters in the slidingWindow whose count will be less than two,
            //also concentrate moveInCharacters.
            if (slidingWindow.Count >= (hashKeyLength -1))
            {
                unHashedDataCount = hashKeyLength -1;
            }
            else
            {
                unHashedDataCount = slidingWindow.Count;
            }

            byte[] unHashedCharacters = new byte[unHashedDataCount + moveInCharacters.Length];
            //copy character in slidingWindow need to be added to hash table.
            for (int i = 0; i < unHashedDataCount; i++)
            {
                unHashedCharacters[i] = slidingWindow[slidingWindow.Count 
                    - unHashedDataCount 
                    + i];
            }
            Array.Copy(moveInCharacters, 0, unHashedCharacters, 
                unHashedDataCount, moveInCharacters.Length);

            AddHashes(unHashedCharacters, unHashedDataCount);

            byte[] redundantCharacters = null;
            if ((slidingWindow.Count + moveInCharacters.Length) > slidingWindow.MaxCount)
            {
                int redundantCharactersCount= slidingWindow.Count + moveInCharacters.Length 
                    - slidingWindow.MaxCount;

                //plus 2, for the last two characters which will be removed, 
                //we need to know 2 more characters after them to compute their hash.
                redundantCharactersCount = redundantCharactersCount + hashKeyLength -1;
                redundantCharacters = new byte[redundantCharactersCount];

                //if move out characters' count is larger than current slidingWindows count,
                //we need concentrate some leading characters of moveInCharacters.
                if (redundantCharactersCount > slidingWindow.Count)
                {
                    for (int i = 0; i < slidingWindow.Count; i++)
                    {
                        redundantCharacters[i] = slidingWindow[i];
                    }
                    Array.Copy(moveInCharacters, 0, redundantCharacters,
                        slidingWindow.Count, redundantCharactersCount - slidingWindow.Count);
                }
                else
                {
                    for (int i = 0; i < redundantCharacters.Length; i++)
                    {
                        redundantCharacters[i] = slidingWindow[i];
                    }
                }

            }

            if (redundantCharacters != null)
            {
                RemoveHashes(redundantCharacters);
            }
        }


        /// <summary>
        /// Clear the hash table
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < table.Length; i++)
            {
                table[i].Clear();
            }
        }


        /// <summary>
        /// get the position where the three bytes string is matched in sliding window
        /// </summary>
        /// <param name="threeBytesData">three bytes data as a key</param>
        /// <returns>the positions where the key matches</returns>
        public int[] GetKeyMatchPositions(byte[] threeBytesData)
        {
            if (threeBytesData == null)
            {
                throw new ArgumentNullException("threeBytesData");
            }

            if (threeBytesData.Length != 3)
            {
                throw new ArgumentException("the input data should be three bytes", "threeBytesData");
            }

            List<int> matchedPositions = new List<int>();
            int key = ComputeHash(threeBytesData);
            List<int> values = table[key];
            for (int i = 0; i < values.Count; i++)
            {
                int j = 0;
                for (; j < threeBytesData.Length; j++)
                {
                    if (threeBytesData[j] != slidingWindow[values[i] - slidingWindow.removedBytesCount + j])
                    {
                        break;
                    }
                }
                if (j == threeBytesData.Length)
                {
                    matchedPositions.Add(values[i] - slidingWindow.removedBytesCount);
                }
            }

            return matchedPositions.ToArray();
        }


        /// <summary>
        /// compute hash for three bytes string
        /// </summary>
        /// <param name="threeBytesData">three bytes data</param>
        /// <returns></returns>
        private static int ComputeHash(byte[] threeBytesData)
        {
            if ((threeBytesData == null) || (threeBytesData.Length != hashKeyLength))
            {
                throw new ArgumentException("Length of threeBytesData must be three.", "threeBytesData");
            }

            int key = 0;
            for (int i = 0; i < threeBytesData.Length; i++)
            {
                key = (key << hashComputingShiftCount) ^ threeBytesData[i];
                key = key % hashTableSize;
            }

            return key;
        }


        /// <summary>
        /// add hash, value to hash table
        /// </summary>
        /// <param name="data">the data which will be sliced into some 3 bytes string,
        /// and add the position where it starts to the hash table</param>
        /// <param name="hashDataCount">the byte counts which need to 
        /// be added to hash table in the sliding window</param>
        private void AddHashes(byte[] data, int hashDataCount)
        {
            //we don't compute hash for last two characters
            for (int i = 0; i < (data.Length - (hashKeyLength - 1)); i++)
            {
                byte[] threeBytesData = new byte[hashKeyLength];
                Array.Copy(data, i, threeBytesData, 0, threeBytesData.Length);
                int hashKey = ComputeHash(threeBytesData);
                table[hashKey].Add(slidingWindow.Count + slidingWindow.removedBytesCount + i - hashDataCount);
            }
        }


        /// <summary>
        /// remove values from hash table
        /// </summary>
        /// <param name="data">data whose index of the sliding window 
        /// will be removed</param>
        private void RemoveHashes(byte[] data)
        {
            for (int i = 0; i < (data.Length - (hashKeyLength -1)); i++)
            {
                byte[] threeBytesData = new byte[hashKeyLength];
                Array.Copy(data, i, threeBytesData, 0, threeBytesData.Length);
                int hashKey = ComputeHash(threeBytesData);
                table[hashKey].Remove(slidingWindow.removedBytesCount + i);
            }
        }


        /// <summary>
        /// modify all values in this hash table, 
        /// we only do this when sliding Window's removedCharactor reaches its limit for 
        /// pefermance concern.
        /// </summary>
        private void UpdateAllValues()
        {
            for (int i = 0; i < table.Length; i++)
            {
                for (int j = 0; j < table[i].Count; j++)
                {
                    table[i][j] -= slidingWindow.removedBytesCount;
                }
            }
            slidingWindow.removedBytesCount = 0;
        }
    }
}
