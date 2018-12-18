using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CodecDebugTool.Core
{
    // output to any types 
    public partial class Tile
    {
        public string GetString()
        {
            throw new NotImplementedException();
        }

        public string[] GetStrings()
        {
            throw new NotImplementedException();
        }

        public Stream GetStream()
        {
            throw new NotImplementedException();
        }

        public Bitmap GetBitmap()
        {
            throw new NotImplementedException();
        }

        public void SaveToFile(string path)
        {
            throw new NotImplementedException();
        }

        public Triplet<T> GetTriplet<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetArray<T>()
        {
            throw new NotImplementedException();
        }

        public void GetMatrices<T>(out T[,] x, out T[,] y,out T[,] z)
        {
            throw new NotImplementedException();
        }

        public void GetVectors<T>(out T[] x, out T[] y, out T[] z)
        {
            throw new NotImplementedException();
        }
    }
}
