using GRBusiness.LPNItem;
using MasterDataBusiness.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using putawayBusiness.ViewModels;
using PutawayBusiness.LPN;
using putawayDataBusiness.ViewModels;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PutawayAPI.Controllers
{
    [Route("api/Putaway")]
    public class PutawayController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;

        public PutawayController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("scanLpn")]
        public IActionResult scanLpn([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                var result = service.scanLpn(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("scanPutaway")]
        public IActionResult scanLpnscanPutaway([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                var result = service.scanLpnscanPutaway(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("scanSKU")]
        public IActionResult scanSKU([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new BinBalanceViewModel();
                Models = JsonConvert.DeserializeObject<BinBalanceViewModel>(body.ToString());
                var result = service.scanSKU(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("suggestion")]
        public IActionResult suggestion([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                var result = service.suggestion(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("confirmLocation")]
        public IActionResult confirmLocation([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new LocationConfigViewModel();
                Models = JsonConvert.DeserializeObject<LocationConfigViewModel>(body.ToString());
                var result = service.confirmLocation(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("SavePutaway")]
        public IActionResult SavePutaway([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                var result = service.SavePutaway(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("SavePutawayAuto")]
        public IActionResult SavePutawayAuto([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();

                var result = service.SavePutawayAuto();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region filterTaskunPack
        [HttpPost("filterTaskunPack")]
        public IActionResult filterTaskunPack([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new View_TaskGRViewModel();
                Models = JsonConvert.DeserializeObject<View_TaskGRViewModel>(body.ToString());
                var result = service.filterTaskunPack(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region filterTask
        [HttpPost("filterTask")]
        public IActionResult filterTask([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new View_TaskGRViewModel();
                Models = JsonConvert.DeserializeObject<View_TaskGRViewModel>(body.ToString());
                var result = service.filterTask(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region filterTask
        [HttpPost("filterTaskPallet")]
        public IActionResult filterTaskPallet([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new View_TaskGRViewModel();
                Models = JsonConvert.DeserializeObject<View_TaskGRViewModel>(body.ToString());
                var result = service.filterTaskPallet(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        [HttpPost("ReportPutawayGR")]
        public IActionResult ReportPutawayGR([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new PutawayService();
                var Models = new ReportPutawayGRViewModel();
                Models = JsonConvert.DeserializeObject<ReportPutawayGRViewModel>(body.ToString());
                localFilePath = service.ReportPutawayGR(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }
        #region AutobasicSuggestion
        [HttpPost("autobasicSuggestion")]
        public IActionResult autobasicSuggestion([FromBody]JObject body)

        {
            try
            {
                var service = new PutawayService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autobasicSuggestion(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region checkUserassign
        [HttpPost("checkUserassign")]
        public IActionResult checkUserassign([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new View_TaskGRViewModel();
                Models = JsonConvert.DeserializeObject<View_TaskGRViewModel>(body.ToString());
                var result = service.checkUserassign(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region deleteUserassign
        [HttpPost("deleteUserassign")]
        public IActionResult deleteUserassign([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new View_TaskGRViewModel();
                Models = JsonConvert.DeserializeObject<View_TaskGRViewModel>(body.ToString());
                var result = service.deleteUserassign(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region scanLpnDockToStaging
        [HttpPost("scanLpnDockToStaging")]
        public IActionResult scanLpnDockToStaging([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                var result = service.scanLpnDockToStaging(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region chkLocation
        [HttpPost("chkLocation")]
        public IActionResult chkLocation([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new LocationConfigViewModel();
                Models = JsonConvert.DeserializeObject<LocationConfigViewModel>(body.ToString());
                var result = service.chkLocation(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region chkLocationDockToStaging
        [HttpPost("chkLocationDockToStaging")]
        public IActionResult chkLocationDockToStaging([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new LocationConfigViewModel();
                Models = JsonConvert.DeserializeObject<LocationConfigViewModel>(body.ToString());
                var result = service.chkLocationDockToStaging(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        [HttpPost("chkTagItem")]
        public IActionResult chkTagItem([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                var result = service.chkTagItem(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region SaveDockToStaging
        [HttpPost("SaveDockToStaging")]
        public IActionResult SaveDockToStaging([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                var result = service.SaveDockToStaging(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region SavetypeselectivePallet
        [HttpPost("SavetypeselectivePallet")]
        public IActionResult SavetypeselectivePallet([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                var result = service.SavetypeselectivePallet(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region filterTaskDockToStaging
        [HttpPost("filterTaskDockToStaging")]
        public IActionResult filterTaskDockToStaging([FromBody]JObject body)
        {
            try
            {
                var service = new PutawayService();
                var Models = new View_TaskGRViewModel();
                Models = JsonConvert.DeserializeObject<View_TaskGRViewModel>(body.ToString());
                var result = service.filterTaskDockToStaging(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

    }
}

