using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace CodecToolSet.Core.RFXPEncode
{
    public class RFXPEncoderWrapper : RfxProgressiveEncoder
    {
        public static Triplet<short[,]> RGBToYUV(byte[,] RSet, byte[,] GSet, byte[,] BSet)
        {
            return RFXEncode.RFXEncoderWrapper.RGBToYUV(RSet, GSet, BSet);
        }

        public static void DWT(short[,] input, bool UseReduceExtrapolate, out short[,] output)
        {
            if (UseReduceExtrapolate)
            {
                output = new short[input.GetLength(0), input.GetLength(1)];
                Array.Copy(input, output, input.Length);
                DWT_Component(output);
            }
            else
            {
                RFXEncode.RFXEncoderWrapper.DWT(input, out output);
            }
        }

        public static void Quantization(short[,] input, QuantizationFactors quant, bool UseReduceExtrapolate, out short[,] output)
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
            doQuantization_Component(output, rfxQuant, UseReduceExtrapolate);
        }

        public static void Linearaztion_NoLL3Delta(short[,] input, bool UseReduceExtrapolate, out short[] output)
        {
            linearization_Compontent(input, UseReduceExtrapolate, out output);
        }

        // Should maintain a LastFrame
        public static void SubBandDiffing(Triplet<short[]> input, Triplet<short[]> lastFrame,bool UseReduceExtrapolate, out Triplet<short[]> output, out bool useDiffing)
        {
            // According to TD SubBandDiffing step is mandatory and in SubBandDiffing decide whether or not use diffing
            short[] x, y, z;
            output = null;
            if (lastFrame == null)
            {
                x = new short[input.X.Length];
                Array.Copy(input.X, x, input.X.Length);
                y = new short[input.Y.Length];
                Array.Copy(input.Y, y, input.Y.Length);
                z = new short[input.Z.Length];
                Array.Copy(input.Z, z, input.Z.Length);
                output = new Triplet<short[]>(x, y, z);
                useDiffing = false;
            }
            else
            {
                short[] yDiffDwt, cbDiffDwt, crDiffDwt;
                int lenOfNonLL3Band = (RdpegfxTileUtils.ComponentElementCount - RdpegfxTileUtils.GetBandSize(BandType_Values.LL3, UseReduceExtrapolate));// ? 81 : 64;
                int yNewZeroCount, cbNewZeroCount, crNewZeroCount;
                int yDiffZeroCount, cbDiffZeroCount, crDiffZeroCount;
                yDiffDwt = RdpegfxTileUtils.SubDiffingDwt(input.X, lastFrame.X, lenOfNonLL3Band, out yNewZeroCount, out yDiffZeroCount);
                cbDiffDwt = RdpegfxTileUtils.SubDiffingDwt(input.Y, lastFrame.Y, lenOfNonLL3Band, out cbNewZeroCount, out cbDiffZeroCount);
                crDiffDwt = RdpegfxTileUtils.SubDiffingDwt(input.Z, lastFrame.Z, lenOfNonLL3Band, out crNewZeroCount, out crDiffZeroCount);
                // According to TD we should only compare the zeros in Luma (Y) component
                // Need to be confirmed
                if ((yDiffDwt != null && cbDiffDwt != null && crDiffDwt != null) && yNewZeroCount < yDiffZeroCount)
                {
                    //use difference tile
                    output = new Triplet<short[]>(yDiffDwt, cbDiffDwt, crDiffDwt);
                    useDiffing = true;
                }
                else
                {
                    // don't use difference tile
                    x = new short[input.X.Length];
                    Array.Copy(input.X, x, input.X.Length);
                    y = new short[input.Y.Length];
                    Array.Copy(input.Y, y, input.Y.Length);
                    z = new short[input.Z.Length];
                    Array.Copy(input.Z, z, input.Z.Length);
                    output = new Triplet<short[]>(x, y, z);
                    useDiffing = false;
                }
            }
        }

        // TODO: make chunks variable
        public static void ProgressiveQuantization(Triplet<short[]> input, ProgressiveQuantizationFactors progQuants, bool UseReduceExtrapolate, out List<Triplet<short[]>> output)
        {
            output = new List<Triplet<short[]>>();
            DwtTile DRS = new DwtTile(input.X, input.Y, input.Z);
            foreach (var quantArray in progQuants.ProgQuants)
            {
                DwtBands yBD = DwtBands.GetFromLinearizationResult(DRS.Y_DwtQ, UseReduceExtrapolate);
                DwtBands cbBD = DwtBands.GetFromLinearizationResult(DRS.Cb_DwtQ, UseReduceExtrapolate);
                DwtBands crBD = DwtBands.GetFromLinearizationResult(DRS.Cr_DwtQ, UseReduceExtrapolate);

                RFX_PROGRESSIVE_CODEC_QUANT quant = Utility.ConvertProgQuant(quantArray);

                ProgressiveQuantization_Component(yBD, TileComponents.Y, quant);
                ProgressiveQuantization_Component(cbBD, TileComponents.Cb, quant);
                ProgressiveQuantization_Component(crBD, TileComponents.Cr, quant);

                DwtBands yDTS= DwtBands.GetFromLinearizationResult(DRS.Y_DwtQ, UseReduceExtrapolate);
                DwtBands cbDTS = DwtBands.GetFromLinearizationResult(DRS.Cb_DwtQ, UseReduceExtrapolate);
                DwtBands crDTS = DwtBands.GetFromLinearizationResult(DRS.Cr_DwtQ, UseReduceExtrapolate);

                DTS_Component(yDTS, TileComponents.Y, quant);
                DTS_Component(cbDTS, TileComponents.Cb, quant);
                DTS_Component(crDTS, TileComponents.Cr, quant);
                DwtTile dwtDts = new DwtTile(yDTS.GetLinearizationData(), cbDTS.GetLinearizationData(), crDTS.GetLinearizationData());
                DRS.Sub(dwtDts);

                var triplet = new Triplet<short[]>(yBD.GetLinearizationData(), cbBD.GetLinearizationData(), crBD.GetLinearizationData());
                output.Add(triplet);
            }
        }

        // The output format is
        // firstPass [X, Y, Z]
        // ProgressivePass1 [encodedX, encodedY, encodedZ]
        // ProgressivePass1 [rawX, rawY, rawZ]
        // ...
        public static void RLGR_SRLEncode(List<Triplet<short[]>> input, ProgressiveQuantizationFactors proQuants, EntropyAlgorithm mode, bool UseReduceExtrapolate, out List<Triplet<byte[]>> output)
        {
            output = new List<Triplet<byte[]>>();
            byte[] x, y, z;

            // fisrt pass
            Triplet<short[]> firstPass = input[0];
            ComputeLL3Deltas(firstPass, UseReduceExtrapolate);
            RFXEncode.RFXEncoderWrapper.RLGREncode(firstPass.X, mode, out x);
            RFXEncode.RFXEncoderWrapper.RLGREncode(firstPass.Y, mode, out y);
            RFXEncode.RFXEncoderWrapper.RLGREncode(firstPass.Z, mode, out z);
            output.Add(new Triplet<byte[]>(x, y, z));

            // fake encodingContext used for SRL encode
            var encodingContext = new RfxProgressiveCodecContext(new []{ RdpegfxTileUtils.GetCodecQuant(ImageQuality_Values.Midium) }, 0, 0, 0, UseReduceExtrapolate);
            encodingContext.DAS = new DwtTile(firstPass.X, firstPass.Y, firstPass.Z);

            var progQuantList = new List<RFX_PROGRESSIVE_CODEC_QUANT>();
            foreach (var quant in proQuants.ProgQuants)
            {
                progQuantList.Add(Utility.ConvertProgQuant(quant));
            }

            // progressive pass
            for (int i = 1; i < input.Count; i++)
            {
                Triplet<short[]> progressivePass = input[i];
                encodingContext.ProgQ = new DwtTile(progressivePass.X, progressivePass.Y, progressivePass.Z);
                encodingContext.prevProgQuant = progQuantList[i - 1];
                EncodedTile encodedTile = SRLEncode(encodingContext, progQuantList[i]);
                output.Add(new Triplet<byte[]>(encodedTile.YEncodedData, encodedTile.CbEncodedData, encodedTile.CrEncodedData));
                output.Add(new Triplet<byte[]>(encodedTile.YRawData, encodedTile.CbRawData, encodedTile.CrRawData));
                encodingContext.DAS.Add(new DwtTile(progressivePass.X, progressivePass.Y, progressivePass.Z));
                encodingContext.prevProgQuant = progQuantList[i];               
            }
        }

        private static void ComputeLL3Deltas(Triplet<short[]> input, bool UseReduceExtrapolate)
        {
            int ll3Len = RdpegfxTileUtils.GetBandSize(BandType_Values.LL3, UseReduceExtrapolate);
            int ll3Idx = RdpegfxTileUtils.ComponentElementCount - ll3Len;

            //for (int i = ll3Idx + 1; i < TileUtils.ComponentElementCount; i++)
            for (int i = RdpegfxTileUtils.ComponentElementCount - 1; i >= ll3Idx + 1; i--)
            {
                foreach (var component in input)
                {
                    component[i] = (short)(component[i] - component[i - 1]);
                }
            }
        }
    }
}
