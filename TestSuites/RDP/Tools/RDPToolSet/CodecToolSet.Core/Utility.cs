using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace CodecToolSet.Core
{
    public class Utility
    {
        const string byteFormat = "{0:X2}";
        const string shortFormat = "{0,5}";
        const string byteSpaces = "  ";
        const string shortSpaces = "     ";
        const int TileSize = 64;

        /// <summary>
        /// Enum of possiable values for progressive chunk 
        /// </summary>
        public enum ProgressiveChunk : byte
        {
            kChunk_None = 0, // Nothing has been sent yet      kChunk_25,  
            kChunk_20 = 1,
            kChunk_25 = 2,
            kChunk_50 = 3,
            kChunk_75 = 4,
            kChunk_100 = 5
        }

        #region Generate output string buffer
        public static StringBuilder CreateStringBuffer<T>(T[,] dataSet)
        {
            string format;
            string spaces;

            SetFormat(typeof(T), out format, out spaces);

            var stringBuf = new StringBuilder();
            var row = dataSet.GetLength(0);
            var column = dataSet.GetLength(1);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    stringBuf.AppendFormat(format, dataSet[i, j]);
                    if (j != column - 1) stringBuf.Append(" ");
                }
                stringBuf.AppendLine();
            }
            return stringBuf;
        }

        public static StringBuilder CreateStringBuffer<T>(T[] dataSet)
        {
            string format;
            string spaces;

            // Fixed character number of a line.
            int cols = 12;
            if (dataSet.Length == TileSize * TileSize)
            {
                cols = TileSize;
            }

            SetFormat(typeof(T), out format, out spaces);

            var stringBuf = new StringBuilder();
            int count = 0;
            foreach (var pixel in dataSet)
            {
                stringBuf.AppendFormat(format, pixel);
                if (++count % cols == 0)
                {
                    stringBuf.AppendLine();
                }
                else
                {
                    stringBuf.Append(" ");
                }
            }
            if (count % cols != 0)
            {
                stringBuf.AppendLine();
            }
            return stringBuf;
        }
        #endregion

        #region Helper Method for Codec
        public static QuantizationFactorsArray GetProgQuantizationFactorsForChunk(ProgressiveChunk chunk)
        {
            RFX_PROGRESSIVE_CODEC_QUANT quant = RdpegfxTileUtils.GetProgCodecQuant((ProgressiveChunk_Values)chunk);
            QuantizationFactorsArray factors = ConvertProgQuant(quant);

            return factors;
        }

        public static TS_RFX_CODEC_QUANT ConvertRFXQuants(QuantizationFactors quant)
        {
            var rfxQuant = new TS_RFX_CODEC_QUANT
            {
                LL3_LH3 = (byte)((quant.LH3 << 4) | quant.LL3),
                HL3_HH3 = (byte)((quant.HH3 << 4) | quant.HL3),
                LH2_HL2 = (byte)((quant.HL2 << 4) | quant.LH2),
                HH2_LH1 = (byte)((quant.LH1 << 4) | quant.HH2),
                HL1_HH1 = (byte)((quant.HH1 << 4) | quant.HL1)
            };
            return rfxQuant;
        }

        public static RFX_PROGRESSIVE_CODEC_QUANT ConvertProgQuant(QuantizationFactorsArray quants)
        {
            var RFXQuants = new RFX_PROGRESSIVE_CODEC_QUANT();
            var quantList = new List<RFX_COMPONMENT_CODEC_QUANT>();
            foreach (var quant in quants.quants)
            {
                var rfxQuant = new RFX_COMPONMENT_CODEC_QUANT
                {
                    LL3_HL3 = (byte)((quant.HL3 << 4) | quant.LL3),
                    LH3_HH3 = (byte)((quant.HH3 << 4) | quant.LH3),
                    HL2_LH2 = (byte)((quant.LH2 << 4) | quant.HL2),
                    HH2_HL1 = (byte)((quant.HL1 << 4) | quant.HH2),
                    LH1_HH1 = (byte)((quant.HH1 << 4) | quant.LH1)
                };
                quantList.Add(rfxQuant);
            }
            RFXQuants.yQuantValues = quantList[0];
            RFXQuants.cbQuantValues = quantList[1];
            RFXQuants.crQuantValues = quantList[2];

            return RFXQuants;
        }

        public static QuantizationFactorsArray ConvertProgQuant(RFX_PROGRESSIVE_CODEC_QUANT quant)
        {
            var fators = new QuantizationFactorsArray();
            fators.quants = new QuantizationFactors[3];

            fators.quants[0] = new QuantizationFactors();
            fators.quants[0].LL3 = (byte)(quant.yQuantValues.LL3_HL3 & 0x0F);
            fators.quants[0].HL3 = (byte)(quant.yQuantValues.LL3_HL3 >> 4);
            fators.quants[0].LH3 = (byte)(quant.yQuantValues.LH3_HH3 & 0x0F);
            fators.quants[0].HH3 = (byte)(quant.yQuantValues.LH3_HH3 >> 4);
            fators.quants[0].HL2 = (byte)(quant.yQuantValues.HL2_LH2 & 0x0F);
            fators.quants[0].LH2 = (byte)(quant.yQuantValues.HL2_LH2 >> 4);
            fators.quants[0].HH2 = (byte)(quant.yQuantValues.HH2_HL1 & 0x0F);
            fators.quants[0].HL1 = (byte)(quant.yQuantValues.HH2_HL1 >> 4);
            fators.quants[0].LH1 = (byte)(quant.yQuantValues.LH1_HH1 & 0x0F);
            fators.quants[0].HH1 = (byte)(quant.yQuantValues.LH1_HH1 >> 4);

            fators.quants[1] = new QuantizationFactors();
            fators.quants[1].LL3 = (byte)(quant.cbQuantValues.LL3_HL3 & 0x0F);
            fators.quants[1].HL3 = (byte)(quant.cbQuantValues.LL3_HL3 >> 4);
            fators.quants[1].LH3 = (byte)(quant.cbQuantValues.LH3_HH3 & 0x0F);
            fators.quants[1].HH3 = (byte)(quant.cbQuantValues.LH3_HH3 >> 4);
            fators.quants[1].HL2 = (byte)(quant.cbQuantValues.HL2_LH2 & 0x0F);
            fators.quants[1].LH2 = (byte)(quant.cbQuantValues.HL2_LH2 >> 4);
            fators.quants[1].HH2 = (byte)(quant.cbQuantValues.HH2_HL1 & 0x0F);
            fators.quants[1].HL1 = (byte)(quant.cbQuantValues.HH2_HL1 >> 4);
            fators.quants[1].LH1 = (byte)(quant.cbQuantValues.LH1_HH1 & 0x0F);
            fators.quants[1].HH1 = (byte)(quant.cbQuantValues.LH1_HH1 >> 4);

            fators.quants[2] = new QuantizationFactors();
            fators.quants[2].LL3 = (byte)(quant.crQuantValues.LL3_HL3 & 0x0F);
            fators.quants[2].HL3 = (byte)(quant.crQuantValues.LL3_HL3 >> 4);
            fators.quants[2].LH3 = (byte)(quant.crQuantValues.LH3_HH3 & 0x0F);
            fators.quants[2].HH3 = (byte)(quant.crQuantValues.LH3_HH3 >> 4);
            fators.quants[2].HL2 = (byte)(quant.crQuantValues.HL2_LH2 & 0x0F);
            fators.quants[2].LH2 = (byte)(quant.crQuantValues.HL2_LH2 >> 4);
            fators.quants[2].HH2 = (byte)(quant.crQuantValues.HH2_HL1 & 0x0F);
            fators.quants[2].HL1 = (byte)(quant.crQuantValues.HH2_HL1 >> 4);
            fators.quants[2].LH1 = (byte)(quant.crQuantValues.LH1_HH1 & 0x0F);
            fators.quants[2].HH1 = (byte)(quant.crQuantValues.LH1_HH1 >> 4);

            return fators;
        }

        public static Frame GetPreviousFrame(string path, Dictionary<string, ICodecParam> parameters)
        {
            if (path == null) return null;

            Tile pretile = Tile.FromFile(path);

            string[] codecs = new[]{
                "rgbToYUV",
                "dwt",
                "quantization",
                "linearization",
            };

            var CodecDic = new Dictionary<string, CodecToolSet.Core.RFXPEncode.RFXPEncodeBase>()
            {
                {"rgbToYUV", new CodecToolSet.Core.RFXPEncode.RGBToYUV()},
                {"dwt", new CodecToolSet.Core.RFXPEncode.DWT()},
                {"quantization", new CodecToolSet.Core.RFXPEncode.Quantization()},
                {"linearization", new CodecToolSet.Core.RFXPEncode.Linearization()},
            };

            foreach (var key in CodecDic.Keys)
            {
                // TODO: use the same parameters with decode
                CodecDic[key].Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY];
                CodecDic[key].Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM];
                CodecDic[key].Parameters[Constants.PARAM_NAME_USE_REDUCE_EXTRAPOLATE] = parameters[Constants.PARAM_NAME_USE_REDUCE_EXTRAPOLATE];
            }

            Tile[] prergbToYUVOut = CodecDic["rgbToYUV"].DoAction(new[] { pretile });
            Tile[] predwtOut = CodecDic["dwt"].DoAction(prergbToYUVOut);
            Tile[] prequantOut = CodecDic["quantization"].DoAction(predwtOut);
            Tile[] prelinearOut = CodecDic["linearization"].DoAction(prequantOut);
            Frame preFrame = new Frame { Tile = prelinearOut.FirstOrDefault() };

            return preFrame;
        }
        #endregion

        private static void SetFormat(Type T, out string format, out string spaces)
        {
            format = "";
            if (T == typeof(byte))
                format = byteFormat;
            else if (T == typeof(short))
                format = shortFormat;

            spaces = "";
            if (T == typeof(byte))
                spaces = byteSpaces;
            else if (T == typeof(short))
                spaces = shortSpaces;
        }
    }
}
