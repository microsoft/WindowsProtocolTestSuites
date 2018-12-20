using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodecToolSet.Core.RFXDecode;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace CodecToolSet.Core.RFXPDecode
{
    public class RFXPDecoderWrapper : RfxProgressiveDecoder
    {
        public static void RLGRDecode(Triplet<byte[]> input, EntropyAlgorithm mode, bool useReduceExtrapolate, out Triplet<short[]> output)
        {
            var outlist = new List<short[]>();
            foreach (var component in input)
            {
                short[] decodedComponent;
                RFXDecoderWrapper.RLGRDecode(component, mode, out decodedComponent);
                ComputeOriginalLL3FromDeltas(decodedComponent, useReduceExtrapolate);
                outlist.Add(decodedComponent);
            }
            output = new Triplet<short[]>(outlist[0], outlist[1], outlist[2]);
        }

        public static void SRLDecode(
            Triplet<byte[]> decodedData,
            Triplet<byte[]> rawData,
            QuantizationFactorsArray quant,
            Triplet<short[]> DAS,
            QuantizationFactorsArray preQuant,
            bool useReduceExtrapolate,
            out Triplet<short[]> output)
        {
            RFX_PROGRESSIVE_CODEC_QUANT progCodecQuant = Utility.ConvertProgQuant(quant);
            RFX_PROGRESSIVE_CODEC_QUANT preCodecQuant = Utility.ConvertProgQuant(preQuant);
            // construct the fake parameter to invoke RfxProgressiveDecoder.SRLDecode 
            var codecContext = new RfxProgressiveCodecContext(new[] { RdpegfxTileUtils.GetCodecQuant(ImageQuality_Values.Midium) }, 0, 0, 0, useReduceExtrapolate);
            var encodeTile = new EncodedTile
            {
                YEncodedData = decodedData.X,
                CbEncodedData = decodedData.Y,
                CrEncodedData = decodedData.Z,
                YRawData = rawData.X,
                CbRawData = rawData.Y,
                CrRawData = rawData.Z,
                ProgCodecQuant = progCodecQuant,
                UseReduceExtrapolate = useReduceExtrapolate // Important
            };
            var sentTile = new DwtTile(DAS.X, DAS.Y, DAS.Z);
            sentTile.ProgCodecQuant = preCodecQuant;
            var frame = new SurfaceFrame(0, RdpegfxTileUtils.TileSize, RdpegfxTileUtils.TileSize);
            frame.UpdateTileDwtQ(new TileIndex(0, 0, RdpegfxTileUtils.TileSize, RdpegfxTileUtils.TileSize), sentTile);
            frame.UpdateTriState(new TileIndex(0, 0, RdpegfxTileUtils.TileSize, RdpegfxTileUtils.TileSize), sentTile);
            var tileState = new TileState(frame, new TileIndex(0, 0, RdpegfxTileUtils.TileSize, RdpegfxTileUtils.TileSize));

            SRLDecode(codecContext, encodeTile, tileState);
            output = new Triplet<short[]>(codecContext.YComponent, codecContext.CbComponent, codecContext.CrComponent);
        }

        public static void ProgressiveDequantization(Triplet<short[]> input, QuantizationFactorsArray quant, bool useReduceExtrapolate, out Triplet<short[]> output)
        {
            output = new Triplet<short[]>();
            output.X = (short[])input.X.Clone();
            output.Y = (short[])input.Y.Clone();
            output.Z = (short[])input.Z.Clone();

            RFX_PROGRESSIVE_CODEC_QUANT codecQuant = Utility.ConvertProgQuant(quant);

            ProgressiveDeQuantization_Component(output.X, codecQuant.yQuantValues, useReduceExtrapolate);
            ProgressiveDeQuantization_Component(output.Y, codecQuant.cbQuantValues, useReduceExtrapolate);
            ProgressiveDeQuantization_Component(output.Z, codecQuant.crQuantValues, useReduceExtrapolate);
        }

        public static void SubBandDiffing(Triplet<short[]> input, Triplet<short[]> lastFrame, bool useDifferenceTile, CodecToolSet.Core.EncodedTileType.EncodedType encodedType, out Triplet<short[]> output)
        {
            // Important! Actually only the first pass need SubBandDiffing, but here we merge the UpgradePass and FirstPass together
            // and make the interface unified this function has been reserved
            // Details refer to TD for SubBandDiffing description
            output = new Triplet<short[]>();
            output.X = (short[])input.X.Clone();
            output.Y = (short[])input.Y.Clone();
            output.Z = (short[])input.Z.Clone();
            // lastFrame is the current encoded tile.
            if (lastFrame == null) return;
            // If encodetile is not UpgradePass but if subband diffing is used we also need to add the decode tile to the current tile
            if (useDifferenceTile || encodedType == EncodedTileType.EncodedType.UpgradePass)
            {
                ArrayAdd(output.X, lastFrame.X);
                ArrayAdd(output.Y, lastFrame.Y);
                ArrayAdd(output.Z, lastFrame.Z);
            }
        }

        public static void SubBandReconstruction(Triplet<short[]> input, bool useReduceExtrapolate, out Triplet<short[,]> output)
        {
            short[,] x, y, z;

            reconstruction_Component(input.X, out x, useReduceExtrapolate);
            reconstruction_Component(input.Y, out y, useReduceExtrapolate);
            reconstruction_Component(input.Z, out z, useReduceExtrapolate);

            output = new Triplet<short[,]>(x, y, z);
        }

        public static void DeQuantization(Triplet<short[,]> input, QuantizationFactorsArray quants, bool useReduceExtrapolate, out Triplet<short[,]> output)
        {
            output = new Triplet<short[,]>();
            output.X = (short[,])input.X.Clone();
            output.Y = (short[,])input.Y.Clone();
            output.Z = (short[,])input.Z.Clone();

            dequantization_Component(output.X, Utility.ConvertRFXQuants(quants.quants[0]), useReduceExtrapolate);
            dequantization_Component(output.Y, Utility.ConvertRFXQuants(quants.quants[1]), useReduceExtrapolate);
            dequantization_Component(output.Z, Utility.ConvertRFXQuants(quants.quants[2]), useReduceExtrapolate);
        }

        public static void InverseDWT(Triplet<short[,]> input, bool useReduceExtrapolate, out Triplet<short[,]> output)
        {
            output = new Triplet<short[,]>();
            output.X = (short[,])input.X.Clone();
            output.Y = (short[,])input.Y.Clone();
            output.Z = (short[,])input.Z.Clone();
            if (useReduceExtrapolate)
            {
                InverseDWT_Component(output.X);
                InverseDWT_Component(output.Y);
                InverseDWT_Component(output.Z);
            }
            else
            {
                short[,] x, y, z;
                RFXDecoderWrapper.InverseDWT(output.X, out x);
                RFXDecoderWrapper.InverseDWT(output.Y, out y);
                RFXDecoderWrapper.InverseDWT(output.Z, out z);
                output = new Triplet<short[,]>(x, y, z);
            }
        }

        public static void YUVToRGB(Triplet<short[,]> input, out Triplet<byte[,]> output)
        {
            byte[,] x, y, z;
            RFXDecoderWrapper.YUVToRGB(input.X, input.Y, input.Z, out x, out y, out z);
            output = new Triplet<byte[,]>(x, y, z);
        }

        private static void ComputeOriginalLL3FromDeltas(short[] input, bool useReduceExtrapolate)
        {
            int ll3Len = RdpegfxTileUtils.GetBandSize(BandType_Values.LL3, useReduceExtrapolate);
            int ll3Idx = RdpegfxTileUtils.ComponentElementCount - ll3Len;

            for (int i = ll3Idx + 1; i < RdpegfxTileUtils.ComponentElementCount; i++)
            {
                input[i] = (short)(input[i] + input[i - 1]);
            }
        }

        private static void MatrixAdd(short[,] x, short[,] y)
        {
            for (int i = 0; i < x.GetLength(0); i++)
            {
                for (int j = 0; j < x.GetLength(1); j++)
                {
                    x[i, j] += y[i, j];
                }
            }
        }

        private static void ArrayAdd(short[] x, short[] y)
        {
            for (int i = 0; i < x.GetLength(0); i++)
            {
                x[i] += y[i];
            }
        }
    }
}
