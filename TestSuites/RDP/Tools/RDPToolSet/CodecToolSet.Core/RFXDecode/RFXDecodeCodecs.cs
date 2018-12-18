using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace CodecToolSet.Core.RFXDecode
{
    public class RFXDecode : RFXDecodeBase
    {
        public override string Name
        {
            get { return Constants.DECODE_NAME_RFXDECODE; }
        }

        public RFXDecode() : base()
        {
            this.SubActions = new List<ICodecAction>
            {
                new RLGRDecode(),
                new SubBandReconstruction(),
                new Dequantization(),
                new InverseDWT(),
                new YUVToRGB(),
                new RGBToImage()
            };

            Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = RFXDecodeBase.DEFAULT_QUANT_ARRAY;
            Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = RFXDecodeBase.ENTROPY_ALG;
        }

        public override Tile DoAction(Tile input)
        {
            Tile output = null;
            this.Input = new[] { input };

            // perform all sub transforms
            foreach (var sub in SubActions)
            {

                // hand over parameters
                foreach (var key in Parameters.Keys)
                {
                    sub.Parameters[key] = Parameters[key];
                }

                output = sub.DoAction(input);
                input = output;
            }

            this.Result = new[] { output };
            return output;
        }
    }

    public class RLGRDecode : RFXDecodeBase
    {
        public override string Name
        {
            get
            {
                return Constants.DECODE_NAME_RLGRDECODE;
            }
        }

        public RLGRDecode()
            : base()
        {
        }

        public override Tile DoAction(Tile input)
        {
            short[] x, y, z;
            this.Input = new[] { input };

            Triplet<byte[]> triplet = input.GetArrays<byte>();

            //// better use foreach, instead of repeating the code..
            RFXDecoderWrapper.RLGRDecode(triplet.X, this.Mode, out x);
            RFXDecoderWrapper.RLGRDecode(triplet.Y, this.Mode, out y);
            RFXDecoderWrapper.RLGRDecode(triplet.Z, this.Mode, out z);

            var resultTriplet = new Triplet<short[]>(x, y, z);
            Tile output = Tile.FromArrays<short>(resultTriplet);

            this.Result = new[] { output };
            return output;
        }
    }

    public class SubBandReconstruction : RFXDecodeBase
    {
        public override string Name
        {
            get { return Constants.DECODE_NAME_SUBBANDRECONSTRUCTION; }
        }

        public SubBandReconstruction()
            : base()
        {
        }

        public override Tile DoAction(Tile input)
        {
            short[,] x, y, z;
            this.Input = new[] { input };

            Triplet<short[]> triplet = input.GetArrays<short>();

            //// better use foreach, instead of repeating the code..
            RFXDecoderWrapper.SubBandReconstruction(triplet.X, out x);
            RFXDecoderWrapper.SubBandReconstruction(triplet.Y, out y);
            RFXDecoderWrapper.SubBandReconstruction(triplet.Z, out z);

            var resultTriplet = new Triplet<short[,]>(x, y, z);
            Tile output = Tile.FromMatrices<short>(resultTriplet);

            this.Result = new[] { output };
            return output;
        }
    }

    public class Dequantization : RFXDecodeBase
    {
        public override string Name
        {
            get { return Constants.DECODE_NAME_DEQUANTIZATION; }
        }

        public Dequantization()
            : base()
        {
        }

        public override Tile DoAction(Tile input)
        {
            short[,] x, y, z;
            this.Input = new[] { input };

            Triplet<short[,]> triplet = input.GetMatrices<short>();

            RFXDecoderWrapper.Dequantization(triplet.X, QuantArray.quants[0], out x);
            RFXDecoderWrapper.Dequantization(triplet.Y, QuantArray.quants[1], out y);
            RFXDecoderWrapper.Dequantization(triplet.Z, QuantArray.quants[2], out z);

            var resultTriplet = new Triplet<short[,]>(x, y, z);
            Tile output = Tile.FromMatrices<short>(resultTriplet);

            this.Result = new[] { output };
            return output;
        }
    }

    public class InverseDWT : RFXDecodeBase
    {
        public override string Name
        {
            get
            {
                return Constants.DECODE_NAME_INVERSEDWT;
            }
        }

        public InverseDWT()
            : base()
        {
        }

        public override Tile DoAction(Tile input)
        {
            short[,] x, y, z;
            this.Input = new[] { input };

            Triplet<short[,]> triplet = input.GetMatrices<short>();

            RFXDecoderWrapper.InverseDWT(triplet.X, out x);
            RFXDecoderWrapper.InverseDWT(triplet.Y, out y);
            RFXDecoderWrapper.InverseDWT(triplet.Z, out z);

            var resultTriplet = new Triplet<short[,]>(x, y, z);
            Tile output = Tile.FromMatrices<short>(resultTriplet);

            this.Result = new[] { output };
            return output;
        }
    }

    public class YUVToRGB : RFXDecodeBase
    {
        public override string Name
        {
            get
            {
                return Constants.DECODE_NAME_YUVTORGB;
            }
        }

        public YUVToRGB()
            : base()
        {
        }

        public override Tile DoAction(Tile input)
        {
            byte[,] x, y, z;
            this.Input = new[] { input };

            Triplet<short[,]> triplet = input.GetMatrices<short>();

            RFXDecoderWrapper.YUVToRGB(triplet.X, triplet.Y, triplet.Z, out x, out y, out z);

            var resultTriplet = new Triplet<byte[,]>(x, y, z);
            Tile output = Tile.FromMatrices<byte>(resultTriplet);

            this.Result = new[] { output };
            return output;
        }
    }

    public class RGBToImage : RFXDecodeBase
    {
        public override string Name
        {
            get
            {
                return Constants.DECODE_NAME_RECONSTRUCTEDFRAME;
            }
        }

        public RGBToImage()
            : base()
        {
        }

        public override Tile DoAction(Tile input)
        {
            this.Result = new[] { input };
            return input;
        }
    }

}
