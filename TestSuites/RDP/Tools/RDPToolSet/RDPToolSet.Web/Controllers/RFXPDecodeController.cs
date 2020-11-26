// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CodecToolSet.Core;
using CodecToolSet.Core.RFXPDecode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RDPToolSet.Web.Models;
using RDPToolSet.Web.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RDPToolSet.Web.Controllers
{
    public class RFXPDecodeController : CodecBaseController
    {
        const string ConstantDAS = "DAS";
        const string ConstantDASDic = "DASDic";
        const string ConstantCodecActionDic = "CodecActionDic";
        const string DecodePreviousFrame = "PreFrame";
        const string PreviousFrameDic = "PreFrameDic";
        // Judge whether the current data in session is valid
        const string IsValid = "RFXPDecode_IsValid";

        public RFXPDecodeController(IWebHostEnvironment hostingEnvironment)
        : base(hostingEnvironment)
        {

        }

        public override ActionResult Index()
        {
            if (this.HttpContext.Session.Get<bool>(IsValid) == false)
            {
                // Clear Session data if the data is not valid
                this.HttpContext.Session.SetObject(ConstantDAS, null);
                this.HttpContext.Session.SetObject(ConstantCodecActionDic, null);
                this.HttpContext.Session.SetObject(DecodePreviousFrame, null);
                this.HttpContext.Session.SetObject(ConstantDASDic, null);
                this.HttpContext.Session.SetObject(PreviousFrameDic, null);
                
                var viewModel = new RFXPDecodeViewModel(0);
                this.HttpContext.Session.SetObject(ModelKey, viewModel);
                LoadFromSession();
                return View(_viewModel);
            }
            else
            {
                this.HttpContext.Session.SetObject(IsValid, false);

                Dictionary<int, ICodecAction> SessionCodecActionDic = this.HttpContext.Session.Get<Dictionary<int, ICodecAction>>(ConstantCodecActionDic);
                if (SessionCodecActionDic != null)
                {
                    // put the inputs in the viewbag
                    var inputList = new List<string[]>();
                    for (int i = 1; i < SessionCodecActionDic.Keys.Count; i++)
                    {
                        Triplet<string> encodedData = SessionCodecActionDic[i].SubActions.FirstOrDefault().Input[0].GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                        Triplet<string> rawData = SessionCodecActionDic[i].SubActions.FirstOrDefault().Input[1].GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                        var components = new List<string>
                        {
                            encodedData.X, rawData.X, encodedData.Y, rawData.Y, encodedData.Z, rawData.Z
                        };
                        inputList.Add(components.ToArray());
                    }
                    if (inputList.Count > 0) ViewBag.Inputs = inputList;
                }
                return View(_viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Decode()
        {
            try
            {
                using (var bodyStream = new StreamReader(Request.Body))
                {
                    var bodyText = await bodyStream.ReadToEndAsync();
                    var obj = JsonConvert.DeserializeObject(bodyText);
                    Decode(obj);
                }
                return Json(ReturnResult<string>.Success("Success"));
            }
            catch(Exception ex)
            {
                return Json(ReturnResult<string>.Fail(ex.Message));
            }
        }

        public override ActionResult LayerPanel([FromBody] LayerPanelRequest request)
        {
            var layersGroup = new List<PanelViewModel>();

            try
            {
                var CodecActionDic = this.HttpContext.Session.Get<Dictionary<int, ICodecAction>>(ConstantCodecActionDic);
                _codecAction = CodecActionDic == null ? null : (CodecActionDic.ContainsKey(request.layer) ? CodecActionDic[request.layer] : null);

                if (_codecAction == null) return PartialView("_Layers", layersGroup);

                if (request.name.Equals(Constants.DECODE_NAME_RECONSTRUCTEDFRAME))
                {
                    return DecodedImage(request.layer);
                }

                // gets the action with the same name as the argument
                var action = _codecAction.SubActions.SingleOrDefault(c => c.Name.Equals(request.name));
                if (action == null || action.Result == null) return PartialView("_Layers", layersGroup);

                // create layers accroding to the given layer.
                // This is different with others
                string idPrefix = request.name.Replace(' ', '-').Replace('/', '-') + "-input-layer-" + request.layer;
                var layerPanel = new PanelViewModel(idPrefix, "Layer " + request.layer);
                Tile result = action.Result.FirstOrDefault();
                Triplet<string> triplet = result.GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                var tabx = new TabViewModel { Id = idPrefix + "-Y", Title = "Y", Content = triplet.X, Editable = false };
                var taby = new TabViewModel { Id = idPrefix + "-Cb", Title = "Cb", Content = triplet.Y, Editable = false };
                var tabz = new TabViewModel { Id = idPrefix + "-Cr", Title = "Cr", Content = triplet.Z, Editable = false };
                layerPanel.Tabs = new List<TabViewModel> { tabx, taby, tabz };
                layersGroup.Add(layerPanel);
            }
            catch (Exception ex)
            {
                return Json(ReturnResult<string>.Fail(ex.Message));
            }

            return PartialView("_Layers", layersGroup);
        }

        public override ActionResult InputPanel([FromBody] LayerPanelRequest request)
        {
            // TODO: Extract Common with function Panel
            var layersGroup = new List<PanelViewModel>();

            try
            {
                var CodecActionDic = this.HttpContext.Session.Get<Dictionary<int, ICodecAction>>(ConstantCodecActionDic);
                _codecAction = CodecActionDic == null ? null : (CodecActionDic.ContainsKey(request.layer) ? CodecActionDic[request.layer] : null);

                if (_codecAction == null) return PartialView("_Layers", layersGroup);

                // gets the action with the same name as the argument
                var action = _codecAction.SubActions.SingleOrDefault(c => c.Name.Equals(request.name));
                if (action == null || action.Input == null) return PartialView("_Layers", layersGroup);

                // create layers accroding to the given layer.
                // This is different with others
                string idPrefix = request.name.Replace(' ', '-').Replace('/', '-') + "-output-layer-" + request.layer;
                var layerPanel = new PanelViewModel(idPrefix, "Layer " + request.layer);
                if (action.Input.Length > 1 && request.layer > 0)
                {
                    Tile input = action.Input[0];
                    Tile rawInput = action.Input[1];
                    Triplet<string> triplet = input.GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                    Triplet<string> rawTriplet = rawInput.GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                    var tabx = new TabViewModel { Id = idPrefix + "-Y", Title = "Y", Content = triplet.X, Editable = true };
                    var tabrawx = new TabViewModel { Id = idPrefix + "-YRaw", Title = "Y Raw Data", Content = rawTriplet.X, Editable = true };
                    var taby = new TabViewModel { Id = idPrefix + "-Cb", Title = "Cb", Content = triplet.Y, Editable = true };
                    var tabrawy = new TabViewModel { Id = idPrefix + "-CbRaw", Title = "Cb Raw Data", Content = rawTriplet.Y, Editable = true };
                    var tabz = new TabViewModel { Id = idPrefix + "-Cr", Title = "Cr", Content = triplet.Z, Editable = true };
                    var tabrawz = new TabViewModel { Id = idPrefix + "-CrRaw", Title = "Cr Raw Data", Content = rawTriplet.Z, Editable = true };

                    layerPanel.Tabs = new List<TabViewModel> { tabx, tabrawx, taby, tabrawy, tabz, tabrawz };
                    layersGroup.Add(layerPanel);
                }
                else
                {
                    Tile input = action.Input.FirstOrDefault();
                    Triplet<string> triplet = input.GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                    var tabx = new TabViewModel { Id = idPrefix + "-Y", Title = "Y", Content = triplet.X, Editable = true };
                    var taby = new TabViewModel { Id = idPrefix + "-Cb", Title = "Cb", Content = triplet.Y, Editable = true };
                    var tabz = new TabViewModel { Id = idPrefix + "-Cr", Title = "Cr", Content = triplet.Z, Editable = true };
                    layerPanel.Tabs = new List<TabViewModel> { tabx, taby, tabz };
                    layersGroup.Add(layerPanel);
                }
            }
            catch (Exception ex)
            {
                return Json(ReturnResult<string>.Fail(ex.Message));
            }

            return PartialView("_Layers", layersGroup);
        }

        public ActionResult CodecTab(int id = 1)
        {
            var viewModel = new RFXPDecodeViewModel(id);
            ViewBag.layer = id;
            return PartialView("_PDecodeTab", viewModel);
        }

        public ActionResult DecodedImage(int layer)
        {
            var CodecActionDic = this.HttpContext.Session.Get<Dictionary<int, ICodecAction>>(ConstantCodecActionDic);
            ICodecAction action = CodecActionDic == null ? null : (CodecActionDic.ContainsKey(layer) ? CodecActionDic[layer] : null);
            // TODO: Error Handler
            if (action == null) return null;

            string decodeImage = DateTime.Now.Ticks + ".bmp";
            ICodecAction restructedFrame = action.SubActions.LastOrDefault();
            Tile result = restructedFrame.Result.FirstOrDefault();
            Bitmap bitmap = result.GetBitmap();

            string decodedPath = Path.Combine(this._hostingEnvironment.WebRootPath, "Images/Decoded");
            if (!Directory.Exists(decodedPath))
            {
                Directory.CreateDirectory(decodedPath);
            }

            var path = Path.Combine(decodedPath, decodeImage);
            bitmap.Save(path, ImageFormat.Bmp);
            return PartialView("_Image", $"~/Images/Decoded/{decodeImage}");
        }

        [HttpPost]
        public async Task<IActionResult> IndexWithInputs()
        {
            try
            {
                using (var bodyStream = new StreamReader(Request.Body))
                {
                    var bodyText = await bodyStream.ReadToEndAsync();
                    dynamic obj = JsonConvert.DeserializeObject(bodyText);

                    foreach (var input in obj)
                    {
                        // TODO: refine this
                        if (input != null && input.Inputs != null)
                        {
                            int layer = JsonHelper.CastTo<int>(input.Layer);
                            if (layer == 0)
                            {
                                // TODO: refine this
                                // retrieve the quant
                                var quantArray = JsonHelper.RetrieveQuantsArray(input.Params.QuantizationFactorsArray);
                                // retrive the progressive quants
                                var progQuantList = new List<QuantizationFactorsArray>();
                                foreach (var layerQuant in input.Params.ProgQuantizationArray)
                                {
                                    var layerQuants = JsonHelper.RetrieveQuantsArray(layerQuant);
                                    progQuantList.Add(layerQuants);
                                }
                                var progQuantarray = new ProgressiveQuantizationFactors
                                {
                                    ProgQuants = progQuantList
                                };

                                EntropyAlgorithm algorithm = JsonHelper.CastTo<EntropyAlgorithm>(input.Params.EntropyAlgorithm);
                                UseDifferenceTile useDifferenceTile = JsonHelper.CastTo<UseDifferenceTile>(input.Params.UseDifferenceTile);
                                UseReduceExtrapolate useReduceExtrapolate = JsonHelper.CastTo<UseReduceExtrapolate>(input.Params.UseReduceExtrapolate);

                                _viewModel = new RFXPDecodeViewModel(0);
                                ((RFXPDecodeViewModel)_viewModel).ProvideParam(quantArray, progQuantarray, algorithm, useDifferenceTile, useReduceExtrapolate);

                                JArray jsonInputs = JArray.Parse(input["Inputs"].ToString());
                                ((RFXPDecodeViewModel)_viewModel).ProvidePanelInputs(layer, jsonInputs[0].TrimEnter(), jsonInputs[1].TrimEnter(), jsonInputs[2].TrimEnter());

                                this.HttpContext.Session.SetObject(ModelKey, _viewModel);
                                this.HttpContext.Session.SetObject(isPreFrameValid, true);
                            }
                            
                            Decode(input);
                            // Updates Decode Status
                            await UpdateDecodeStatus(layer);
                        }
                    }
                }

                this.HttpContext.Session.SetObject(IsValid, true);
                return Json(ReturnResult<string>.Success("Success"));
            }
            catch(Exception ex)
            {
                return Json(ReturnResult<string>.Fail(ex.Message));
            }
        }

        public async Task<IActionResult> UpdateDecodeStatus(int layer)
        {
            try
            {
                Dictionary<int, Tile> dasDic = this.HttpContext.Session.Get<Dictionary<int, Tile>>(ConstantDASDic);

                if (dasDic.ContainsKey(layer))
                {
                    this.HttpContext.Session.Get<Tile>(ConstantDAS).Add(dasDic[layer]);
                }

                Dictionary<int, Frame> preFrameDic = this.HttpContext.Session.Get<Dictionary<int, Frame>>(PreviousFrameDic);
                if (preFrameDic.ContainsKey(layer))
                {
                    this.HttpContext.Session.SetObject(DecodePreviousFrame, preFrameDic[layer]);
                }

                return Json(ReturnResult<string>.Success("Success"));
            }
            catch(Exception ex)
            {
                return Json(ReturnResult<string>.Fail(ex.Message));
            }
        }

        public override ActionResult Recompute([FromBody] RecomputeRequest request)
        {
            //dynamic obj = CodecBaseController.GetJsonObject(Request.InputStream);

            //int layer = JsonHelper.CastTo<int>(request.Layer);

            ICodecAction _rfxDecode = GetDecoderWithParameters(request);

            string name = JsonHelper.CastTo<string>(request.Action);

            // gets the action with the same name as the argument
            var action = _rfxDecode.SubActions.SingleOrDefault(c => c.Name.Equals(name));

            // if action not found
            if (action == null)
            {
                return Json(ReturnResult<string>.Fail("Action not found"));
            }

            // retrive tiles from Inputs
            var tileList = new List<Tile>();
            foreach (var tileJson in request.Inputs)
            {
                Triplet<string> triplet = JsonHelper.RetrieveTriplet(tileJson);

                string dataFormat = request.Params.UseDataFormat;
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

            // hand over parameters
            foreach (var key in _rfxDecode.Parameters.Keys)
            {
                action.Parameters[key] = _rfxDecode.Parameters[key];
            }

            var result = action.DoAction(tileList.ToArray());

            // recompute the following steps and update
            bool following = false;
            Tile[] input = result;
            foreach (var act in _rfxDecode.SubActions)
            {
                if (following)
                {
                    // hand over parameters
                    foreach (var key in _rfxDecode.Parameters.Keys)
                    {
                        act.Parameters[key] = _rfxDecode.Parameters[key];
                    }
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

            return Json(ReturnResult<string>.Success("Success"));
        }

        // TODO: it not correct to put it here.
        private void Decode(dynamic obj)
        {
            int layer = JsonHelper.CastTo<int>(obj.Layer);
            ICodecAction _rfxPDecode = GetDecoderWithParameters(obj);

            string decOrHex = (string)obj.Params.DecOrHex;
            JArray jsonInputs = JArray.Parse(obj["Inputs"].ToString());

            if (layer >= 1)
            {
                Tile tile = null;
                Tile tileRaw = null;

                if (decOrHex.Equals("hex"))
                {
                    tile = Tile.FromStrings(new Triplet<string>(
                        jsonInputs[0].TrimEnter(), jsonInputs[2].TrimEnter(), jsonInputs[4].TrimEnter()),
                        new HexTileSerializer());

                    tileRaw = Tile.FromStrings(new Triplet<string>(
                        jsonInputs[1].TrimEnter(), jsonInputs[3].TrimEnter(), jsonInputs[5].TrimEnter()),
                        new HexTileSerializer());
                }
                else
                {
                    tile = Tile.FromStrings(new Triplet<string>(
                        jsonInputs[0].TrimEnter(), jsonInputs[2].TrimEnter(), jsonInputs[4].TrimEnter()),
                        new IntegerTileSerializer());

                    tileRaw = Tile.FromStrings(new Triplet<string>(
                        jsonInputs[3].TrimEnter(), jsonInputs[3].TrimEnter(), jsonInputs[5].TrimEnter()),
                        new IntegerTileSerializer());
                }

                _rfxPDecode.DoAction(new[] { tile, tileRaw });
            }
            else
            {
                Tile tile = null;
                if (decOrHex.Equals("hex"))
                {
                    tile = Tile.FromStrings(new Triplet<string>(
                        jsonInputs[0].TrimEnter(), jsonInputs[1].TrimEnter(), jsonInputs[2].TrimEnter()),
                        new HexTileSerializer());
                }
                else
                {
                    tile = Tile.FromStrings(new Triplet<string>(
                        jsonInputs[0].TrimEnter(), jsonInputs[1].TrimEnter(), jsonInputs[2].TrimEnter()),
                        new IntegerTileSerializer());
                }

                _rfxPDecode.DoAction(new[] { tile });
            }

            // Update DAS
            ICodecAction rlgrSRLDecode = _rfxPDecode.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PDECODE_NAME_RLGRSRLDECODE));
            Dictionary<int, Tile> dasDic = this.HttpContext.Session.Get<Dictionary<int, Tile>>(ConstantDASDic);
            dasDic[layer] = rlgrSRLDecode.Result.FirstOrDefault();

            // Update PreFrame
            ICodecAction subbandDiffing = _rfxPDecode.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PDECODE_NAME_SUBBANDDIFFING));
            Dictionary<int, Frame> preFrameDic = this.HttpContext.Session.Get<Dictionary<int, Frame>>(PreviousFrameDic);
            preFrameDic[layer] = new Frame { Tile = subbandDiffing.Result.FirstOrDefault() };
        }

        private ICodecAction GetDecoderWithParameters(dynamic obj)
        {
            ICodecAction _rfxPDecode = new RFXPDecode();

            int layer = JsonHelper.CastTo<int>(obj.Layer);

            // retrieve the quant
            var quantArray = JsonHelper.RetrieveQuantsArray(obj.Params.QuantizationFactorsArray);
            _rfxPDecode.Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = quantArray;

            // retrive the progressive quants
            var progQuantList = new List<QuantizationFactorsArray>();
            foreach (var layerQuant in obj.Params.ProgQuantizationArray)
            {
                var layerQuants = JsonHelper.RetrieveQuantsArray(layerQuant);
                progQuantList.Add(layerQuants);
            }
            var progQuantarray = new ProgressiveQuantizationFactors
            {
                ProgQuants = progQuantList
            };
            _rfxPDecode.Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANT_LIST] = progQuantarray;

            _rfxPDecode.Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = JsonHelper.CastTo<EntropyAlgorithm>(obj.Params.EntropyAlgorithm);
            _rfxPDecode.Parameters[Constants.PARAM_NAME_USE_DIFFERENCE_TILE] = JsonHelper.CastTo<UseDifferenceTile>(obj.Params.UseDifferenceTile);
            _rfxPDecode.Parameters[Constants.PARAM_NAME_USE_REDUCE_EXTRAPOLATE] = JsonHelper.CastTo<UseReduceExtrapolate>(obj.Params.UseReduceExtrapolate);

            if (layer == 0)
            {
                // Session used to store DAS and previous codec
                Tile DAS = Tile.FromArrays<short>(new Triplet<short[]>(
                    new short[Tile.TileSize * Tile.TileSize],
                    new short[Tile.TileSize * Tile.TileSize],
                    new short[Tile.TileSize * Tile.TileSize])
                    );
                Dictionary<int, ICodecAction> CodecActionDic = new Dictionary<int, ICodecAction>();
                this.HttpContext.Session.SetObject(ConstantDAS, DAS);
                this.HttpContext.Session.SetObject(ConstantCodecActionDic, CodecActionDic);
                this.HttpContext.Session.SetObject(ConstantDASDic, new Dictionary<int, Tile>());
                
                // TODO: error handle
                var preFramePath = this.HttpContext.Session.Get<string>(PreviousFrameImage);
                Frame preFrame = Utility.GetPreviousFrame(preFramePath, _rfxPDecode.Parameters);
                this.HttpContext.Session.SetObject(DecodePreviousFrame, preFrame);
                this.HttpContext.Session.SetObject(PreviousFrameDic, new Dictionary<int, Frame>());
            }

            // TODO: deal with null
            // Add current codecAction in the session
            Dictionary<int, ICodecAction> SessionCodecActionDic = this.HttpContext.Session.Get<Dictionary<int, ICodecAction>>(ConstantCodecActionDic);
            SessionCodecActionDic[layer] = _rfxPDecode;

            var EncodeType = layer == 0 ? CodecToolSet.Core.EncodedTileType.EncodedType.FirstPass : CodecToolSet.Core.EncodedTileType.EncodedType.UpgradePass;
            ICodecAction rlgrSRLDecode = _rfxPDecode.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PDECODE_NAME_RLGRSRLDECODE));
            rlgrSRLDecode.Parameters[Constants.PARAM_NAME_ENCODED_TILE_TYPE] = new CodecToolSet.Core.EncodedTileType { Type = EncodeType };

            QuantizationFactorsArray progQuant = progQuantarray.ProgQuants[layer];
            ICodecAction progDeQuantization = _rfxPDecode.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PDECODE_NAME_PROGRESSIVEDEQUANTIZATION));
            progDeQuantization.Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANTS] = progQuant;

            ICodecAction subbandDiffing = _rfxPDecode.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PDECODE_NAME_SUBBANDDIFFING));
            Frame preframe = this.HttpContext.Session.Get<Frame>(DecodePreviousFrame);
            subbandDiffing.Parameters[Constants.PARAM_NAME_PREVIOUS_FRAME] = preframe;
            subbandDiffing.Parameters[Constants.PARAM_NAME_ENCODED_TILE_TYPE] = new CodecToolSet.Core.EncodedTileType { Type = EncodeType };

            ICodecAction DeQuantization = _rfxPDecode.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PDECODE_NAME_DEQUANTIZATION));

            // deal with some intermediate parameters
            if (layer >= 1)
            {
                rlgrSRLDecode.Parameters[Constants.PARAM_NAME_DAS] = new Frame { Tile = this.HttpContext.Session.Get<Tile>(ConstantDAS) };
                rlgrSRLDecode.Parameters[Constants.PARAM_NAME_PREVIOUS_PROGRESSIVE_QUANTS] = progQuantarray.ProgQuants[layer - 1];
                rlgrSRLDecode.Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANTS] = progQuantarray.ProgQuants[layer];
            }

            return _rfxPDecode;
        }
    }
}
