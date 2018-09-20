using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodecToolSet.Core;
using CodecToolSet.Core.RFXPEncode;

namespace RDPToolSet.Web.Models
{
    public class RFXPEncodeSteps
    {
        public ushort ImageInput = 0;
        public ushort RGBToYCbCr = 1;
        public ushort DWT = 2;
        public ushort Quantization = 3;
        public ushort Linearization = 4;
        public ushort SubBandDiffing = 5;
        public ushort ProgQuantization = 6;
        public ushort EntropyEncoding = 7;
    }

    public class RFXPEncodeViewModel : ICodecViewModel
    {
        public string Name
        {
            get { return "RomoteFX Encoding"; }
        }

        public RFXPEncodeSteps Steps
        {
            get { return new RFXPEncodeSteps(); }
        }

        public IEnumerable<ICodecParam> Params
        {
            get
            {
                return new List<ICodecParam>
                {
                    RFXPEncodeBase.ENTROPY_ALG,

                    RFXPEncodeBase.DEFAULT_QUANT_ARRAY,

                    RFXPEncodeBase.DEFAULT_USE_DIFFERENCE_TILE,

                    RFXPEncodeBase.DEFAULT_USE_REDUCE_EXTRAPOLATE,

                    RFXPEncodeBase.DEFAULT_PROG_QUANT
                };
            }
        }

        public IList<PanelViewModel> Panels
        {
            get
            {
                return new List<PanelViewModel> {
                    new PanelViewModel("image-input"      , Constants.PENCODE_NAME_TILEINPUT                   , "R", "G" , "B" ),
                    new PanelViewModel("rgb-to-ycbcr"     , Constants.PENCODE_NAME_RGBTOYUV                    , "Y", "Cb", "Cr"),
                    new PanelViewModel("dwt"              , Constants.PENCODE_NAME_DWT                         , "Y", "Cb", "Cr"),
                    new PanelViewModel("quantization"     , Constants.PENCODE_NAME_QUANTIZATION                , "Y", "Cb", "Cr"),
                    new PanelViewModel("linearization"    , Constants.PENCODE_NAME_LINEARIZATION               , "Y", "Cb", "Cr"),
                    new PanelViewModel("subband-diffing"  , Constants.PENCODE_NAME_SUBBANDDIFFING              , "Y", "Cb", "Cr"),
                    new PanelViewModel("progquantization" , Constants.PENCODE_NAME_PROGRESSIVEQUANTIZATION     , "Y", "Cb", "Cr"),
                    new PanelViewModel("entropy-encoding" , Constants.PENCODE_NAME_RLGRSRLENCODE               , "Y", "Cb", "Cr")
                };
            }
        }

        public IList<PanelViewModel> InPanels
        {
            get
            {
                return new List<PanelViewModel> {
                    new PanelViewModel("image-input-in"      , Constants.PENCODE_NAME_TILEINPUT                   , true , "R", "G" , "B" ),
                    new PanelViewModel("rgb-to-ycbcr-in"     , Constants.PENCODE_NAME_RGBTOYUV                    , true , "R", "G", "B"),
                    new PanelViewModel("dwt-in"              , Constants.PENCODE_NAME_DWT                         , true , "Y", "Cb", "Cr"),
                    new PanelViewModel("quantization-in"     , Constants.PENCODE_NAME_QUANTIZATION                , true , "Y", "Cb", "Cr"),
                    new PanelViewModel("linearization-in"    , Constants.PENCODE_NAME_LINEARIZATION               , true , "Y", "Cb", "Cr"),
                    new PanelViewModel("subband-diffing-in"  , Constants.PENCODE_NAME_SUBBANDDIFFING              , true , "Y", "Cb", "Cr"),
                    new PanelViewModel("progquantization-in" , Constants.PENCODE_NAME_PROGRESSIVEQUANTIZATION     , true , "Y", "Cb", "Cr"),
                    new PanelViewModel("entropy-encoding-in" , Constants.PENCODE_NAME_RLGRSRLENCODE               , true , "Y", "Cb", "Cr")
                };
            }
        }
    }
}