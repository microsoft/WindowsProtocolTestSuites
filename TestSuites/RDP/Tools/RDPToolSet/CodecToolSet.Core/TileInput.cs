using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CodecDebugTool.Core
{
    // all sort of input goes here
    public partial class Tile
    {
        public static Tile FromString(string input)
        {
            throw new NotImplementedException();
        }

        public static Tile FromStrings(string[] input)
        {
            throw new NotImplementedException();
        }

        public static Tile FromStream(Stream input)
        {
            throw new NotImplementedException();
        }

        public static Tile FromBitmap(Bitmap input)
        {
            throw new NotImplementedException();
        }

        public static Tile FromTxtFile(string path)
        {
            throw new NotImplementedException();
        }

        public static Tile FromTriplet<T>(Triplet<T> input)
        {
            throw new NotImplementedException();
        }

        public static Tile FromArray<T>(IEnumerable<T> input)
        {
            throw new NotImplementedException();
        }

        public static Tile FromMatrices<T>(T[,] x, T[,] y, T[,] z)
        {
            throw new NotImplementedException();
        }

        public static Tile FromVectors<T>(T[] x, T[] y, T[] z)
        {
            throw new NotImplementedException();
        }
    }
}
