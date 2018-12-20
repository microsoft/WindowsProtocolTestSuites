using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodecToolSet.Core;
using CodecToolSet.Core.RFXPEncode;
using RDPToolSet.Web.Models;

namespace RDPToolSet.Web.Controllers
{
    public class RFXPEncodeController : CodecBaseController
    {
        public override ActionResult Index()
        {
            var rfxPEncode = new RFXPEncode();
            var rfxPEnocdeModel = new RFXPEncodeViewModel();
            var envValues = new Dictionary<string, object>
           {
               {ActionKey, rfxPEncode},
               {ModelKey, rfxPEnocdeModel}
           };
            SaveToSession(envValues);
            LoadFromSession();
            // Clear the previous frame in session
            Session[PreviousFrameImage] = null;
            return View(_viewModel);
        }

        [HttpPost]
        public ActionResult Encode()
        {
            dynamic obj = GetJsonObject(Request.InputStream);

            // retrieve the quant
            var quantArray = JsonHelper.RetrieveQuantsArray(obj.Params.QuantizationFactorsArray);
            _codecAction.Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = quantArray;

            // retrive the progressive quants
            var progQuantList = new List<QuantizationFactorsArray>();
            foreach (var layer in obj.Params.ProgQuantizationArray)
            {
                var layerQuants = JsonHelper.RetrieveQuantsArray(layer);
                progQuantList.Add(layerQuants);
            }
            var progQuantarray = new ProgressiveQuantizationFactors
            {
                ProgQuants = progQuantList
            };
            _codecAction.Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANT_LIST] = progQuantarray;
                       
            _codecAction.Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = JsonHelper.CastTo<EntropyAlgorithm>(obj.Params.EntropyAlgorithm);
            _codecAction.Parameters[Constants.PARAM_NAME_USE_REDUCE_EXTRAPOLATE] = JsonHelper.CastTo<UseReduceExtrapolate>(obj.Params.UseReduceExtrapolate);

            // TODO: error handle
            var preFramePath = (string)Session[PreviousFrameImage];
            Frame preFrame = Utility.GetPreviousFrame(preFramePath, _codecAction.Parameters);

            var encodeImagePath = (string)Session[EncodedImage];
            var tile = Tile.FromFile(encodeImagePath);

