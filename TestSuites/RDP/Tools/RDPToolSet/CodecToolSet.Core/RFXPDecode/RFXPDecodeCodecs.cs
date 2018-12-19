using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodecToolSet.Core.RFXPDecode
{
    public sealed class RFXPDecode : RFXPDecodeBase
    {
        public override string Name
        {
            get { return Constants.PDECODE_NAME_RFXPDECODE; }
        }

        public RFXPDecode()
            : base()
        {
            this.SubActions = new List<ICodecAction>
            {
                new RLGR_SRLDecode(),
                new ProgressiveDequantization(),
                new SubBandDiffing(),
                new SubBandReconstruction(),
                new DeQuantization(),
                new InverseDWT(),
                new YUVToRGB(),
                new RGBToImage()
            };

            Parameters[Constants.PARAM_NAME_QUANT_FACTORS] = RFXPDecodeBase.DEFAULT_QUANT;
            Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = RFXPDecodeBase.ENTROPY_ALG;
            Parameters[Constants.PARAM_NAME_USE_DIFFERENCE_TILE] = RFXPDecodeBase.DEFAULT_USE_DIFFERENCE_TILE;
            Parameters[Constants.PARAM_NAME_USE_REDUCE_EXTRAPOLATE] = RFXPDecodeBase.DEFAULT_USE_REDUCE_EXTRAPOLATE;
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            Tile[] output = null;
            this.Input = inputs;

            // perform all sub transforms
            foreach (var sub in SubActions)
            {

                // hand over parameters
                foreach (var key in Parameters.Keys)
                {
                    sub.Parameters[key] = Parameters[key];
                }

                output = sub.DoAction(inputs);
                inputs = output;
            }

            this.Result = output;
            return output;
        }
    }

    public sealed class RLGR_SRLDecode : RFXPDecodeBase
    {
        public RLGR_SRLDecode()
            : base()
        {
        }

        public override string Name
        {
            get
            {
                return Constants.PDECODE_NAME_RLGRSRLDECODE;
            }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            Triplet<short[]> output;
            this.Input = inputs;

            EncodedTileType encodeType = (EncodedTileType)Parameters[Constants.PARAM_NAME_ENCODED_TILE_TYPE];
            if (encodeType == null || encodeType.Type == EncodedTileType.EncodedType.Simpe || encodeType.Type == EncodedTileType.EncodedType.FirstPass)
            {
                RFXPDecoderWrapper.RLGRDecode(inputs[0].GetArrays<byte>(), Mode, UseReduceExtrapolate.Enabled, out output);

            }
            else
            {
                Frame DAS = (Frame)Parameters[Constants.PARAM_NAME_DAS];
                QuantizationFactorsArray quants = (QuantizationFactorsArray)Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANTS];
                QuantizationFactorsArray preQuants = (QuantizationFactorsArray)Parameters[Constants.PARAM_NAME_PREVIOUS_PROGRESSIVE_QUANTS];
                RFXPDecoderWrapper.SRLDecode(inputs[0].GetArrays<byte>(), inputs[1].GetArrays<byte>(), quants, DAS.Tile.GetArrays<short>(), preQuants, UseReduceExtrapolate.Enabled, out output);
            }
            Tile tile = Tile.FromArrays<short>(output);
            this.Result = new[] { tile };
            return new[] { tile };
        }
    }

    public sealed class ProgressiveDequantization : RFXPDecodeBase
    {
        public override string Name
        {
            get
            {
                return Constants.PDECODE_NAME_PROGRESSIVEDEQUANTIZATION;
            }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            Triplet<short[]> output;
            this.Input = inputs;

            QuantizationFactorsArray quants = (QuantizationFactorsArray)Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANTS];

            RFXPDecoderWrapper.ProgressiveDequantization(inputs[0].GetArrays<short>(), quants, UseReduceExtrapolate.Enabled, out output);

            Tile tile = Tile.FromArrays<short>(output);
            this.Result = new[] { tile };
            return new[] { tile };
        }
    }

    public sealed class SubBandDiffing : RFXPDecodeBase
    {
        public override string Name
        {
            get
            {
                return Constants.PDECODE_NAME_SUBBANDDIFFING;
            }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            Triplet<short[]> output;
            this.Input = inputs;

            Frame lastFrame = (Frame)Parameters[Constants.PARAM_NAME_PREVIOUS_FRAME];
            EncodedTileType encodeType = (EncodedTileType)Parameters[Constants.PARAM_NAME_ENCODED_TILE_TYPE];

            Triplet<short[]> preTriplet = lastFrame == null ? null : lastFrame.Tile.GetArrays<short>();

            RFXPDecoderWrapper.SubBandDiffing(inputs[0].GetArrays<short>(), preTriplet, ParamUseDifferenceTile.Enabled, encodeType.Type, out output);

            Tile tile = Tile.FromArrays<short>(output);
            this.Result = new[] { tile };
            return new[] { tile };
        }
    }

    public sealed class SubBandReconstruction : RFXPDecodeBase
    {
        public override string Name
        {
            get
            {
                return Constants.PDECODE_NAME_SUBBANDRECONSTRUCTION;
            }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            Triplet<short[,]> output;
            this.Input = inputs;

            RFXPDecoderWrapper.SubBandReconstruction(inputs[0].GetArrays<short>(), UseReduceExtrapolate.Enabled, out output);

            Tile tile = Tile.FromMatrices<short>(output);
            this.Result = new[] { tile };
            return new[] { tile };
        }
    }

    public sealed class DeQuantization : RFXPDecodeBase
    {
        public override string Name
        {
            get
            {
                return Constants.PDECODE_NAME_DEQUANTIZATION;
            }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            Triplet<short[,]> output;
            this.Input = inputs;

            QuantizationFactorsArray quants = (QuantizationFactorsArray)Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY];
            RFXPDecoderWrapper.DeQuantization(inputs[0].GetMatrices<short>(), quants, UseReduceExtrapolate.Enabled, out output);

            Tile tile = Tile.FromMatrices<short>(output);
            this.Result = new[] { tile };
            return new[] { tile };
        }
    }

    public sealed class InverseDWT : RFXPDecodeBase
    {
        public override string Name
        {
            get
            {
                return Constants.PDECODE_NAME_INVERSEDWT;
            }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            Triplet<short[,]> output;
            this.Input = inputs;

            RFXPDecoderWrapper.InverseDWT(inputs[0].GetMatrices<short>(), UseReduceExtrapolate.Enabled, out output);

            Tile tile = Tile.FromMatrices<short>(output);
            this.Result = new[] { tile };
            return new[] { tile };
        }
    }

    public sealed class YUVToRGB : RFXPDecodeBase
    {
        public override string Name
        {
            get
            {
                return Constants.PDECODE_NAME_YUVTORGB;
            }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            Triplet<byte[,]> output;
            this.Input = inputs;

            RFXPDecoderWrapper.YUVToRGB(inputs[0].GetMatrices<short>(), out output);

            Tile tile = Tile.FromMatrices<byte>(output);
            this.Result = new[] { tile };
            return new[] { tile };
        }
    }

    public class RGBToImage : RFXPDecodeBase
    {
        public override string Name
        {
            get
            {
                return Constants.PDECODE_NAME_RECONSTRUCTEDFRAME;
            }
        }

        public RGBToImage()
            : base()
        {
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            this.Result = new[] { inputs[0] };
            return new[] { inputs[0] };
        }
    }
}
