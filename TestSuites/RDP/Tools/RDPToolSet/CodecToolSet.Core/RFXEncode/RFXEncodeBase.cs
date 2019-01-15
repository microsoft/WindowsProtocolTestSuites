using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;


// ReSharper disable once CheckNamespace
namespace CodecToolSet.Core.RFXEncode
{
    public abstract class RFXEncodeBase : ICodecAction
    {
        protected RFXEncodeBase()
        {
            Parameters = new Dictionary<String, ICodecParam>();
        }

        public static QuantizationFactors DEFAULT_QUANT = new QuantizationFactors
        {
            LL3 = 0x6,
            LH3 = 0x6,
            HL3 = 0x6,
            HH3 = 0x6,
            LH2 = 0x7,
            HL2 = 0x7,
            HH2 = 0x8,
            LH1 = 0x8,
            HL1 = 0x8,
            HH1 = 0x9
        };

        public static QuantizationFactorsArray DEFAULT_QUANT_ARRAY = new QuantizationFactorsArray
        {
            quants = new[]{
                DEFAULT_QUANT,
                DEFAULT_QUANT,
                DEFAULT_QUANT
            }
        };

        public static EntropyAlgorithm ENTROPY_ALG = new EntropyAlgorithm
        {
            Algorithm = EntropyAlgorithm.AlgorithmEnum.RLGR1
        };

        public virtual String Name {
            get
            {
                return Constants.ENCODE_NAME_RFXENCODE;
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
                    return RFXEncodeBase.DEFAULT_QUANT_ARRAY;
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
                    return RFXEncodeBase.ENTROPY_ALG;
                }
            }
        }

        public virtual Tile[] Result { get; protected set; }

        public virtual Tile[] Input { get; protected set; }

        public abstract Tile DoAction(Tile input);

        public virtual Tile[] DoAction(Tile[] inputs)
        {
            return new[] { DoAction(inputs.FirstOrDefault()) };
        }

        public IEnumerable<ICodecAction> SubActions { get; protected set; }

        public Dictionary<String, ICodecParam> Parameters { get; protected set; }
    }
}