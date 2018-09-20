using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodecDebugTool.Core
{
    //
    //
    // THE FOLLOWING CODE ARE JUST FOR TEST AND DEMO PURPOSE
    // REPLACE THEM WITH THE REAL CODE IN THE FUTURE
    //
    //
    public abstract class MockCodecBase : ICodecAction
    {
        public virtual string Name { get { return "Mock Codec Action"; } }

        public Tile[] Result { get; protected set; }

        public IEnumerable<ICodecAction> SubActions { get; protected set; }

        public ICollection<ICodecParam> Parameters { get; protected set; }

        protected MockCodecBase()
        {
            this.SubActions = null;
            this.Parameters = new Collection<ICodecParam>();
        }

        public virtual Tile DoAction(Tile input)
        {
            // just return a random tile as result
            Result = new[] { Tile.RandomTile() };
            return Result.FirstOrDefault();
        }

        public Tile[] DoAction(Tile[] inputs)
        {
            return new[] {this.DoAction(inputs.FirstOrDefault())};
        }
    }

    public sealed class MockRFXDecode : MockCodecBase
    {
        public override string Name
        {
            get { return "RFX Decode"; }
        }

        public MockRFXDecode() : base()
        {
            // register all sub-actions
            this.SubActions = new List<ICodecAction> {
                new EntropyDecoding(),
                new SubBandReconstruction(),
                new Dequantization(),
                new InverseDWT(),
                new YCbCrToRGB(),
                new ReconstructedFrame()
            };
        }

        public override Tile DoAction(Tile input)
        {
            Tile tile = input;

            foreach (var action in this.SubActions) {
                // handover parameters
                foreach (var param in this.Parameters) 
                    action.Parameters.Add(param);
                // perform each action
                tile = action.DoAction(tile);
            }
            
            // record the result
            this.Result = new[] {tile};

            return tile;
        }
    }

    public sealed class MockRFXEncode : MockCodecBase
    {
        public override string Name
        {
            get { return "RFX Encode"; }
        }

        public MockRFXEncode() : base()
        {
            // register all sub-actions
            this.SubActions = new List<ICodecAction> {
                new RGBToYCbCr(),
                new DWT(),
                new Quantization(),
                new Linearization(),
                new EntropyEncoding(),
            };
        }

        public override Tile DoAction(Tile input)
        {
            Tile tile = input;

            foreach (var action in this.SubActions) {
                // handover parameters
                foreach (var param in this.Parameters) 
                    action.Parameters.Add(param);
                // perform each action
                tile = action.DoAction(tile);
            }
            
            // record the result
            this.Result = new[] {tile};

            return tile;
        }
    }

    public sealed class RGBToYCbCr : MockCodecBase { public override string Name { get { return "RGB To YCbCr"; } } }
    public sealed class DWT : MockCodecBase { public override string Name { get { return "DWT"; } } }
    public sealed class Quantization : MockCodecBase { public override string Name { get { return "Quantization"; } } }
    public sealed class Linearization : MockCodecBase { public override string Name { get { return "Linearization"; } } }
    public sealed class EntropyEncoding : MockCodecBase { public override string Name { get { return "Entropy Encoding"; } } }

    public sealed class EntropyDecoding : MockCodecBase { public override string Name { get { return "Entropy Decoding"; } } }
    public sealed class SubBandReconstruction : MockCodecBase { public override string Name { get { return "Sub-Band Reconstruction"; } } }
    public sealed class Dequantization : MockCodecBase { public override string Name { get { return "Dequantization"; } } }
    public sealed class InverseDWT : MockCodecBase { public override string Name { get { return "Inverse DWT"; } } }
    public sealed class YCbCrToRGB : MockCodecBase { public override string Name { get { return "YCbCr To RGB"; } } }
    public sealed class ReconstructedFrame : MockCodecBase { public override string Name { get { return "Reconstructed Frame"; } } }
}
