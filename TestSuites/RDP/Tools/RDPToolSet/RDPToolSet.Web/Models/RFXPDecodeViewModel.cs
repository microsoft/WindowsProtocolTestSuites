using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodecToolSet.Core;
using CodecToolSet.Core.RFXPDecode;

namespace RDPToolSet.Web.Models
{
    public class RFXPDecodeSteps
    {
        public ushort TileInput = 0;
        public ushort EntropyDecoding = 1;
        public ushort ProgDeQuantization = 2;
        public ushort SubBandDiffing = 3;
        public ushort SubBandRestruction = 4;
        public ushort DeQuantization = 5;
        public ushort InverseDWT = 6;
        public ushort YCbCrToRGB = 7;
        public ushort RestructFrame = 8;
    }

    public class RFXPDecodeViewModel : ICodecViewModel
    {
        IEnumerable<ICodecParam> _params = new List<ICodecParam>
                {
                    RFXPDecodeBase.ENTROPY_ALG,

                    RFXPDecodeBase.DEFAULT_QUANT_ARRAY,

                    RFXPDecodeBase.DEFAULT_PROG_QUANT,

                    RFXPDecodeBase.DEFAULT_USE_DIFFERENCE_TILE,

                    RFXPDecodeBase.DEFAULT_USE_REDUCE_EXTRAPOLATE
                };

        public RFXPDecodeViewModel(int layer)
        {
            SetLayer(layer);
        }

        public void SetLayer(int layer)
        {
            Layer = layer;

            var followingPanels = FollowingList(layer);

            List<PanelViewModel> panels;
            if (layer == 0)
            {
                panels = new List<PanelViewModel> {
                    new PanelViewModel("tile-input-layer0"      , Constants.PDECODE_NAME_TILEINPUT              , true, "Y", "Cb" , "Cr" )
                };
                panels.AddRange(followingPanels);
                Panels = panels;
            }
            else if (layer >= 1)
            {
                panels = new List<PanelViewModel> {
                    new PanelViewModel("tile-input-layer" + layer      , Constants.PDECODE_NAME_TILEINPUT              , true, "Y", "Y Raw Data", "Cb", "Cb Raw Data", "Cr", "Cr Raw Data" )
                };
                panels.AddRange(followingPanels);
                Panels = panels;
            }

            // InPanels
            var inFollowingPanels = InFollowingList(layer);
            List<PanelViewModel> inpanelist;
            if (layer == 0)
            {
                inpanelist = new List<PanelViewModel> {
                    null,
                    new PanelViewModel("entropy-decoding-in-layer0"      , Constants.PDECODE_NAME_TILEINPUT              , true, "Y", "Cb" , "Cr" )
                };
                inpanelist.AddRange(inFollowingPanels);
                InPanels = inpanelist;
            }
            else if (layer >= 1)
            {
                inpanelist = new List<PanelViewModel> {
                    null,
                    new PanelViewModel("entropy-decoding-in-layer" + layer      , Constants.PDECODE_NAME_TILEINPUT              , true, "Y", "Y Raw Data", "Cb", "Cb Raw Data", "Cr", "Cr Raw Data" )
                };
                inpanelist.AddRange(inFollowingPanels);
                InPanels = inpanelist;
            }
        }

        public string Name
        {
            get { return "RomoteFX Encoding"; }
        }

        public int Layer { get; set; }

        public RFXPDecodeSteps Steps
        {
            get { return new RFXPDecodeSteps(); }
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
            get;
            set;
        }

        public IList<PanelViewModel> InPanels
        {
            get;
            set;
        }

        private List<PanelViewModel> FollowingList(int layer)
        {
            var followingPanels = new List<PanelViewModel>{
                    new PanelViewModel("entropy-decoding-in-layer" + layer   , Constants.PDECODE_NAME_RLGRSRLDECODE              , "Y", "Cb", "Cr"),
                    new PanelViewModel("prog-dequantization-layer" + layer   , Constants.PDECODE_NAME_PROGRESSIVEDEQUANTIZATION  , "Y", "Cb", "Cr"),
                    new PanelViewModel("subband-diffing-layer" + layer       , Constants.PDECODE_NAME_SUBBANDDIFFING             , "Y", "Cb", "Cr"),
                    new PanelViewModel("subband-restruction-layer" + layer   , Constants.PDECODE_NAME_SUBBANDRECONSTRUCTION      , "Y", "Cb", "Cr"),
                    new PanelViewModel("dequantization-layer" + layer        , Constants.PDECODE_NAME_DEQUANTIZATION             , "Y", "Cb", "Cr"),
                    new PanelViewModel("inserse-dwt-layer" + layer           , Constants.PDECODE_NAME_INVERSEDWT                 , "Y", "Cb", "Cr"),
                    new PanelViewModel("yuv-to-rgb-layer" + layer            , Constants.PDECODE_NAME_YUVTORGB                   , "R", "G", "B"),
                    new PanelViewModel("restruct-frame-layer" + layer        , Constants.PDECODE_NAME_RECONSTRUCTEDFRAME)
            };
            return followingPanels;
        }

        private List<PanelViewModel> InFollowingList(int layer)
        {
            var followingPanels = new List<PanelViewModel>{
                    new PanelViewModel("prog-dequantization-in-layer" + layer   , Constants.PDECODE_NAME_PROGRESSIVEDEQUANTIZATION  , true, "Y", "Cb", "Cr"),
                    new PanelViewModel("subband-diffing-in-layer" + layer       , Constants.PDECODE_NAME_SUBBANDDIFFING             , true, "Y", "Cb", "Cr"),
                    new PanelViewModel("subband-restruction-in-layer" + layer   , Constants.PDECODE_NAME_SUBBANDRECONSTRUCTION      , true, "Y", "Cb", "Cr"),
                    new PanelViewModel("dequantization-in-layer" + layer        , Constants.PDECODE_NAME_DEQUANTIZATION             , true, "Y", "Cb", "Cr"),
                    new PanelViewModel("inserse-dwt-in-layer" + layer           , Constants.PDECODE_NAME_INVERSEDWT                 , true, "Y", "Cb", "Cr"),
                    new PanelViewModel("yuv-to-rgb-in-layer" + layer            , Constants.PDECODE_NAME_YUVTORGB                   , true, "Y", "Cb", "Cr"),
                    new PanelViewModel("restruct-frame-in-layer" + layer        , Constants.PDECODE_NAME_RECONSTRUCTEDFRAME)
            };
            return followingPanels;
        }

        public void ProvidePanelInputs(int layer, params string[] inputs)
        {
            if (layer == 0)
            {
                var tileInput = new PanelViewModel("tile-input-layer0", Constants.PDECODE_NAME_TILEINPUT, true, new[] { "Y", "Cb", "Cr" }, inputs);
                Panels[0] = tileInput;
            }
            else
            {
                var tileInput = new PanelViewModel("tile-input-layer" + layer, Constants.PDECODE_NAME_TILEINPUT, true, new[] { "Y", "Y Raw Data", "Cb", "Cb Raw Data", "Cr", "Cr Raw Data" }, inputs);
                Panels[0] = tileInput;
            }

        }

        public void ProvideParam(params ICodecParam[] parameters)
        {
            _params = parameters.AsEnumerable();
        }
    }
}