using System.Collections.Concurrent;

namespace CodecToolSet.Core
{
    public static class Constants
    {
        // params
        public const string PARAM_NAME_QUANT_FACTORS = "QuantizationFactors";
        public const string PARAM_NAME_QUANT_FACTORS_ARRAY = "QuantizationFactorsArray";
        public const string PARAM_NAME_ENTROPY_ALGORITHM = "EntropyAlgorithm";
        public const string PARAM_NAME_USE_REDUCE_EXTRAPOLATE = "UseReduceExtrapolate";
        public const string PARAM_NAME_USE_DIFFERENCE_TILE = "UseDifferenceTile";
        public const string PARAM_NAME_PREVIOUS_FRAME = "PreviousFrame";
        public const string PARAM_NAME_ENCODED_TILE_TYPE = "EncodedTileType";
        public const string PARAM_NAME_DAS = "DAS";
        public const string PARAM_NAME_PREVIOUS_PROGRESSIVE_QUANTS = "PreviousProgressiveQuant";
        public const string PARAM_NAME_PROGRESSIVE_QUANTS = "ProgressiveQaunt";
        public const string PARAM_NAME_PROGRESSIVE_QUANT_LIST = "ProgressiveQauntList";

        // RFXEncode
        public const string ENCODE_NAME_RFXENCODE = "RFXEncode";
        public const string ENCODE_NAME_TILEINPUT = "Tile Input";
        public const string ENCODE_NAME_RGBTOYUV = "RGB To YUV";
        public const string ENCODE_NAME_DWT = "DWT";
        public const string ENCODE_NAME_QUANTIZATION = "Quantization";
        public const string ENCODE_NAME_LINEARIZATION = "Linearization";
        public const string ENCODE_NAME_RLGRENCODE = "RLGR Encode";

        // RFXDecode
        public const string DECODE_NAME_RFXDECODE = "RFX Decode";
        public const string DECODE_NAME_TILEINPUT = "Tile Input";
        public const string DECODE_NAME_RLGRDECODE = "RLGR Decode";
        public const string DECODE_NAME_SUBBANDRECONSTRUCTION = "Sub-Band Reconstruction";
        public const string DECODE_NAME_DEQUANTIZATION = "Dequantization";
        public const string DECODE_NAME_INVERSEDWT = "Inverse DWT";
        public const string DECODE_NAME_YUVTORGB = "YUV To RGB";
        public const string DECODE_NAME_RECONSTRUCTEDFRAME = "Reconstructed Frame";

        // RFXPEncode
        public const string PENCODE_NAME_RFXPENCODE = "RFX Progressive Encode";
        public const string PENCODE_NAME_TILEINPUT = "Tile Input";
        public const string PENCODE_NAME_RGBTOYUV = "RGB To YUV";
        public const string PENCODE_NAME_DWT = "DWT";
        public const string PENCODE_NAME_QUANTIZATION = "Quantization";
        public const string PENCODE_NAME_LINEARIZATION = "Linearization";
        public const string PENCODE_NAME_SUBBANDDIFFING = "Sub-Band Diffing";
        public const string PENCODE_NAME_PROGRESSIVEQUANTIZATION = "Progressive Quantization";
        public const string PENCODE_NAME_RLGRSRLENCODE = "RLGR/SRL Encode";
        
        // RFXPDecode
        public const string PDECODE_NAME_TILEINPUT = "Tile Input";
        public const string PDECODE_NAME_RFXPDECODE = "RFX Progressive Decode";
        public const string PDECODE_NAME_RLGRSRLDECODE = "RLGR/SRL Decode";
        public const string PDECODE_NAME_PROGRESSIVEDEQUANTIZATION = "Progressive DeQuantization";
        public const string PDECODE_NAME_SUBBANDDIFFING = "Sub-Band Diffing";
        public const string PDECODE_NAME_SUBBANDRECONSTRUCTION = "Sub-Band Reconstruction";
        public const string PDECODE_NAME_DEQUANTIZATION = "DeQuantization";
        public const string PDECODE_NAME_INVERSEDWT = "Inverse DWT";
        public const string PDECODE_NAME_YUVTORGB = "YUV To RGB";
        public const string PDECODE_NAME_RECONSTRUCTEDFRAME = "Reconstructed Frame";

        public class DataFormat
        {
            public const string DEC = "dec";
            public const string HEX = "hex";
            public const string Integer = "Integer";
            public const string FixedPoint_11_5 = "11.5";
            public const string FixedPoint_12_4 = "12.4";
        }
       
    }
}
