using System;
using System.Collections.Generic;

namespace CodecToolSet.Core
{
    public interface ITileSerializer
    {
        /// <summary>
        /// Serializes a Tile object to a single string.
        /// </summary>
        /// <param name="tile">
        /// The Tile object to be serialized.
        /// </param>
        /// <returns>
        /// The string representation of the Tile object.
        /// </returns>
        String Serialize(Tile tile);

        /// <summary>
        /// Serializes a certain dimension in a Tile.
        /// </summary>
        /// <param name="tile">
        /// The Tile object.
        /// </param>
        /// <param name="d">
        /// The dimension to be serialized.
        /// </param>
        /// <returns>
        /// The serialized data of that dimension.
        /// </returns>
        String Serialize(Tile tile, TripletDimension d);

        /// <summary>
        /// Parses the given string into a Tile object.
        /// </summary>
        /// <param name="serializedTile">
        /// The serialized string representation of a Tile.
        /// </param>
        /// <returns>
        /// The Tile object represented by the given string.
        /// </returns>
        /// <exception cref="FormatException">
        /// If the given string cannot be parsed into a Tile
        /// object due to incorrect format, this exception 
        /// will be thrown.
        /// </exception>
        Tile Deserialize(String serializedTile);

        Tile Deserialize(Triplet<String> serializedTile);

        /// <summary>
        /// Parses the given string into a Tile object.
        /// </summary>
        /// <param name="input">
        /// The serialized string representation of a Tile. 
        /// </param>
        /// <param name="output">
        /// The Tile object represented by the given string.
        /// </param>
        /// <returns>
        /// True if succeed; false otherwise.
        /// </returns>
        /// <remarks>
        /// This method never throws any exception. If exceptions
        /// happened, it just returns false.
        /// </remarks>
        Boolean TryDeserialize(String input, out Tile output);

        Boolean TryDeserialize(Triplet<String> input, out Tile output);
    }

    public class SampleTileSerializer : ITileSerializer
    {
        public string Serialize(Tile tile)
        {
            throw new NotImplementedException();
        }

        public string Serialize(Tile tile, TripletDimension d)
        {
            return Utility.CreateStringBuffer(tile.GetArrays<short>()[d]).ToString();
        }

        public Tile Deserialize(string serializedTile)
        {
            throw new NotImplementedException();
        }

        public Tile Deserialize(Triplet<string> serializedTile)
        {
            // this is joking
            return Tile.RandomTile();
        }

        public bool TryDeserialize(string input, out Tile output)
        {
            throw new NotImplementedException();
        }

        public bool TryDeserialize(Triplet<string> input, out Tile output)
        {
            throw new NotImplementedException();
        }
    }

    public class HexTileSerializer : ITileSerializer
    {
        public string Serialize(Tile tile)
        {
            throw new NotImplementedException();
        }

        public string Serialize(Tile tile, TripletDimension d)
        {
            return Utility.CreateStringBuffer(tile.GetArrays<byte>()[d]).ToString();
        }

        public Tile Deserialize(string serializedTile)
        {
            throw new NotImplementedException();
        }

        public Tile Deserialize(Triplet<string> serializedTile)
        {
            var dataList = new List<byte[]>();

            foreach (var data in serializedTile)
            {
                string[] split = data.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var byteArray = new byte[split.Length];
                for (int i = 0; i < split.Length; i++)
                {
                    byteArray[i] = Convert.ToByte(split[i], 16);
                }
                dataList.Add(byteArray);
            }

            Triplet<byte[]> triplet = new Triplet<byte[]>(dataList[0], dataList[1], dataList[2]);
            Tile tile = Tile.FromArrays<byte>(triplet);
            return tile;
        }

        public bool TryDeserialize(string input, out Tile output)
        {
            throw new NotImplementedException();
        }

        public bool TryDeserialize(Triplet<string> input, out Tile output)
        {
            throw new NotImplementedException();
        }
    }

    public class IntegerTileSerializer : ITileSerializer
    {
        public string Serialize(Tile tile)
        {
            throw new NotImplementedException();
        }

        public string Serialize(Tile tile, TripletDimension d)
        {
            return Utility.CreateStringBuffer(tile.GetArrays<short>()[d]).ToString();
        }

        public Tile Deserialize(string serializedTile)
        {
            throw new NotImplementedException();
        }

        public Tile Deserialize(Triplet<string> serializedTile)
        {
            var dataList = new List<int[]>();

            foreach (var data in serializedTile)
            {
                string[] split = data.Split(new[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                var intArray = new int[split.Length];
                for (int i = 0; i < split.Length; i++)
                {
                    intArray[i] = Convert.ToInt16(split[i]);
                }
                dataList.Add(intArray);
            }

            Triplet<int[]> triplet = new Triplet<int[]>(dataList[0], dataList[1], dataList[2]);
            Tile tile = Tile.FromArrays<int>(triplet);
            return tile;
        }

        public bool TryDeserialize(string input, out Tile output)
        {
            throw new NotImplementedException();
        }

        public bool TryDeserialize(Triplet<string> input, out Tile output)
        {
            throw new NotImplementedException();
        }
    }

    public static class TileSerializerFactory
    {
        public static IEnumerable<ITileSerializer> GetAllTileSerializers()
        {
            return new List<ITileSerializer> {
                // add all serializers here
                new SampleTileSerializer(),
                new HexTileSerializer(),
                new IntegerTileSerializer()
            };
        }

        public static ITileSerializer GetDefaultSerizlizer()
        {
            return new IntegerTileSerializer();
        }
    }
}
