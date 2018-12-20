using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodecToolSet.Core.RFXPDecode
{
    public abstract class RFXPDecodeBase : ICodecAction
    {
        protected RFXPDecodeBase()
        {
            Parameters = new Dictionary<String, ICodecParam>();
        }

        public static QuantizationFactors DEFAULT_QUANT = new QuantizationFactors
        {
            // Media
            LL3 = 0x6,
            HL3 = 0x6,
            LH3 = 0x6,
            HH3 = 0x6,
            HL2 = 0x8,
            LH2 = 0x7,
            HH2 = 0x7,
            HL1 = 0x7,
            LH1 = 0x9,
            HH1 = 0x8
        };

        public static QuantizationFactorsArray DEFAULT_QUANT_ARRAY = new QuantizationFactorsArray
        {
            quants = new[]{
                DEFAULT_QUANT,
                DEFAULT_QUANT,
                DEFAULT_QUANT
            }
        };

        public static ProgressiveQuantizationFactors DEFAULT_PROG_QUANT = new ProgressiveQuantizationFactors
        {
            ProgQuants = new List<QuantizationFactorsArray>
            {
                Utility.GetProgQuantizationFactorsForChunk(Utility.ProgressiveChunk.kChunk_25),
                Utility.GetProgQuantizationFactorsForChunk(Utility.ProgressiveChunk.kChunk_50),
                Utility.GetProgQuantizationFactorsForChunk(Utility.ProgressiveChunk.kChunk_75),
                Utility.GetProgQuantizationFactorsForChunk(Utility.ProgressiveChunk.kChunk_100)
            }
        };

        public static EntropyAlgorithm ENTROPY_ALG = new EntropyAlgorithm
        {
            Algorithm = EntropyAlgorithm.AlgorithmEnum.RLGR1
        };

        public static UseReduceExtrapolate DEFAULT_USE_REDUCE_EXTRAPOLATE = new UseReduceExtrapolate
        {
            Enabled = true
        };

        public static UseDifferenceTile DEFAULT_USE_DIFFERENCE_TILE = new UseDifferenceTile
        {
            Enabled = true
        };

        #region Properties
        public virtual String Name
        {
            get
            {
                return Constants.DECODE_NAME_RFXDECODE;
            }
        }

        public QuantizationFactorsArray QuantArray
        {
            get
            {
                var parameter = Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY];
                if (parameter is QuantizationFactorsArray)
                {
                    return (QuantizationFactorsArray)parameter;
                }
                else
                {
                    return RFXPDecodeBase.DEFAULT_QUANT_ARRAY;
                }
            }
        }

        public ProgressiveQuantizationFactors ProgQuants
        {
            get
            {
                var parameter = Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANT_LIST];
                if (parameter is ProgressiveQuantizationFactors)
                {
                    return (ProgressiveQuantizationFactors)parameter;
                }
                else
                {
                    return RFXPDecodeBase.DEFAULT_PROG_QUANT;
                }
            }
        }

        public UseReduceExtrapolate UseReduceExtrapolate
        {
            get
            {
                var parameter = Parameters[Constants.PARAM_NAME_USE_REDUCE_EXTRAPOLATE];
                if (parameter is UseReduceExtrapolate)
                {
                    return (UseReduceExtrapolate)parameter;
                }
                else
                {
                    return RFXPDecodeBase.DEFAULT_USE_REDUCE_EXTRAPOLATE;
                }
            }
        }

        public UseDifferenceTile ParamUseDifferenceTile
        {
            get
            {
                var parameter = Parameters[Constants.PARAM_NAME_USE_DIFFERENCE_TILE];
                if (parameter is UseDifferenceTile)
                {
                    return (UseDifferenceTile)parameter;
                }
                else
                {
                    return RFXPDecodeBase.DEFAULT_USE_DIFFERENCE_TILE;
                }
            }
        }

        public EntropyAlgorithm Mode
        {
            get
            {
                var parameter = Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM];
                if (parameter is EntropyAlgorithm)
                {
                    return (EntropyAlgorithm)parameter;
                }
                else
                {
                    return RFXPDecodeBase.ENTROPY_ALG;
                }
            }
        }
        #endregion

        public virtual Tile[] Result { get; protected set; }

        public virtual Tile[] Input { get; protected set; }

        public virtual Tile DoAction(Tile input)
        {
            throw new NotImplementedException();
        }

        public abstract Tile[] DoAction(Tile[] inputs);

        public IEnumerable<ICodecAction> SubActions { get; protected set; }

        public Dictionary<String, ICodecParam> Parameters { get; protected set; }
    }
}
