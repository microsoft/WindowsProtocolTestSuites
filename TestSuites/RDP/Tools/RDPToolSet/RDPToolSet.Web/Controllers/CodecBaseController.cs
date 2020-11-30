// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CodecToolSet.Core;
using CodecToolSet.Core.RFXPEncode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using RDPToolSet.Web.Models;
using RDPToolSet.Web.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace RDPToolSet.Web.Controllers
{
    public abstract class CodecBaseController : Controller
    {
        protected ICodecViewModel _viewModel;
        protected ICodecAction _codecAction;
        protected const string EncodedImage = "EncodedImage";
        protected const string PreviousFrameImage = "PreviousFrameImage";
        protected const string isPreFrameValid = "isPreFrameValid";
        protected const string ActionKey = "_codecAction";
        protected const string ModelKey = "_viewModel";
        protected const string isActionValid = "isActionValid";
        protected const string isModelValid = "isModelValid";
        protected readonly IWebHostEnvironment _hostingEnvironment;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            LoadFromSession();
            SetViewBag();
        }

        public CodecBaseController(IWebHostEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        public virtual ActionResult Index()
        {
            return View(_viewModel);
        }

        public virtual ActionResult LayerPanel([FromBody] LayerPanelRequest request)
        {
            if (request.name.Equals(Constants.DECODE_NAME_RECONSTRUCTEDFRAME))
            {
                return DecodedImage();
            }

            var layers = new List<PanelViewModel>();

            try
            {
                var action = _codecAction.SubActions.FirstOrDefault(c => c.Name.Equals(request.name));
                if (action == null || action.Result == null) return PartialView("_Layers", layers);

                for (int index = 0; index < action.Result.Length; index++)
                {
                    string idPrefix = request.name.Replace(' ', '-') + "-layer-" + index;
                    var layer = new PanelViewModel(idPrefix, "Layer " + index);
                    Tile result = action.Result[index];
                    Triplet<string> triplet = result.GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                    var tabx = new TabViewModel { Id = idPrefix + "-Y", Title = "Y", Content = triplet.X, Editable = false };
                    var taby = new TabViewModel { Id = idPrefix + "-Cb", Title = "Cb", Content = triplet.Y, Editable = false };
                    var tabz = new TabViewModel { Id = idPrefix + "-Cr", Title = "Cr", Content = triplet.Z, Editable = false };
                    layer.Tabs = new List<TabViewModel> { tabx, taby, tabz };
                    layers.Add(layer);
                }
            }
            catch (Exception ex)
            {
                return Json(ReturnResult<string>.Fail(ex.Message));
            }

            return PartialView("_Layers", layers);
        }

        public virtual ActionResult InputPanel([FromBody] LayerPanelRequest request)
        {
            var layers = new List<PanelViewModel>();

            try
            {
                // gets the action with the same name as the argument
                var action = _codecAction.SubActions.FirstOrDefault(c => c.Name.Equals(request.name));
                if (action == null || action.Input == null) return PartialView("_Layers", layers);

                for (int index = 0; index < action.Input.Length; index++)
                {
                    string idPrefix = request.name.Replace(' ', '-').Replace('/', '-') + "-input-layer-" + index;
                    var layer = new PanelViewModel(idPrefix, "Layer " + index);
                    Tile result = action.Input[index];
                    Triplet<string> triplet = result.GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                    var tabx = new TabViewModel { Id = idPrefix + "-Y", Title = "Y", Content = triplet.X, Editable = true };
                    var taby = new TabViewModel { Id = idPrefix + "-Cb", Title = "Cb", Content = triplet.Y, Editable = true };
                    var tabz = new TabViewModel { Id = idPrefix + "-Cr", Title = "Cr", Content = triplet.Z, Editable = true };
                    layer.Tabs = new List<TabViewModel> { tabx, taby, tabz };
                    layers.Add(layer);
                }
            }
            catch (Exception ex)
            {
                return Json(ReturnResult<string>.Fail(ex.Message));
            }

            return PartialView("_Layers", layers);
        }

        [HttpPost]
        public ActionResult Upload(IFormFile file, string imageType)
        {
            if (this.Request.Form.Files.Count > 1)
            {
                return Json(ReturnResult<string>.Fail("Please upload 1 image each time."));
            }
            // file type
            if (file != null && file.Length > 0)
            {
                string[] filenames = file.FileName.Split(new[] { '.' });
                string filename = String.Format("{0}_{1}.{2}", filenames[0], DateTime.Now.Ticks, filenames.Length >= 2 ? filenames[1] : "");

                string uploadPath = Path.Combine(this._hostingEnvironment.WebRootPath, ActionHelper.ImageUploadFolder);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var path = Path.Combine(uploadPath, filename);

                using (var fileStream = file.OpenReadStream())
                {
                    using (var newfileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.CopyTo(newfileStream);
                    }
                }

                if (imageType.Equals(EncodedImage))
                {
                    this.HttpContext.Session.SetObject(EncodedImage, path);
                }
                else if (imageType.Equals(PreviousFrameImage))
                {
                    this.HttpContext.Session.SetObject(PreviousFrameImage, path);
                }
            }
            return Json(ReturnResult<string>.Success("Success"));
        }

        [HttpGet]
        public ActionResult RemoveUploadedImage(string imageType)
        {
            if (imageType.Equals(EncodedImage))
            {
                this.HttpContext.Session.Set(EncodedImage, new byte[0]);
            }
            if (imageType.Equals(PreviousFrameImage))
            {
                this.HttpContext.Session.Set(PreviousFrameImage, new byte[0]);
            }

            return Json(ReturnResult<string>.Success("Success"));
        }

        [HttpGet]
        public ActionResult SetSamples(string sample)
        {
            string samplePath = Path.Combine(this._hostingEnvironment.WebRootPath, "html");
            var path = Path.Combine(samplePath, sample);
            this.HttpContext.Session.SetObject(EncodedImage, path);

            return Json(ReturnResult<string>.Success("Success"));
        }

        public ActionResult PreviousFrame()
        {
            if (this.HttpContext.Session.Get<bool>(isPreFrameValid) == false || string.IsNullOrEmpty(this.HttpContext.Session.Get<string>(PreviousFrameImage)))
            {
                return Json(string.Empty);
            }
            else
            {
                this.HttpContext.Session.SetObject(isPreFrameValid, false);

                string filename = Path.GetFileName(this.HttpContext.Session.Get<string>(PreviousFrameImage));
                string relativePath = $"~/{ActionHelper.ImageUploadFolder}/" + filename;
                return Json(Url.Content(relativePath));
            }
        }

        public virtual ActionResult DecodedImage()
        {
            string decodeImage = DateTime.Now.Ticks + ".bmp";
            ICodecAction restructedFrame = _codecAction.SubActions.LastOrDefault();
            if (restructedFrame.Result == null) return PartialView("_Image", null);
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

        public virtual ActionResult Recompute([FromBody] RecomputeRequest request)
        {
            // gets the action with the same name as the argument
            var action = _codecAction.SubActions.FirstOrDefault(c => c.Name.Equals(request.Action));

            // if action not found
            if (action == null)
            {
                return Json(ReturnResult<string>.Fail("Action not found"));
            }

            // retrieve the quant
            var quantArray = JsonHelper.RetrieveQuantsArray(request.Params.QuantizationFactorsArray);

            foreach (var _action in _codecAction.SubActions)
            {
                _action.Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = quantArray;
                _action.Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = JsonHelper.CastTo<EntropyAlgorithm>(request.Params.EntropyAlgorithm);
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

            var result = action.DoAction(tileList.FirstOrDefault());

            // recompute the following steps and update
            bool following = false;
            Tile input = result;
            foreach (var act in _codecAction.SubActions)
            {
                if (following)
                {
                    result = act.DoAction(input);
                    input = result;
                }
                else
                {
                    if (act.Name.Equals(request.Action))
                    {
                        following = true;
                    }
                }
            }

            return Json(ReturnResult<string>.Success("Success"));
        }

        public virtual ActionResult Input()
        {
            try
            {
                using (var bodyStream = new StreamReader(Request.Body))
                {
                    var bodyText = bodyStream.ReadToEndAsync().GetAwaiter().GetResult();
                    dynamic json = JsonConvert.DeserializeObject(bodyText);
                    var action = CodecFactory.GetCodecAction((string)json.Action);
                    // if action not found
                    if (action == null)
                    {
                        return Json(ReturnResult<string>.Fail("Action not found"));
                    }

                    //TODO: add parameters
                    var tile = Tile.FromStrings(new Triplet<string>(
                    json.Inputs.X, json.Inputs.Y, json.Inputs.Z),
                    new IntegerTileSerializer());

                    var result = action.DoAction(tile);
                    return Json(result.GetStrings(TileSerializerFactory.GetDefaultSerizlizer()));
                }
            }
            catch(Exception ex)
            {
                return Json(ReturnResult<string>.Fail(ex.Message));
            }
        }

        public ActionResult DefaultProgQuants()
        {
            QuantizationFactorsArray quantArray = new QuantizationFactorsArray();
            quantArray.quants = new QuantizationFactors[3] { RFXPEncode.ZERO_QUANT, RFXPEncode.ZERO_QUANT, RFXPEncode.ZERO_QUANT };
            ViewBag.isProg = true;
            return PartialView("_Quants", new[] { quantArray });
        }

        protected void SaveToSession(Dictionary<string, object> envValues)
        {
            foreach (var key in envValues.Keys)
            {
                HttpContext.Session.SetObject(key, envValues[key]);
            }
        }

        protected void LoadFromSession()
        {
            _codecAction = HttpContext.Session.Get<ICodecAction>(ActionKey);
            
            _viewModel = HttpContext.Session.Get<ICodecViewModel>(ModelKey);
        }

        private void SetViewBag()
        {
            var rfxMenu = new Menu("RemoteFX", "Encode", "RFXEncode", "Decode", "RFXDecode");
            var rfxpMenu = new Menu("RemoteFXProg", "Encode", "RFXPEncode", "Decode", "RFXPDecode");
            var layoutModel = new LayoutViewModel(
                "RDP Codec Tool",
                "Index",
                "CodecHome",
                 "RDP Codec Tool",
                 "Microsoft Protocol Test Suite Development Team",
                 "2.0",
                 rfxMenu,
                 rfxpMenu
                 );
            ViewBag.model = layoutModel;
        }
    }

    internal static class JsonHelper
    {
        public static T CastTo<T>(this object obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        public static QuantizationFactorsArray RetrieveQuantsArray(dynamic obj)
        {
            var quantList = new List<QuantizationFactors>();
            foreach (var componet in obj)
            {
                var quant = JsonHelper.CastTo<QuantizationFactors>(componet);
                quantList.Add(quant);
            }
            var quantArray = new QuantizationFactorsArray
            {
                quants = quantList.ToArray()
            };
            return quantArray;
        }

        public static Triplet<string> RetrieveTriplet(dynamic obj)
        {
            var componentList = new List<String>();
            foreach (var component in obj)
            {
                componentList.Add((string)component);
            }
            return new Triplet<string>(componentList[0], componentList[1], componentList[2]);
        }
    }
}
