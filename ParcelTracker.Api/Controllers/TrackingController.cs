using Microsoft.AspNetCore.Mvc;
using ParcelTracker.Api.Models;
using ParcelTracker.Api.Services;

namespace ParcelTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackingController : ControllerBase
    {
        private readonly GoogleSheetsService _googleSheetsService;

        public TrackingController(GoogleSheetsService googleSheetsService)
        {
            _googleSheetsService = googleSheetsService;
        }

        [HttpGet("{trackingNumber}")]
        public ActionResult<TrackingInfo> Get(string trackingNumber)
        {
            var info = _googleSheetsService.GetTrackingInfo(trackingNumber);
            if(info == null)
            {
                return NotFound("Tracking number");
            }
            return Ok(info);
        }
    }
}
