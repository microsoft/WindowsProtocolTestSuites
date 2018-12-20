using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CodecToolSet.Core;
using CodecToolSet.Core.RFXEncode;
using RDPToolSet.Web.Models;

namespace RDPToolSet.Web.Controllers
{
   public class RFXEncodeController : CodecBaseController
   {

       public override ActionResult Index()
       {
           var rfxEncode = new RFXEncode();
           var rfxEnocdeModel = new RFXEncodeViewModel();
           var envValues = new Dictionary<string, object>
           {
               {ActionKey, rfxEncode},
               {ModelKey, rfxEnocdeModel}
           };
           SaveToSession(envValues);
           LoadFromSession();
           return View(_viewModel);
       }

        public ActionResult Encode()
        {
            dynamic obj = GetJsonObject(Request.InputStream);

            // retrieve the quant
            var quantArray = JsonHelper.RetrieveQuantsArray(obj.Params.QuantizationFactorsArray);
            _codecAction.Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = quantArray;

            _codecAction.Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = JsonHelper.CastTo<EntropyAlgorithm>(obj.Params.EntropyAlgorithm);



            // TODO: error handle
            var encodeImagePath = (string)Session[EncodedImage];
            var tile = Tile.FromFile(encodeImagePath);

            _codecAction.DoAction(tile);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
   }
}
