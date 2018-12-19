using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;

namespace CodecToolSet.Core.RFXPEncode
{
    public sealed class RFXPEncode : RFXPEncodeBase
    {
        public override string Name
        {
            get { return Constants.PENCODE_NAME_RFXPENCODE; }
        }

        public RFXPEncode()
            : base()
        {
            this.SubActions = new List<ICodecAction>
            {
                new TileInput(),
                new RGBToYUV(),
                new DWT(),
                new Quantization(),
                new Linearization(),
                new SubBandDiffing(),
                new ProgressiveQuantization(),
                new RLGR_SRLEncode()
            };

            Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = RFXPEncodeBase.DEFAULT_QUANT_ARRAY;
            Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = RFXPEncodeBase.ENTROPY_ALG;
            Parameters[Constants.PARAM_NAME_USE_DIFFERENCE_TILE] = RFXPEncodeBase.DEFAULT_USE_DIFFERENCE_TILE;
            Parameters[Constants.PARAM_NAME_USE_REDUCE_EXTRAPOLATE] = RFXPEncodeBase.DEFAULT_USE_REDUCE_EXTRAPOLATE;
            Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANT_LIST] = RFXPEncodeBase.DEFAULT_PROG_QUANT;
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

    public sealed class TileInput : RFXPEncodeBase
    {
        public TileInput()
            : base()
        {
        }

        public override string Name
        {
            get
            {
                return Constants.PENCODE_NAME_TILEINPUT;
            }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            this.Input = inputs;
            this.Result = (Tile[])inputs.Clone();
            return inputs;
        }
    }

    public sealed class RGBToYUV : RFXPEncodeBase
    {
        public RGBToYUV()
            : base()
        {
        }

        public override string Name
        {
            get
            {
                return Constants.PENCODE_NAME_RGBTOYUV;
            }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            this.Input = inputs;

            Triplet<byte[,]> triplet = inputs.FirstOrDefault().GetMatrices<byte>();
            Triplet<short[,]> resultTriplet = RFXPEncoderWrapper.RGBToYUV(triplet.X, triplet.Y, triplet.Z);
            Tile output = Tile.FromMatrices<short>(resultTriplet);

            this.Result = new[] { output };
            return new[] { output };
        }
    }

    public sealed class DWT : RFXPEncodeBase
    {
        public DWT()
            : base()
        {
        }

        public override string Name
        {
            get
            {
                return Constants.PENCODE_NAME_DWT;
            }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            short[,] x, y, z;
            this.Input = inputs;

            Triplet<short[,]> triplet = inputs.FirstOrDefault().GetMatrices<short>();

            RFXPEncoderWrapper.DWT(triplet.X, ParamUseReduceExtrapolate.Enabled, out x);
            RFXPEncoderWrapper.DWT(triplet.Y, ParamUseReduceExtrapolate.Enabled, out y);
            RFXPEncoderWrapper.DWT(triplet.Z, ParamUseReduceExtrapolate.Enabled, out z);

            var resultTriplet = new Triplet<short[,]>(x, y, z);
            Tile output = Tile.FromMatrices<short>(resultTriplet);

            this.Result = new[] { output };
            return new[] { output };
        }
    }

    public sealed class Quantization : RFXPEncodeBase
    {
        public Quantization()
            : base()
        {
        }

        public override string Name
        {
            get { return Constants.PENCODE_NAME_QUANTIZATION; }
        }

        public sealed override Tile[] DoAction(Tile[] inputs)
        {
            short[,] x, y, z;
            this.Input = inputs;

            Triplet<short[,]> triplet = inputs.FirstOrDefault().GetMatrices<short>();

            RFXPEncoderWrapper.Quantization(triplet.X, QuantArray.quants[0], ParamUseReduceExtrapolate.Enabled, out x);
            RFXPEncoderWrapper.Quantization(triplet.Y, QuantArray.quants[1], ParamUseReduceExtrapolate.Enabled, out y);
            RFXPEncoderWrapper.Quantization(triplet.Z, QuantArray.quants[2], ParamUseReduceExtrapolate.Enabled, out z);

            var resultTriplet = new Triplet<short[,]>(x, y, z);
            Tile output = Tile.FromMatrices<short>(resultTriplet);

            this.Result = new[] { output };
            return new[] { output };
        }
    }

