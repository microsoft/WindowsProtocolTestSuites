using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

// ReSharper disable once CheckNamespace
namespace CodecToolSet.Core.RFXEncode
{
    public sealed class RFXEncode : RFXEncodeBase
    {
        public override string Name
        {
            get { return Constants.ENCODE_NAME_RFXENCODE; }
        }

        public RFXEncode() : base()
        {
            this.SubActions = new List<ICodecAction>
            {
                new TileInput(),
                new RGBToYUV(),
                new DWT(),
                new Quantization(),
                new Linearization(),
                new RLGREncode()
            };

            Parameters[Constants.PARAM_NAME_QUANT_FACTORS] = RFXEncodeBase.DEFAULT_QUANT;
            Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = RFXEncodeBase.ENTROPY_ALG;
        }

        public override Tile DoAction(Tile input)
        {
            Tile output = null;
            this.Input = new[] { input };

            // perform all sub transforms
            foreach (var sub in SubActions) {

                // hand over parameters
                foreach (var key in Parameters.Keys) {
                    sub.Parameters[key] = Parameters[key];
                }

                output = sub.DoAction(input);
                input = output;
            }

            this.Result = new[] { output };
            return output;
        }
    }

    public sealed class TileInput : RFXEncodeBase
    {
        public TileInput()
            : base()
        {
        }

        public override string Name
        {
            get
            {
                return Constants.ENCODE_NAME_TILEINPUT;
            }
        }

        public override Tile DoAction(Tile input)
        {
            this.Result = new[] { input };
            return input;
        }
    }

    public sealed class RGBToYUV : RFXEncodeBase
    {
        public RGBToYUV()
            : base()
        {
        }

        public override string Name
        {
            get
            {
                return Constants.ENCODE_NAME_RGBTOYUV;
            }
        }

        public override Tile DoAction(Tile input)
        {
            this.Input = new[] { input };

            Triplet<byte[,]> triplet = input.GetMatrices<byte>();
            Triplet<short[,]> resultTriplet = RFXEncoderWrapper.RGBToYUV(triplet.X, triplet.Y, triplet.Z);
            Tile output = Tile.FromMatrices<short>(resultTriplet);
            
            this.Result = new[] { output };
            return output;
        }
    }

    public sealed class DWT : RFXEncodeBase
    {
        public DWT()
            : base()
        {
        }

        public override string Name
        {
            get
            {
                return Constants.ENCODE_NAME_DWT;
            }
        }

        public override Tile DoAction(Tile input)
        {
            short[,] x, y, z;

            this.Input = new[] { input };
            Triplet<short[,]> triplet = input.GetMatrices<short>();
            
            RFXEncoderWrapper.DWT(triplet.X, out x);
            RFXEncoderWrapper.DWT(triplet.Y, out y);
            RFXEncoderWrapper.DWT(triplet.Z, out z);
            
            var resultTriplet = new Triplet<short[,]>(x, y, z);
            Tile output = Tile.FromMatrices<short>(resultTriplet);

            this.Result = new[] { output };
            return output;
        }
    }

    public sealed class Quantization : RFXEncodeBase
    {
        public Quantization()
            : base()
        {
        }

        public override string Name
        {
            get { return Constants.ENCODE_NAME_QUANTIZATION; }
        }

        public sealed override Tile DoAction(Tile input)
        {
            short[,] x, y, z;

            this.Input = new[] { input };
            Triplet<short[,]> triplet = input.GetMatrices<short>();

            RFXEncoderWrapper.Quantization(triplet.X, QuantArray.quants[0], out x);
            RFXEncoderWrapper.Quantization(triplet.Y, QuantArray.quants[1], out y);
            RFXEncoderWrapper.Quantization(triplet.Z, QuantArray.quants[2], out z);

            var resultTriplet = new Triplet<short[,]>(x, y, z);
            Tile output = Tile.FromMatrices<short>(resultTriplet);

            this.Result = new[] { output };
            return output;
        }
    }

    public sealed class Linearization : RFXEncodeBase
    {
        public Linearization()
            : base()
        {
        }

        public override string Name
        {
            get { return Constants.ENCODE_NAME_LINEARIZATION; }
        }

        public override Tile DoAction(Tile input)
        {
            short[] x, y, z;

            this.Input = new[] { input };
            Triplet<short[,]> triplet = input.GetMatrices<short>();

            //// better use foreach, instead of repeating the code..
            RFXEncoderWrapper.Linearaztion(triplet.X, out x);
            RFXEncoderWrapper.Linearaztion(triplet.Y, out y);
            RFXEncoderWrapper.Linearaztion(triplet.Z, out z);

            var resultTriplet = new Triplet<short[]>(x, y, z);
            Tile output = Tile.FromArrays<short>(resultTriplet);

            this.Result = new[] { output };
            return output;
        }
    }

    public sealed class RLGREncode : RFXEncodeBase
    {
        public RLGREncode()
            : base()
        {
        }

        public override string Name
        {
            get
            {
                return Constants.ENCODE_NAME_RLGRENCODE;
            }
        }

        public override Tile DoAction(Tile input)
        {
            byte[] x, y, z;

            this.Input = new[] { input };
            Triplet<short[]> triplet = input.GetArrays<short>();

            //// better use foreach, instead of repeating the code..
            RFXEncoderWrapper.RLGREncode(triplet.X, this.Mode, out x);
            RFXEncoderWrapper.RLGREncode(triplet.Y, this.Mode, out y);
            RFXEncoderWrapper.RLGREncode(triplet.Z, this.Mode, out z);

            var resultTriplet = new Triplet<byte[]>(x, y, z);
            Tile output = Tile.FromArrays<byte>(resultTriplet);

            this.Result = new[] { output };
            return output;
        }
    }

}