            ICodecAction diffing = _codecAction.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PENCODE_NAME_SUBBANDDIFFING));
            diffing.Parameters[Constants.PARAM_NAME_PREVIOUS_FRAME] = preFrame;

            _codecAction.DoAction(new[] { tile });

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // This Action may cost much time. TODO: improve it
        public override ActionResult LayerPanel()
        {
            dynamic obj = CodecBaseController.GetJsonObject(Request.InputStream);

            string name = JsonHelper.CastTo<string>(obj.name);

            var layers = new List<PanelViewModel>();
            // gets the action with the same name as the argument
            var action = _codecAction.SubActions.SingleOrDefault(c => c.Name.Equals(name));
            if (action == null || action.Result == null) return PartialView("_Layers", layers);

            layers = GenerateLayers(name, action.Result, false);
            // add UseDifferenceTile
            ICodecAction subbandDiffingAction = _codecAction.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PDECODE_NAME_SUBBANDDIFFING));
            if (subbandDiffingAction != null)
            {
                var parameters = new Dictionary<string, string>();
                var use_diffing = "False";
                if(action.Parameters.ContainsKey(Constants.PARAM_NAME_USE_DIFFERENCE_TILE) &&
                    action.Parameters[Constants.PARAM_NAME_USE_DIFFERENCE_TILE] is UseDifferenceTile &&
                    ((UseDifferenceTile)action.Parameters[Constants.PARAM_NAME_USE_DIFFERENCE_TILE]).Enabled)
                {
                    use_diffing = "True";
                }
                parameters.Add("use-diffing", use_diffing);
                ViewBag.Parameters = parameters;
            }

            return PartialView("_Layers", layers);
        }

        public override ActionResult InputPanel()
        {
            dynamic obj = CodecBaseController.GetJsonObject(Request.InputStream);

            string name = JsonHelper.CastTo<string>(obj.name);

            // gets the action with the same name as the argument
            var layers = new List<PanelViewModel>();
            var action = _codecAction.SubActions.SingleOrDefault(c => c.Name.Equals(name));
            if (action == null || action.Result == null) return PartialView("_Layers", layers);

            layers = GenerateLayers(name, action.Input, true);

            return PartialView("_Layers", layers);
        }

        private List<PanelViewModel> GenerateLayers(string name, Tile[] layerData, bool editable)
        {
            var layers = new List<PanelViewModel>();
            var formedstr = name.Replace(' ', '-').Replace('/', '-');
            if ((!name.Equals(Constants.PENCODE_NAME_RLGRSRLENCODE)) || editable)
            {
                
                for (int index = 0; index < layerData.Length; index++)
                {
                    string idPrefix = editable ? formedstr + "-input-layer-" + index : formedstr + "-output-layer-" + index;
                    var layer = new PanelViewModel(idPrefix, "Layer " + index);
                    Tile result = layerData[index];
                    Triplet<string> triplet = result.GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                    var tabx = new TabViewModel { Id = idPrefix + "-Y", Title = "Y", Content = triplet.X, Editable = editable };
                    var taby = new TabViewModel { Id = idPrefix + "-Cb", Title = "Cb", Content = triplet.Y, Editable = editable };
                    var tabz = new TabViewModel { Id = idPrefix + "-Cr", Title = "Cr", Content = triplet.Z, Editable = editable };
                    layer.Tabs = new List<TabViewModel> { tabx, taby, tabz };
                    layers.Add(layer);
                }
            }
            else
            {
                // first Pass
                string idPrefix = editable ? formedstr + "-input-layer-0" : formedstr + "-output-layer-0";
                var layer = new PanelViewModel(idPrefix, "Layer 0");
                Tile result = layerData[0];
                Triplet<string> triplet = result.GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                var tabx = new TabViewModel { Id = idPrefix + "-Y", Title = "Y", Content = triplet.X, Editable = editable };
                var taby = new TabViewModel { Id = idPrefix + "-Cb", Title = "Cb", Content = triplet.Y, Editable = editable };
                var tabz = new TabViewModel { Id = idPrefix + "-Cr", Title = "Cr", Content = triplet.Z, Editable = editable };
                layer.Tabs = new List<TabViewModel> { tabx, taby, tabz };
                layers.Add(layer);
                // layers
                for (int index = 1; index < layerData.Length; index += 2)
                {
                    idPrefix = editable ? formedstr + "-input-layer-" + index : formedstr + "-output-layer-" + index;
                    layer = new PanelViewModel(idPrefix, "Layer " + (index + 1) / 2);

                    result = layerData[index];
                    triplet = result.GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                    tabx = new TabViewModel { Id = idPrefix + "-Y", Title = "Y", Content = triplet.X, Editable = editable };
                    taby = new TabViewModel { Id = idPrefix + "-Cb", Title = "Cb", Content = triplet.Y, Editable = editable };
                    tabz = new TabViewModel { Id = idPrefix + "-Cr", Title = "Cr", Content = triplet.Z, Editable = editable };

                    Tile raw = layerData[index + 1];
                    Triplet<string> rawTriplet = raw.GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                    var rawx = new TabViewModel { Id = idPrefix + "-raw-Y", Title = "Y Raw Data", Content = rawTriplet.X, Editable = editable };
                    var rawy = new TabViewModel { Id = idPrefix + "-raw-Cb", Title = "Cb Raw Data", Content = rawTriplet.Y, Editable = editable };
                    var rawz = new TabViewModel { Id = idPrefix + "-raw-Cr", Title = "Cr Raw Data", Content = rawTriplet.Z, Editable = editable };

                    layer.Tabs = new List<TabViewModel> { tabx, rawx, taby, rawy, tabz, rawz };
                    layers.Add(layer);
                }
            }
            return layers;
        }

        public override ActionResult Recompute()
        {
            // TODO: Add parameters obtain into a function
            dynamic obj = GetJsonObject(Request.InputStream);

            string name = JsonHelper.CastTo<string>(obj.Action);

            // gets the action with the same name as the argument
            var action = _codecAction.SubActions.SingleOrDefault(c => c.Name.Equals(name));

            // if action not found
            if (action == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            // retrieve the quant
            var quantArray = JsonHelper.RetrieveQuantsArray(obj.Params.QuantizationFactorsArray);
            _codecAction.Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = quantArray;

            // retrive the progressive quants
            var progQuantList = new List<QuantizationFactorsArray>();
            foreach (var layer in obj.Params.ProgQuantizationArray)
            {
                var layerQuants = JsonHelper.RetrieveQuantsArray(layer);
                progQuantList.Add(layerQuants);
            }
            var progQuantarray = new ProgressiveQuantizationFactors
            {
                ProgQuants = progQuantList
            };
            _codecAction.Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANT_LIST] = progQuantarray;

            _codecAction.Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = JsonHelper.CastTo<EntropyAlgorithm>(obj.Params.EntropyAlgorithm);
            _codecAction.Parameters[Constants.PARAM_NAME_USE_REDUCE_EXTRAPOLATE] = JsonHelper.CastTo<UseReduceExtrapolate>(obj.Params.UseReduceExtrapolate);

            // TODO: error handle
            var preFramePath = (string)Session[PreviousFrameImage];
            Frame preFrame = Utility.GetPreviousFrame(preFramePath, _codecAction.Parameters);

            ICodecAction diffing = _codecAction.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PENCODE_NAME_SUBBANDDIFFING));
            diffing.Parameters[Constants.PARAM_NAME_PREVIOUS_FRAME] = preFrame;

            // retrive tiles from Inputs
            var tileList = new List<Tile>();
            foreach (var tileJson in obj.Inputs)
            {
                Triplet<string> triplet = JsonHelper.RetrieveTriplet(tileJson);

                string dataFormat = obj.Params.UseDataFormat;
                Tile tile = null;
                if (dataFormat.Equals(Constants.DataFormat.HEX))
                {
                    tile = Tile.FromStrings(triplet, new HexTileSerializer());
                }
                else
                {
                    tile = Tile.FromStrings(triplet, new IntegerTileSerializer());
                }
                if (dataFormat.Equals(Constants.DataFormat.FixedPoint_11_5))
                {
                    tile.RightShift(5);
                }
                if (dataFormat.Equals(Constants.DataFormat.FixedPoint_12_4))
                {
                    tile.RightShift(4);
                }

                tileList.Add(tile);
            }

            var result = action.DoAction(tileList.ToArray());

            // recompute the following steps and update
            bool following = false;
            Tile[] input = result;
            foreach (var act in _codecAction.SubActions)
            {
                if (following)
                {
                    result = act.DoAction(input);
                    input = result;
                }
                else
                {
                    if (act.Name.Equals(name))
                    {
                        following = true;
                    }
                }
            }

            // TODO: recompute the following steps and update

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}
