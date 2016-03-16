// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// RomoteFX context class
    /// </summary>
    public class RemoteFXCodecContext
    {
        /// <summary>
        /// RLGR entropy algorithm
        /// </summary>
        public EntropyAlgorithm Mode;

        #region RGB data
        /// <summary>
        /// Red component
        /// </summary>
        public byte[,] RSet;

        /// <summary>
        /// Green component
        /// </summary>
        public byte[,] GSet;

        /// <summary>
        /// Blue component
        /// </summary>
        public byte[,] BSet; 
        #endregion

        #region YUV data
        /// <summary>
        /// Y component
        /// </summary>
        public short[,] YSet;

        /// <summary>
        /// Cb component
        /// </summary>
        public short[,] CbSet;

        /// <summary>
        /// Cr component
        /// </summary>
        public short[,] CrSet; 
        #endregion

        #region Linearized YUV component
        /// <summary>
        /// Linearized Y component
        /// </summary>
        public short[] YComponent;

        /// <summary>
        /// Linearized U component
        /// </summary>
        public short[] CbComponent;

        /// <summary>
        /// Linearized V component
        /// </summary>
        public short[] CrComponent; 
        #endregion

        #region RLGR encoded YUV data
        /// <summary>
        /// RLGR encoded Y data
        /// </summary>
        public byte[] YData;

        /// <summary>
        /// RLGR encoded U data
        /// </summary>
        public byte[] CbData;

        /// <summary>
        /// RLGR encoded V data
        /// </summary>
        public byte[] CrData; 
        #endregion

        /// <summary>
        /// The scalar quantization values for the ten sub-bands in the 3-level DWT decomposition.
        /// </summary>
        public TS_RFX_CODEC_QUANT[] CodecQuantVals;

        public byte QuantIdxY;

        public byte QuantIdxCb;

        public byte QuantIdxCr;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tsRfxCodecQuant"></param>
        /// <param name="mode"></param>
        public RemoteFXCodecContext(TS_RFX_CODEC_QUANT tsRfxCodecQuant, EntropyAlgorithm mode)
        {
            this.CodecQuantVals = new TS_RFX_CODEC_QUANT[] { tsRfxCodecQuant };
            this.QuantIdxY = 0;
            this.QuantIdxCb = 0;
            this.QuantIdxCr = 0;
            this.Mode = mode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tsRfxCodecQuantVals"></param>
        /// <param name="quantIdxY"></param>
        /// <param name="quantIdxCb"></param>
        /// <param name="quantIdxCr"></param>
        /// <param name="mode"></param>
        public RemoteFXCodecContext(TS_RFX_CODEC_QUANT[] tsRfxCodecQuantVals, byte quantIdxY, byte quantIdxCb, byte quantIdxCr, EntropyAlgorithm mode)
        {
            if(tsRfxCodecQuantVals == null || tsRfxCodecQuantVals.Length == 0)
            {
                throw new ArgumentException("Parameter tsRfxCodecQuantVals cannot be null and its length must larger than 0.");
            }
            int maxIndex = tsRfxCodecQuantVals.Length -1;
            if (quantIdxY > maxIndex || quantIdxCb > maxIndex || quantIdxCr > maxIndex)
            {
                throw new ArgumentException("Quant index for Y , Cb or Cr is/are larger than the size of tsRfxCodecQuantVals.");
            }
            this.CodecQuantVals = tsRfxCodecQuantVals;
            this.QuantIdxY = quantIdxY;
            this.QuantIdxCb = quantIdxCb;
            this.QuantIdxCr = quantIdxCr;
            this.Mode = mode;
        }
    }

    /// <summary>
    /// DWT direction enumeration
    /// </summary>
    enum ArrayDirection
    {
        Vertical,
        Horizontal
    }
}