    public sealed class Linearization : RFXPEncodeBase
    {
        public Linearization()
            : base()
        {
        }

        public override string Name
        {
            get { return Constants.PENCODE_NAME_LINEARIZATION; }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            short[] x, y, z;
            this.Input = inputs;

            Triplet<short[,]> triplet = inputs.FirstOrDefault().GetMatrices<short>();

            //// better use foreach, instead of repeating the code..
            RFXPEncoderWrapper.Linearaztion_NoLL3Delta(triplet.X, ParamUseReduceExtrapolate.Enabled, out x);
            RFXPEncoderWrapper.Linearaztion_NoLL3Delta(triplet.Y, ParamUseReduceExtrapolate.Enabled, out y);
            RFXPEncoderWrapper.Linearaztion_NoLL3Delta(triplet.Z, ParamUseReduceExtrapolate.Enabled, out z);

            var resultTriplet = new Triplet<short[]>(x, y, z);
            Tile output = Tile.FromArrays<short>(resultTriplet);

            this.Result = new[] { output };
            return new[] { output };
        }
    }

    public sealed class SubBandDiffing : RFXPEncodeBase
    {
        public SubBandDiffing()
            : base() 
        {
        }

        public override string Name
        {
            get
            {
                return Constants.PENCODE_NAME_SUBBANDDIFFING;
            }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            this.Input = inputs;

            Triplet<short[]> preFrame = null;
            Triplet<short[]> triplet = null;
            bool useDiffing = false;
            ICodecParam param = Parameters.ContainsKey(Constants.PARAM_NAME_PREVIOUS_FRAME) ? Parameters[Constants.PARAM_NAME_PREVIOUS_FRAME] : null;
            if (param != null && param is Frame)
            {
                preFrame = ((Frame)param).Tile.GetArrays<short>();
            }

            RFXPEncoderWrapper.SubBandDiffing(inputs.FirstOrDefault().GetArrays<short>(), preFrame, ParamUseReduceExtrapolate.Enabled, out triplet, out useDiffing);
            ParamUseDifferenceTile.Enabled = useDiffing;
            Tile output = Tile.FromArrays<short>(triplet);

            this.Result = new[] { output };
            return new[] { output };
        }
    }


    public sealed class ProgressiveQuantization : RFXPEncodeBase
    {
        public ProgressiveQuantization()
            : base()
        {
        }

        public override string Name
        {
            get
            {
                return Constants.PENCODE_NAME_PROGRESSIVEQUANTIZATION;
            }
        }

        public override Tile[] DoAction(Tile[] inputs)
        {
            this.Input = inputs;

            List<Triplet<short[]>> triplets = null;
            Tile input = inputs.FirstOrDefault();
            RFXPEncoderWrapper.ProgressiveQuantization(input.GetArrays<short>(), ProgQuants, ParamUseReduceExtrapolate.Enabled, out triplets);

            List<Tile> tiles = new List<Tile>();
            foreach (var triplet in triplets)
            {
                tiles.Add(Tile.FromArrays<short>(triplet));
            }

            this.Result = tiles.ToArray();
            return tiles.ToArray();
        }
    }


    public sealed class RLGR_SRLEncode : RFXPEncodeBase
    {
        public RLGR_SRLEncode()
            : base()
        {
        }

        public override string Name
        {
            get
            {
                return Constants.PENCODE_NAME_RLGRSRLENCODE;
            }
        }

        // The output format is
        // firstPass [X, Y, Z]
        // ProgressivePass1 [encodedX, encodedY, encodedZ]
        // ProgressivePass1 [rawX, rawY, rawZ]
        // ...
        public override Tile[] DoAction(Tile[] inputs)
        {
            this.Input = inputs;

            var inputList = new List<Triplet<short[]>>();
            foreach (var input in inputs)
            {
                inputList.Add(input.GetArrays<short>());
            }
            List<Triplet<byte[]>> triplets = null;

            RFXPEncoderWrapper.RLGR_SRLEncode(inputList, ProgQuants, Mode, ParamUseReduceExtrapolate.Enabled, out triplets);

            List<Tile> tiles = new List<Tile>();
            foreach (var triplet in triplets)
            {
                tiles.Add(Tile.FromArrays<byte>(triplet));
            }

            this.Result = tiles.ToArray();
            return tiles.ToArray();
        }
    }
}
