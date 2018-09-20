using System.Collections.Generic;
namespace CodecToolSet.Core
{
    // declares a make-up interface
    public interface ICodecParam {}

    public class QuantizationFactors : ICodecParam
    {
        public byte LL3 { get; set; }
        public byte LH3 { get; set; }
        public byte HL3 { get; set; }
        public byte HH3 { get; set; }
        public byte LH2 { get; set; }
        public byte HL2 { get; set; }
        public byte HH2 { get; set; }
        public byte LH1 { get; set; }
        public byte HL1 { get; set; }
        public byte HH1 { get; set; }
    }

    // TODO: Make sure there are three components for this
    public class QuantizationFactorsArray : ICodecParam
    {
        public QuantizationFactors[] quants;
    }

    public class ProgressiveQuantizationFactors : ICodecParam
    {
        public List<QuantizationFactorsArray> ProgQuants;
        
        public ProgressiveQuantizationFactors()
        {
            ProgQuants = new List<QuantizationFactorsArray>();
        }
        
    }

    public class EntropyAlgorithm : ICodecParam
    {
        public enum AlgorithmEnum {
            None,
            RLGR1,
            RLGR3
        }

        public AlgorithmEnum Algorithm
        {
            get;
            set;
        }
    }

    public class UseReduceExtrapolate : ICodecParam
    {
        public bool Enabled
        {
            get;
            set;
        }
    }

    public class UseDifferenceTile : ICodecParam
    {
        public bool Enabled
        {
            get;
            set;
        }
    }

    public class Frame : ICodecParam
    {
        public Tile Tile
        {
            get;
            set;
        }
    }

    public class EncodedTileType : ICodecParam
    {
        public enum EncodedType
        {
            Simpe,
            FirstPass,
            UpgradePass
        }

        public EncodedType Type
        {
            get;
            set;
        }

    }

}