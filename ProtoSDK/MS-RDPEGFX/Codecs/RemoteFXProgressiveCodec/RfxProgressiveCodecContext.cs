// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// The RemoteFX Progressive  Codec Context class
    /// </summary>
    public class RfxProgressiveCodecContext : RemoteFXCodecContext
    {
        /// <summary>
        /// Indicates if the Discrete Wavelet Transform (DWT) uses the "Reduce Extrapolate" method.
        /// </summary>
        public bool UseReduceExtrapolate;

        /// <summary>
        /// Indicates if use progressive techniques.
        /// </summary>
        public bool UseProgressive;

        /// <summary>
        /// Indicates if allow to send difference tile
        /// </summary>
        public bool UseDifferenceTile;

        //Data already sent
        public DwtTile DAS;

        //Data remaining to be sent
        public DwtTile DRS;

        //Data to be sent
        public DwtTile DTS;

        //Data to be sent after progressive quantantian
        public DwtTile ProgQ;

        //The tri-state
        public DwtTile TriSignState;

        //the last chunk for progressive encoding
        public RFX_PROGRESSIVE_CODEC_QUANT prevProgQuant;

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="tsRfxCodecQuantVals">Codec quantity values array</param>
        /// <param name="quantIdxY">Index of Y component in quantity array</param>
        /// <param name="quantIdxCb">Index of Cb component in quantity array</param>
        /// <param name="quantIdxCr">Index of Cr component in quantity array</param>
        /// <param name="bProgressive">indicates if use progressive codec</param>
        /// <param name="bTileDiff">indicates if sub-diffing</param>
        /// <param name="bReduceExtrapolate">Indicates if use Reduce-Extrapolate method in DWT</param>
        public RfxProgressiveCodecContext(
            TS_RFX_CODEC_QUANT[] tsRfxCodecQuantVals, 
            byte quantIdxY, 
            byte quantIdxCb, 
            byte quantIdxCr,
            bool bProgressive = false, 
            bool bTileDiff = true,
            bool bReduceExtrapolate = true)
            : base(tsRfxCodecQuantVals, quantIdxY, quantIdxCb, quantIdxCr, EntropyAlgorithm.CLW_ENTROPY_RLGR1)
        {
            UseProgressive = bProgressive;
            UseDifferenceTile = bTileDiff;
            UseReduceExtrapolate = bReduceExtrapolate;
            prevProgQuant = RdpegfxTileUtils.GetProgCodecQuant(ProgressiveChunk_Values.kChunk_None);
        }

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="tsRfxCodecQuantVals">Codec quantity values array</param>
        /// <param name="quantIdxY">Index of Y component in quantity array</param>
        /// <param name="quantIdxCb">Index of Cb component in quantity array</param>
        /// <param name="quantIdxCr">Index of Cr component in quantity array</param>
        /// <param name="useReduceExtrapolate">Indicates if used Reduce-Extrapolate method in DWT</param>
        public RfxProgressiveCodecContext(
            TS_RFX_CODEC_QUANT[] tsRfxCodecQuantVals,
            byte quantIdxY, 
            byte quantIdxCb, 
            byte quantIdxCr,
            bool useReduceExtrapolate = true)
            : base(tsRfxCodecQuantVals, quantIdxY, quantIdxCb, quantIdxCr, EntropyAlgorithm.CLW_ENTROPY_RLGR1)
        {
            UseReduceExtrapolate = useReduceExtrapolate;
        }
    }



}
