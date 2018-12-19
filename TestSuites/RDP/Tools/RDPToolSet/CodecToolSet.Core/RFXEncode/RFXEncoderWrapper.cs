using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace CodecToolSet.Core.RFXEncode
{
    public class RFXEncoderWrapper : RemoteFXEncoder
    {
        public static Triplet<short[,]> RGBToYUV(byte[,] RSet, byte[,] GSet, byte[,] BSet)
        {
            var YSet = new short[TileSize, TileSize];
            var CbSet = new short[TileSize, TileSize];
            var CrSet = new short[TileSize, TileSize];
            for (int x = 0; x < TileSize; x++)
            {
                for (int y = 0; y < TileSize; y++)
                {
                    short[] yuv = RGBToYCbCr(RSet[x, y], GSet[x, y], BSet[x, y]);
                    YSet[x, y] = yuv[0];
                    CbSet[x, y] = yuv[1];
                    CrSet[x, y] = yuv[2];
                }
            }
            var result = new Triplet<short[,]>(YSet, CbSet, CrSet);
            return result;
        }

        public static void DWT(short[,] input, out short[,] output)
        {
            output = new short[input.GetLength(0), input.GetLength(1)];
            Array.Copy(input, output, input.Length);
            DWT_RomoteFX(output);
        }

        public static void Quantization(short[,] input, QuantizationFactors quant, out short[,] output)
        {
            var rfxQuant = new TS_RFX_CODEC_QUANT
            {
                LL3_LH3 = (byte)((quant.LH3 << 4) | quant.LL3),
                HL3_HH3 = (byte)((quant.HH3 << 4) | quant.HL3),
                LH2_HL2 = (byte)((quant.HL2 << 4) | quant.LH2),
                HH2_LH1 = (byte)((quant.LH1 << 4) | quant.HH2),
                HL1_HH1 = (byte)((quant.HH1 << 4) | quant.HL1)
            };
            output = new short[input.GetLength(0), input.GetLength(1)];
            Array.Copy(input, output, input.Length);
            doQuantization_Component(output, rfxQuant);
        }

        public static void Linearaztion(short[,] input, out short[] output)
        {
            linearization_Compontent(input, out output);
        }

        public static void RLGREncode(short[] input, EntropyAlgorithm mode, out byte[] output)
        {
            var rfxMode = (Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx.EntropyAlgorithm)mode.Algorithm;
            var RLGREncoder = new RLGREncoder();
            output = RLGREncoder.Encode(input, rfxMode);
        }
    }
}
