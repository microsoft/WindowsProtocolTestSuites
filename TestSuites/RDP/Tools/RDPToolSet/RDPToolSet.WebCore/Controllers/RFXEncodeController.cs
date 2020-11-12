using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodecToolSet.Core;
using CodecToolSet.Core.RFXEncode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RDPToolSet.WebCore.Models;
using RDPToolSet.WebCore.Utils;

namespace RDPToolSet.WebCore.Controllers
{
    public class RFXEncodeController : CodecBaseController
    {
        public RFXEncodeController(IWebHostEnvironment hostingEnvironment)
            : base(hostingEnvironment)
        {
            
        }

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

        public async Task<IActionResult> Encode()
        {
            try
            {
                using (var bodyStream = new StreamReader(Request.Body))
                {
                    var bodyText = await bodyStream.ReadToEndAsync();
                    dynamic obj = JsonConvert.DeserializeObject(bodyText);
                    var quantArray = JsonHelper.RetrieveQuantsArray(obj.Params.QuantizationFactorsArray);
                    _codecAction.Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = quantArray;
                    _codecAction.Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = JsonHelper.CastTo<EntropyAlgorithm>(obj.Params.EntropyAlgorithm);

                    // TODO: error handle
                    var encodeImagePath = this.HttpContext.Session.Get<string>(EncodedImage);
                    var tile = Tile.FromFile(encodeImagePath);

                    _codecAction.DoAction(tile);
                    //HttpContext.Session.SetObject(ActionKey, _codecAction);
                    return Json(ReturnResult<string>.Success("Success"));
                }
            }catch(Exception ex)
            {
                return Json(ReturnResult<string>.Fail(ex.Message));
            }
        }
    }
}
