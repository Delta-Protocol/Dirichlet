﻿using System.Collections;
using System.Collections.Generic;

namespace Decompose.Numerics
{
    public class Word32BitArray : IEnumerable<bool>
    {
        private const int wordLength = 32;

        private int length;
        private int words;
        private int[] bits;

        public int Length
        {
            get { return length; }
        }

        public Word32BitArray(int length)
        {
            this.length = length;
            words = (length + wordLength - 1) / wordLength;
            bits = new int[words];
        }

        public bool this[int j]
        {
            get
            {
                return (bits[j / wordLength] & 1 << j % wordLength) != 0;
            }
            set
            {
                if (value)
                    bits[j / wordLength] |= 1 << j % wordLength;
                else
                    bits[j / wordLength] &= ~(1 << j % wordLength);
            }
        }

        public IEnumerator<bool> GetEnumerator()
        {
            for (int i = 0; i < length; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}