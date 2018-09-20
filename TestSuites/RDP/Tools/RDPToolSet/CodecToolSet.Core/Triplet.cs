using System;
using System.Collections;
using System.Collections.Generic;

namespace CodecToolSet.Core
{
    public sealed class Triplet<T> : IEnumerable<T>
    {
        #region Properties

        // three dimensions
        public T X { get; set; }
        public T Y { get; set; }
        public T Z { get; set; }

        #endregion

        #region Constructors

        public Triplet()
        {
        }

        public Triplet(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        //public Triplet(T[] array)
        //{
        //    if (array.Length > 0) X = array[0];
        //    if (array.Length > 1) X = array[1];
        //    if (array.Length > 2) X = array[2];
        //} 

        #endregion

        #region Indexer

        public T this[TripletDimension d]
        {
            get {
                switch (d) {
                        case TripletDimension.X: return X;
                        case TripletDimension.Y: return Y;
                        case TripletDimension.Z: return Z;
                        default: return default(T);
                }
            }
        }

        #endregion

        #region IEnumerable

        // user can use foreach to loop all dimensions 
        public IEnumerator<T> GetEnumerator()
        {
            yield return X;
            yield return Y;
            yield return Z;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    public enum TripletDimension 
    {
        X = 0,
        Y = 1,
        Z = 2
    }
}