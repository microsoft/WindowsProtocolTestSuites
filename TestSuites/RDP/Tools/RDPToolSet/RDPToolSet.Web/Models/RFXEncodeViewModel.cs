using CodecToolSet.Core;
using CodecToolSet.Core.RFXEncode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDPToolSet.Web.Models
{
    public class RFXEncodeSteps
    {
        public ushort ImageInput      = 0;
        public ushort RGBToYCbCr      = 1;
        public ushort DWT             = 2;
        public ushort Quantization    = 3;
        public ushort Linearization   = 4;
        public ushort EntropyEncoding = 5;
    }
    
    public class RFXEncodeViewModel : ICodecViewModel
    {
        public string Name
        {
            get { return "RomoteFX Encoding"; }
        }

        public RFXEncodeSteps Steps
        {
            get { return new RFXEncodeSteps(); }
        }

        public IEnumerable<ICodecParam> Params
        {
            get
            {
                return new List<ICodecParam>
                {
                    RFXEncodeBase.ENTROPY_ALG,

                    RFXEncodeBase.DEFAULT_QUANT_ARRAY
                };
            }
        }

        public IList<PanelViewModel> Panels
        {
            get
            {
                return new List<PanelViewModel> {
                    new PanelViewModel("image-input"      , Constants.ENCODE_NAME_TILEINPUT      , "R", "G" , "B" ),
                    new PanelViewModel("rgb-to-ycbcr"     , Constants.ENCODE_NAME_RGBTOYUV       , "Y", "Cb", "Cr"),
                    new PanelViewModel("dwt"              , Constants.ENCODE_NAME_DWT            , "Y", "Cb", "Cr"),
                    new PanelViewModel("quantization"     , Constants.ENCODE_NAME_QUANTIZATION   , "Y", "Cb", "Cr"),
                    new PanelViewModel("linearization"    , Constants.ENCODE_NAME_LINEARIZATION  , "Y", "Cb", "Cr"),
                    new PanelViewModel("entropy-encoding" , Constants.ENCODE_NAME_RLGRENCODE     , "Y", "Cb", "Cr"),
                };
            }
        }

        public IList<PanelViewModel> InPanels
        {
            get
            {
                return new List<PanelViewModel> {
                    new PanelViewModel("image-input-in"      , Constants.ENCODE_NAME_TILEINPUT      , true , "R", "G" , "B" ),
                    new PanelViewModel("rgb-to-ycbcr-in"     , Constants.ENCODE_NAME_RGBTOYUV       , true , "R", "G" , "B"),
                    new PanelViewModel("dwt-in"              , Constants.ENCODE_NAME_DWT            , true , "Y", "Cb", "Cr"),
                    new PanelViewModel("quantization-in"     , Constants.ENCODE_NAME_QUANTIZATION   , true , "Y", "Cb", "Cr"),
                    new PanelViewModel("linearization-in"    , Constants.ENCODE_NAME_LINEARIZATION  , true , "Y", "Cb", "Cr"),
                    new PanelViewModel("entropy-encoding-in" , Constants.ENCODE_NAME_RLGRENCODE     , true , "Y", "Cb", "Cr"),
                };
            }
        }
    }
}