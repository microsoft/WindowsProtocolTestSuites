using System;
using System.Collections.Generic;

namespace CodecToolSet.Core
{
    public interface ICodecAction
    {
        /// <summary>
        /// The name of the codec transform, which serves 
        /// as the identity of the codec.
        /// </summary>
        /// <example>
        /// RemoteFX encode, DWT, Linearization, etc.
        /// </example>
        String Name { get; }

        /// <summary>
        /// The result from the last execution of <see cref="DoAction"/>.
        /// </summary>
        Tile[] Result { get; }

        /// <summary>
        /// The input for each step
        /// </summary>
        Tile[] Input { get; }

        /// <summary>
        /// Performs the codec transformation.
        /// </summary>
        /// <param name="input">
        /// The input tile.
        /// </param>
        /// <returns>
        /// The output tile.
        /// </returns>
        Tile DoAction(Tile input);

        /// <summary>
        /// Performs the codec transformation.
        /// </summary>
        /// <param name="inputs">
        /// The input tiles.
        /// </param>
        /// <returns>
        /// The output tiles.
        /// </returns>
        /// <remarks>
        /// This method takes in an resurns an array of Tiles,
        /// which is used by progressive codecs. 
        /// </remarks>
        Tile[] DoAction(Tile[] inputs);

        /// <summary>
        /// The sub transforms if any. 
        /// </summary>
        /// <example>
        /// Remote FX encode includes sub transforms like 
        /// DWT, Quantization, Linearization and RLGR encode.
        /// </example>
        IEnumerable<ICodecAction> SubActions { get; }

        /// <summary>
        /// All the parameters needed to perform the transformation.
        /// </summary>
        /// <remarks>
        /// It is able to take in any object. Derived classes
        /// should cast the object into the correct type.
        /// </remarks>
        Dictionary<String, ICodecParam> Parameters { get; }
    }
}
