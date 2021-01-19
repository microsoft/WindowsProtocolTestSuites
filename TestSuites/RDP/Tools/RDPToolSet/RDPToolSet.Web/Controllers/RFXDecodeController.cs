// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodecToolSet.Core;
using CodecToolSet.Core.RFXDecode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RDPToolSet.Web.Models;
using RDPToolSet.Web.Utils;

namespace RDPToolSet.Web.Controllers
{
    public class RFXDecodeController : CodecBaseController
    {
        public RFXDecodeController(IWebHostEnvironment hostingEnvironment)
        : base(hostingEnvironment)
        {

        }

        public override ActionResult Index()
        {
            var envValues = new Dictionary<string, object>();
            if (this.HttpContext.Session.Get<bool>(isActionValid) == false)
            {
                var rfxDecode = new RFXDecode();
                envValues[ActionKey] = rfxDecode;
            }
            else
            {
                this.HttpContext.Session.SetObject(isActionValid, false);
            }

            if (this.HttpContext.Session.Get<bool>(isModelValid) == false)
            {
                var rfxDeocdeModel = new RFXDecodeViewModel();
                envValues[ModelKey] = rfxDeocdeModel;
            }
            else
            {
                this.HttpContext.Session.SetObject(isModelValid, false);
            }

            SaveToSession(envValues);
            LoadFromSession();
            return View(_viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Decode()
        {
            try
            {
                using (var bodyStream = new StreamReader(Request.Body))
                {
                    var bodyText = await bodyStream.ReadToEndAsync();
                    dynamic obj = (JObject)JsonConvert.DeserializeObject(bodyText);

                    // retrieve the quant
                    var quantArray = JsonHelper.RetrieveQuantsArray(obj.Params.QuantizationFactorsArray);
                    _codecAction.Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = quantArray;
                    _codecAction.Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = JsonHelper.CastTo<EntropyAlgorithm>(obj.Params.EntropyAlgorithm);

                    string decOrHex = (string)obj.Params.DecOrHex;

                    Tile tile = null;
                    JObject jsonInputs = JObject.Parse(obj["Inputs"].ToString());
                    dynamic objInputs = JsonConvert.DeserializeObject(jsonInputs.ToString());
                    string y = jsonInputs["Y"].ToString();
                    string cb = jsonInputs["Cb"].ToString();
                    string cr = jsonInputs["Cr"].ToString();

                    if (decOrHex.Equals("hex"))
                    {
                        tile = Tile.FromStrings(new Triplet<string>(y, cb, cr),
                        new HexTileSerializer());
                    }
                    else
                    {
                        tile = Tile.FromStrings(new Triplet<string>(y, cb, cr),
                        new IntegerTileSerializer());
                    }
                    _codecAction.DoAction(tile);

                    return Json(ReturnResult<string>.Success("Success"));
                }
            }
            catch (Exception ex)
            {
                return Json(ReturnResult<string>.Fail(ex.Message));
            }
        }

        /// <summary>
        /// return an index page according to the inputs
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> IndexWithInputs()
        {
            try
            {
                using (var bodyStream = new StreamReader(Request.Body))
                {
                    var bodyText = await bodyStream.ReadToEndAsync();
                    dynamic obj = JsonConvert.DeserializeObject(bodyText);

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
                }
                return Json(ReturnResult<string>.Success("Success"));
            }
            catch (Exception ex)
            {
                return Json(ReturnResult<string>.Fail(ex.Message));
            }
        }
    }
}
