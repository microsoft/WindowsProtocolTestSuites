using System.Collections.Generic;
using CodecToolSet.Core;
using CodecToolSet.Core.RFXDecode;
using System.Linq;

namespace RDPToolSet.Web.Models
{
    public class RFXDecodeSteps
    {
        public ushort EncodedInput = 0;
        public ushort EntropyDecoding = 1;
        public ushort SubBandReconstruction = 2;
        public ushort Dequantization = 3;
        public ushort InverseDWT = 4;
        public ushort YCbCrToRGB = 5;
        public ushort ReconstructedFrame = 6;
    }

    public class RFXDecodeViewModel : ICodecViewModel
    {
        IEnumerable<ICodecParam> _params = new List<ICodecParam>
                {
                    RFXDecodeBase.ENTROPY_ALG,
                    RFXDecodeBase.DEFAULT_QUANT_ARRAY
                };

        IList<PanelViewModel> _panels = new List<PanelViewModel> {
                    new PanelViewModel("encoded-input"          , Constants.DECODE_NAME_TILEINPUT              , true, "Y", "Cb","Cr"),
                    new PanelViewModel("entropy-decoding"       , Constants.DECODE_NAME_RLGRDECODE             , "Y", "Cb", "Cr"),
                    new PanelViewModel("sub-band-reconstruction", Constants.DECODE_NAME_SUBBANDRECONSTRUCTION  , "Y", "Cb", "Cr"),
                    new PanelViewModel("dequantization"         , Constants.DECODE_NAME_DEQUANTIZATION         , "Y", "Cb", "Cr"),
                    new PanelViewModel("inverse-dwt"            , Constants.DECODE_NAME_INVERSEDWT             , "Y", "Cb", "Cr"),
                    new PanelViewModel("ycbcr-to-rgb"           , Constants.DECODE_NAME_YUVTORGB               , "R", "G", "B" ),
                    new PanelViewModel("reconstructed-frame"    , Constants.DECODE_NAME_RECONSTRUCTEDFRAME),
                };

        public string Name
        {
            get { return "RomoteFX Decoding"; }
        }

        public RFXDecodeSteps Steps
        {
            get
            {
                return new RFXDecodeSteps();
            }
        }

        public IEnumerable<ICodecParam> Params
        {
            get
            {
                return _params;
            }
        }

        public IList<PanelViewModel> Panels
        {
            get
            {
                return _panels;
            }
        }

        public IList<PanelViewModel> InPanels
        {
            get
            {
                return new List<PanelViewModel> {
                    new PanelViewModel("encoded-input-in"          , Constants.DECODE_NAME_TILEINPUT              , true, "Y", "Cb", "Cr"),
                    new PanelViewModel("entropy-decoding-in"       , Constants.DECODE_NAME_RLGRDECODE             , true, "Y", "Cb", "Cr"),
                    new PanelViewModel("sub-band-reconstruction-in", Constants.DECODE_NAME_SUBBANDRECONSTRUCTION  , true, "Y", "Cb", "Cr"),
                    new PanelViewModel("dequantization-in"         , Constants.DECODE_NAME_DEQUANTIZATION         , true, "Y", "Cb", "Cr"),
                    new PanelViewModel("inverse-dwt-in"            , Constants.DECODE_NAME_INVERSEDWT             , true, "Y", "Cb", "Cr"),
                    new PanelViewModel("ycbcr-to-rgb-in"           , Constants.DECODE_NAME_YUVTORGB               , true, "Y", "Cb", "Cr" ),
                    new PanelViewModel("reconstructed-frame-in"    , Constants.DECODE_NAME_RECONSTRUCTEDFRAME),
                };
            }
        }

        public void ProvidePanelInputs(params string[] inputs)
        {
            PanelViewModel tileInput = new PanelViewModel("encoded-input", Constants.DECODE_NAME_TILEINPUT, true, new[] { "Y", "Cb", "Cr" }, inputs);
            _panels[0] = tileInput;
            
        }

        public void ProvideParam(params ICodecParam[] parameters)
        {
            _params = parameters.AsEnumerable();
        }
    }

}