using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodecToolSet.Core;
using CodecToolSet.Core.RFXDecode;
using RDPToolSet.Web.Models;



namespace RDPToolSet.Web.Controllers
{
    public class RFXDecodeController : CodecBaseController
    {
        public override ActionResult Index()
        {
            var envValues = new Dictionary<string, object>();
            if (Session == null || Session[isActionValid] == null || ((bool)Session[isActionValid]) == false)
            {
                var rfxDecode = new RFXDecode();
                envValues[ActionKey] = rfxDecode;
            }
            else
            {
                Session[isActionValid] = false;
            }
            if (Session == null || Session[isModelValid] == null || ((bool)Session[isModelValid]) == false)
            {
                var rfxDeocdeModel = new RFXDecodeViewModel();
                envValues[ModelKey] = rfxDeocdeModel;
            }
            else
            {
                Session[isModelValid] = false;
            }

            SaveToSession(envValues);
            LoadFromSession();
            return View(_viewModel);
        }

        public ActionResult Decode()
        {
            dynamic obj = GetJsonObject(Request.InputStream);

            // retrieve the quant
            var quantArray = JsonHelper.RetrieveQuantsArray(obj.Params.QuantizationFactorsArray);
            _codecAction.Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = quantArray;
            _codecAction.Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = JsonHelper.CastTo<EntropyAlgorithm>(obj.Params.EntropyAlgorithm);

            string decOrHex = (string)obj.Params.DecOrHex;

            Tile tile = null;
            if (decOrHex.Equals("hex"))
            {
                tile = Tile.FromStrings(new Triplet<string>(
                obj.Inputs.Y, obj.Inputs.Cb, obj.Inputs.Cr),
                new HexTileSerializer());
            }else{
                tile = Tile.FromStrings(new Triplet<string>(
                obj.Inputs.Y, obj.Inputs.Cb, obj.Inputs.Cr),
                new IntegerTileSerializer());
            }
            _codecAction.DoAction(tile);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        /// <summary>
        /// return an index page according to the inputs
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexWithInputs()
        {
            dynamic obj = GetJsonObject(Request.InputStream);

            // retrieve the quant
            QuantizationFactorsArray quantArray = JsonHelper.RetrieveQuantsArray(obj.Params.QuantizationFactorsArray);
            EntropyAlgorithm algorithm = JsonHelper.CastTo<EntropyAlgorithm>(obj.Params.EntropyAlgorithm);


            _viewModel = new RFXDecodeViewModel();
            // Updates parameters
            ((RFXDecodeViewModel)_viewModel).ProvideParam(quantArray, algorithm);
            Triplet<string> triplet = JsonHelper.RetrieveTriplet(obj.Inputs);
            ((RFXDecodeViewModel)_viewModel).ProvidePanelInputs(triplet.ToArray());

            var envValues = new Dictionary<string, object>()
            {
                {ModelKey, _viewModel},
                {isModelValid, true}
            };

            SaveToSession(envValues);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
