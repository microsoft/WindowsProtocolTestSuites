using System.Collections.Generic;
using System.Linq;

namespace CodecToolSet.Core
{
    public class CodecFactory
    {
        private static readonly IEnumerable<ICodecAction> Codecs = new List<ICodecAction> {
            // RFX Encode
            new RFXEncode.RFXEncode(),
            new RFXEncode.TileInput(),
            new RFXEncode.RGBToYUV(),
            new RFXEncode.DWT(),
            new RFXEncode.Quantization(),
            new RFXEncode.Linearization(),
            new RFXEncode.RLGREncode(),

            // RFX Decode
            new RFXDecode.RFXDecode(),
            new RFXDecode.RLGRDecode(),
            new RFXDecode.SubBandReconstruction(),
            new RFXDecode.Dequantization(),
            new RFXDecode.InverseDWT(),
            new RFXDecode.YUVToRGB(),

            // RFX Progressive Encode
            new RFXPEncode.TileInput(),
            new RFXPEncode.RGBToYUV(),
            new RFXPEncode.DWT(),
            new RFXPEncode.Quantization(),
            new RFXPEncode.Linearization(),
            new RFXPEncode.SubBandDiffing(),
            new RFXPEncode.ProgressiveQuantization(),
            new RFXPEncode.RLGR_SRLEncode(),

            // RFX Progressive Decode
            new RFXPDecode.RLGR_SRLDecode(),
            new RFXPDecode.ProgressiveDequantization(),
            new RFXPDecode.SubBandDiffing(),
            new RFXPDecode.SubBandReconstruction(),
            new RFXPDecode.DeQuantization(),
            new RFXPDecode.InverseDWT(),
            new RFXPDecode.YUVToRGB(),
            new RFXPDecode.RGBToImage()
        };

        // use linq to query the codec action, or add the name-transform 
        // into a dictionary ??
        public static ICodecAction GetCodecAction(string name)
        {
            return Codecs.SingleOrDefault(c => c.Name.Equals(name));
        }
    }
}