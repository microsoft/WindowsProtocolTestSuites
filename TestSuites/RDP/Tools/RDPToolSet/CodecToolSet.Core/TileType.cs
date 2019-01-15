using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace CodecToolSet.Core
{
    public sealed class Tile
    {
        public const int TileSize = 64;

        #region Fields

        // it is a triplet of Int16 arrays inside, but the triplet will not be exposed.
        // the users do not care what's inside, they operate on the tile through 
        // input/output methods.
        private Triplet<int[]> _triplet;

        #endregion

        #region Constructor

        // privatize the constructor. 
        // user can only get a tile object from input methods.
        private Tile()
        {
            _triplet = new Triplet<int[]>();
        }

        #endregion
      
        #region Input Methods

        public static Tile FromFile(String path)
        {
            Image image = Bitmap.FromFile(path);
            return Tile.FromBitmap(image, 0, 0);
        }

        public static Tile FromBitmap(Image image, int leftOffset, int topOffset)
        {

            var bitmap = new Bitmap(image);
            var RSet = new byte[TileSize, TileSize];
            var GSet = new byte[TileSize, TileSize];
            var BSet = new byte[TileSize, TileSize];

            int right = Math.Min(image.Width - 1, leftOffset + TileSize - 1);
            int bottom = Math.Min(image.Height - 1, topOffset + TileSize - 1);

            BitmapData bmpData = bitmap.LockBits(new Rectangle(leftOffset, topOffset, right - leftOffset + 1, bottom - topOffset + 1), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* cusor = (byte*)bmpData.Scan0.ToPointer();
                for (int y = topOffset; y <= bottom; y++)
                {
                    for (int x = leftOffset; x <= right; x++)
                    {
                        int tileX = x - leftOffset;
                        int tileY = y - topOffset;
                        BSet[tileX, tileY] = cusor[0];
                        GSet[tileX, tileY] = cusor[1];
                        RSet[tileX, tileY] = cusor[2];
                        cusor += 3;
                    }
                    cusor += (bmpData.Stride - 3 * (bmpData.Width));
                }
            }
            bitmap.UnlockBits(bmpData);

            var triplet = new Triplet<byte[,]>(RSet, GSet, BSet);
            Tile tile = Tile.FromMatrices<byte>(triplet);
            return tile;
        }

        public static Tile RandomTile()
        {
            var buffer = new Triplet<byte[]>(
                x: new byte[0x1000],
                y: new byte[0x1000],
                z: new byte[0x1000]);
            var random = new Random(Guid.NewGuid().GetHashCode());
            buffer.ForEach(buf => random.NextBytes(buf));
            return Tile.FromArrays(buffer);
        }

        // automatically picks up the correct serializer 
        public static Tile FromString(String input)
        {
            Tile tile;

            // check each serializer existed
            foreach (var serializer in TileSerializerFactory.GetAllTileSerializers()) {
                // if any serializer is able to deserialize the input,
                // just use it. 
                if (serializer.TryDeserialize(input, out tile))
                    return tile;
            }

            // no serializer is able to parse the input
            throw new FormatException();
        }

        public static Tile FromString(String input, ITileSerializer serializer)
        {
            return serializer.Deserialize(input);
        }

        public static Tile FromStrings(Triplet<String> input, ITileSerializer serializer)
        {
            return serializer.Deserialize(input);
        }

        public static Tile FromArrays<T>(Triplet<T[]> input) 
            where T : struct, IConvertible
        {
            return new Tile {
                _triplet = new Triplet<int[]> {
                    // Convert should work since T is IConvertible
                    // SRL encoded raw data may be a NULL
                    X = input.X == null ? null : input.X.Select(i => Convert.ToInt32(i)).ToArray(),
                    Y = input.Y == null ? null : input.Y.Select(i => Convert.ToInt32(i)).ToArray(),
                    Z = input.Z == null ? null : input.Z.Select(i => Convert.ToInt32(i)).ToArray()
                }
            };
        }

        public static Tile FromMatrices<T>(Triplet<T[,]> input) 
            where T: struct, IConvertible
        {
            return new Tile
            {
                _triplet = new Triplet<int[]> {
                    X = input.X == null ? null : input.X.Flatten().Select(i => Convert.ToInt32(i)).ToArray(),
                    Y = input.Y == null ? null : input.Y.Flatten().Select(i => Convert.ToInt32(i)).ToArray(),
                    Z = input.Z == null ? null : input.Z.Flatten().Select(i => Convert.ToInt32(i)).ToArray(),
                }
            };
        }

        #endregion

        #region Output Methods

        public void SaveToFile(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            foreach (var dimension in _triplet)
            {
                if (dimension == null)
                {
                    streamWriter.WriteLine("NULL");
                }
                else
                {
                    foreach (var element in dimension)
                    {
                        streamWriter.Write(element);
                        streamWriter.Write("\t");
                    }
                    streamWriter.WriteLine();
                }
            }

            streamWriter.Close();
            fileStream.Close();
        }

        public Bitmap GetBitmap()
        {
            Bitmap tileImg = new Bitmap(TileSize, TileSize);

            BitmapData bmpData = tileImg.LockBits(new Rectangle(0, 0, tileImg.Width, tileImg.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            unsafe
            {
                Triplet<byte[,]> triplet = GetMatrices<byte>();
                byte* cusor = (byte*)bmpData.Scan0.ToPointer();
                for (int y = 0; y < bmpData.Height; y++)
                {
                    for (int x = 0; x < bmpData.Width; x++)
                    {
                        cusor[0] = triplet.Z[x, y];
                        cusor[1] = triplet.Y[x, y];
                        cusor[2] = triplet.X[x, y];
                        cusor += 3;
                    }
                    cusor += (bmpData.Stride - 3 * (bmpData.Width));
                }
            }
            tileImg.UnlockBits(bmpData);

            return tileImg;
        }

        public String GetString(ITileSerializer serializer)
        {
            return serializer.Serialize(this);
        }

        public Triplet<String> GetStrings(ITileSerializer serializer)
        {
            return new Triplet<String> {
                // TODO: return "No Data" instead?
                X = _triplet.X == null || _triplet.X.Length == 0 ? "" : serializer.Serialize(this, TripletDimension.X),
                Y = _triplet.Y == null || _triplet.Y.Length == 0 ? "" : serializer.Serialize(this, TripletDimension.Y),
                Z = _triplet.Z == null || _triplet.Z.Length == 0 ? "" : serializer.Serialize(this, TripletDimension.Z),
            };
        }

        public Triplet<T[]> GetArrays<T>() where T : struct
        {
            return new Triplet<T[]> {
                X = _triplet.X == null ? null : _triplet.X.Select(i => (T)Convert.ChangeType(i, typeof(T))).ToArray(),
                Y = _triplet.Y == null ? null : _triplet.Y.Select(i => (T)Convert.ChangeType(i, typeof(T))).ToArray(),
                Z = _triplet.Z == null ? null : _triplet.Z.Select(i => (T)Convert.ChangeType(i, typeof(T))).ToArray()
            };
        }

        public Triplet<T[,]> GetMatrices<T>() where T : struct
        {
            var array = this.GetArrays<T>();

            return new Triplet<T[,]> {
                X = array.X == null ? null : array.X.ToMatrix(TileSize),
                Y = array.Y == null ? null : array.Y.ToMatrix(TileSize),
                Z = array.Z == null ? null : array.Z.ToMatrix(TileSize)
            };
        }

        #endregion

        #region Basic Operation
        public bool Add(Tile addition)
        {
            Triplet<int[]> triplet = addition.GetArrays<int>();
            if (triplet.X != null && triplet.Y != null && triplet.Z != null
                && _triplet.X != null && _triplet.Y != null && _triplet.Z != null
                && triplet.X.Length == _triplet.X.Length && triplet.Y.Length == _triplet.Y.Length && triplet.Z.Length == _triplet.Z.Length)
            {
                for (int i = 0; i < _triplet.X.Length; i++)
                {
                    _triplet.X[i] += triplet.X[i];
                }
                for (int i = 0; i < _triplet.Y.Length; i++)
                {
                    _triplet.Y[i] += triplet.Y[i];
                }
                for (int i = 0; i < _triplet.Z.Length; i++)
                {
                    _triplet.Z[i] += triplet.Z[i];
                }
            }
            return false;
        }

        public bool Sub(Tile subtracter)
        {
            return false;
        }

        public void LeftShift(int count)
        {
            for (int i = 0; i < _triplet.X.Length; i++)
            {
                _triplet.X[i] <<= count;
            }
            for (int i = 0; i < _triplet.Y.Length; i++)
            {
                _triplet.Y[i] <<= count;
            }
            for (int i = 0; i < _triplet.Z.Length; i++)
            {
                _triplet.Z[i] <<= count;
            }
        }

        public void RightShift(int count)
        {
            for (int i = 0; i < _triplet.X.Length; i++)
            {
                _triplet.X[i] >>= count;
            }
            for (int i = 0; i < _triplet.Y.Length; i++)
            {
                _triplet.Y[i] >>= count;
            }
            for (int i = 0; i < _triplet.Z.Length; i++)
            {
                _triplet.Z[i] >>= count;
            }
        }

        #endregion
    }

}
