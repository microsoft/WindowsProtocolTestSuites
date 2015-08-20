// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Mppc
{
    internal class SlidingWindow
    {
        // we don't want to update all the values in hash table 
        // when some bytes is removed from the sliding window for performance,
        // so we add this field to get absolute position. 
        // this field will be modify by sliding window itself and the hash table. 
        internal int removedBytesCount;

        private int maxCount;
        private int startIndex;
        private int endIndex;
        private byte[] buffer;


        /// <summary>
        /// the bytes count the sliding window currently has
        /// </summary>
        public int Count
        {
            get
            {
                return (endIndex + buffer.Length - startIndex) % buffer.Length;
            }

        }


        /// <summary>
        /// the max bytes count of sliding window
        /// </summary>
        public int MaxCount
        {
            get 
            { 
                return maxCount; 
            }
        }


        /// <summary>
        /// Get data in the indexer position
        /// </summary>
        /// <param name="index">the position</param>
        /// <returns>data</returns>
        public byte this[int index]
        {
            get 
            {
                // this is a fix for windows implemeted mppc
                if (index < 0)
                {
                    index = index & (maxCount - 1);
                }

                return buffer[(index + startIndex) % buffer.Length]; 
            }
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="slidingWindowSize">slidingWindow Size</param>
        public SlidingWindow(int slidingWindowSize)
        {
            maxCount = slidingWindowSize;
            // set buffer's length to slidingWindowSize+1, this will help 
            // us to differ full from empty.
            buffer = new byte[slidingWindowSize + 1];
        }


        /// <summary>
        /// update the sliding window when a character is being added to it.
        /// </summary>
        /// <param name="character">the character</param>
        public void Update(byte character)
        {
            if (this.Count == this.MaxCount)
            {
                MoveIndexOneStep(ref startIndex);
                this.removedBytesCount++;
            }
            buffer[endIndex] = character;
            MoveIndexOneStep(ref endIndex);
        }


        /// <summary>
        /// update the sliding window when a sequence of character is being added to it.
        /// </summary>
        /// <param name="data">the data which will be written to history buffer</param>
        public void Update(byte[] data)
        {
            if (data == null)
            {
                return;
            }

            for (int i = 0; i < data.Length; i++)
            {
                Update(data[i]);
            }
        }


        /// <summary>
        /// clear sliding window
        /// </summary>
        public void Clear()
        {
            removedBytesCount = 0;
            startIndex = 0;
            endIndex = 0;
        }


        /// <summary>
        /// move start Index or end Index by one
        /// </summary>
        /// <param name="index">the index</param>
        private void MoveIndexOneStep(ref int index)
        {
            index = (index + 1) % buffer.Length;
        }


        /// <summary>
        /// get the data in history buffer referred by offset and length
        /// </summary>
        /// <param name="offset">mppc offset</param>
        /// <param name="length">mppc length</param>
        /// <returns>the data gotten</returns>
        public byte[] GetMatchedData(uint offset, uint length)
        {
            byte[] Matched = new byte[length];

            if (length <= offset)
            {
                for (int i = 0; i < Matched.Length; i++)
                {
                    Matched[i] = this[this.Count - (int)offset + i];
                }
            }
            else
            {
                // if length > offset
                byte[] dataArray = new byte[offset];
                for (int i = 0; i < offset; i++)
                {
                    dataArray[i] = this[this.Count - (int)offset + i];
                }
                for (int i = 0; i < Matched.Length; i++)
                {
                    Matched[i] = dataArray[i % offset];
                }
            }

            return Matched;
        }
    }
}
