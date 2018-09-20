using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using CodecToolSet.Core;
using RDPToolSet.Web.Models;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using CodecToolSet.Core.RFXPEncode;

namespace RDPToolSet.Web.Controllers
{
    [HandleError]
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

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            LoadFromSession();
            SetViewBag();
        }

        public virtual ActionResult Index()
        {
            return View(_viewModel);
        }

        public virtual ActionResult LayerPanel()
        {
            dynamic obj = CodecBaseController.GetJsonObject(Request.InputStream);

            string name = JsonHelper.CastTo<string>(obj.name);

            if (name.Equals(Constants.DECODE_NAME_RECONSTRUCTEDFRAME))
            {
                return DecodedImage();
            }

            var layers = new List<PanelViewModel>();

            // gets the action with the same name as the argument
            var action = _codecAction.SubActions.SingleOrDefault(c => c.Name.Equals(name));
            if (action == null || action.Result == null) return PartialView("_Layers", layers);

            for (int index = 0; index < action.Result.Length; index++)
            {
                string idPrefix = name.Replace(' ', '-') + "-layer-" + index;
                var layer = new PanelViewModel(idPrefix, "Layer " + index);
                Tile result = action.Result[index];
                Triplet<string> triplet = result.GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                var tabx = new TabViewModel { Id = idPrefix + "-Y", Title = "Y", Content = triplet.X, Editable = false };
                var taby = new TabViewModel { Id = idPrefix + "-Cb", Title = "Cb", Content = triplet.Y, Editable = false };
                var tabz = new TabViewModel { Id = idPrefix + "-Cr", Title = "Cr", Content = triplet.Z, Editable = false };
                layer.Tabs = new List<TabViewModel> { tabx, taby, tabz };
                layers.Add(layer);
            }

            return PartialView("_Layers", layers);
        }

        public virtual ActionResult InputPanel()
        {
            // TODO: Merge this with above

            dynamic obj = CodecBaseController.GetJsonObject(Request.InputStream);

            string name = JsonHelper.CastTo<string>(obj.name);

            var layers = new List<PanelViewModel>();

            // gets the action with the same name as the argument
            var action = _codecAction.SubActions.SingleOrDefault(c => c.Name.Equals(name));
            if (action == null || action.Input == null) return PartialView("_Layers", layers);

            for (int index = 0; index < action.Input.Length; index++)
            {
                string idPrefix = name.Replace(' ', '-').Replace('/', '-') + "-input-layer-" + index;
                var layer = new PanelViewModel(idPrefix, "Layer " + index);
                Tile result = action.Input[index];
                Triplet<string> triplet = result.GetStrings(TileSerializerFactory.GetDefaultSerizlizer());
                var tabx = new TabViewModel { Id = idPrefix + "-Y", Title = "Y", Content = triplet.X, Editable = true };
                var taby = new TabViewModel { Id = idPrefix + "-Cb", Title = "Cb", Content = triplet.Y, Editable = true };
                var tabz = new TabViewModel { Id = idPrefix + "-Cr", Title = "Cr", Content = triplet.Z, Editable = true };
                layer.Tabs = new List<TabViewModel> { tabx, taby, tabz };
                layers.Add(layer);
            }

            return PartialView("_Layers", layers);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, string imageType)
        {
            // file type
            if (file != null && file.ContentLength > 0)
            {
                string[] filenames = file.FileName.Split(new[] { '.' });
                string filename = String.Format("{0}_{1}.{2}", filenames[0], DateTime.Now.Ticks, filenames.Length >= 2 ? filenames[1] : "");

                string uploadPath = Server.MapPath("~/Images/Uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var path = Path.Combine(uploadPath, filename);
                file.SaveAs(path);
                if (imageType.Equals(EncodedImage))
                {
                    Session[EncodedImage] = path;
                }
                else if (imageType.Equals(PreviousFrameImage))
                {
                    Session[PreviousFrameImage] = path;
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult RemoveUploadedImage(string imageType)
        {
            if (imageType.Equals(EncodedImage))
            {
                Session[EncodedImage] = null;
            }
            if (imageType.Equals(PreviousFrameImage))
            {
                Session[PreviousFrameImage] = null;
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        [HttpGet]
        public ActionResult SetSamples(string sample)
        {
            string samplePath = Server.MapPath("~/Static/");
            var path = Path.Combine(samplePath, sample);
            Session[EncodedImage] = path;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult PreviousFrame()
        {
            if (Session == null || Session[isPreFrameValid] == null || (bool)Session[isPreFrameValid] == false || Session[PreviousFrameImage] == null)
            {
                return Json(string.Empty);
            }
            else
            {
                Session[isPreFrameValid] = false;
                string filename = Path.GetFileName((string)Session[PreviousFrameImage]);
                string relativePath = "~/Images/Uploads/" + filename;
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

            string decodedPath = Server.MapPath("~/Images/Decoded");
            if (!Directory.Exists(decodedPath))
            {
                Directory.CreateDirectory(decodedPath);
            }

            var path = "~/Images/Decoded/" + decodeImage;
            var physicalPath = Server.MapPath(path);
            bitmap.Save(physicalPath, ImageFormat.Bmp);
            return PartialView("_Image", path);
        }

        public virtual ActionResult Recompute()
        {
            dynamic obj = CodecBaseController.GetJsonObject(Request.InputStream);

            string name = JsonHelper.CastTo<string>(obj.Action);

            // gets the action with the same name as the argument
            var action = _codecAction.SubActions.SingleOrDefault(c => c.Name.Equals(name));

            // if action not found
            if (action == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            // retrieve the quant
            var quantArray = JsonHelper.RetrieveQuantsArray(obj.Params.QuantizationFactorsArray);

            foreach (var _action in _codecAction.SubActions)
            {
                _action.Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = quantArray;
                _action.Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = JsonHelper.CastTo<EntropyAlgorithm>(obj.Params.EntropyAlgorithm);
            }

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
                    if (act.Name.Equals(name))
                    {
                        following = true;
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public virtual ActionResult Input()
        {
            dynamic json = GetJsonObject(Request.InputStream);

            var action = CodecFactory.GetCodecAction((string)json.Action);

            // if action not found
            if (action == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            //TODO: add parameters

            var tile = Tile.FromStrings(new Triplet<string>(
                json.Inputs.X, json.Inputs.Y, json.Inputs.Z),
                new IntegerTileSerializer());

            var result = action.DoAction(tile);

            return Json(result.GetStrings(TileSerializerFactory.GetDefaultSerizlizer()));
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
                Session[key] = envValues[key];
            }
        }

        protected void LoadFromSession()
        {
            if (Session != null)
            {
                if (Session[ActionKey] != null)
                {
                    _codecAction = (ICodecAction)Session[ActionKey];
                }
                if (Session[ModelKey] != null)
                {
                    _viewModel = (ICodecViewModel)Session[ModelKey];
                }
            }
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

        internal static dynamic GetJsonObject(Stream jsonRequest)
        {
            string json;
            using (var reader = new StreamReader(jsonRequest))
                json = reader.ReadToEnd();
            dynamic obj = System.Web.Helpers.Json.Decode(json);
            return obj;
        }
    }

    internal static class JsonHelper
    {
        public static T CastTo<T>(this object obj)
        {
            return Json.Decode<T>(Json.Encode(obj));
        }

        public static QuantizationFactorsArray RetrieveQuantsArray(DynamicJsonArray obj)
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

        public static Triplet<string> RetrieveTriplet(DynamicJsonArray obj)
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