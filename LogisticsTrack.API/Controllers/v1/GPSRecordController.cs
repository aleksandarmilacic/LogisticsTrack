using LogisticsTrack.Domain;
using LogisticsTrack.Domain.BMOModels;
using LogisticsTrack.Domain.DTOModels;
using LogisticsTrack.Domain.Enums;
using LogisticsTrack.Domain.HelperModels; 
using LogisticsTrack.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsTrack.API.Controllers.v1
{
 
    [Route("api/v1/TruckPlan")]
    public class GPSRecordController : BaseController<GPSRecordDTO, GPSRecordDTO, GPSRecordBMO>
    {
        private readonly GPSRecordService _entityService;

        public GPSRecordController(GPSRecordService entityService) : base(entityService)
        {
            _entityService = entityService;
        }

        // GET: api/v1/TruckPlan/{guid}/Distance
        [HttpGet]
        [Route("{truckPlanId:guid}/Distance")]
        [ProducesDefaultResponseType(typeof(double))]
        public async Task<IActionResult> GetTruckPlanDistance(Guid truckPlanId)
        {
            _entityService.TruckPlanId = truckPlanId;
            var distance = await _entityService.CalculateDistanceDrivenAsync();
            return Ok(distance);
        }

        // GET: api/v1/TruckPlan/{guid}/GPSRecord
        [HttpGet]
        [Route("{truckPlanId:guid}/GPSRecord")]
        [ProducesDefaultResponseType(typeof(PaginationList<IEnumerable<GPSRecordDTO>>))]
        public async Task<IActionResult> GetGPSRecords(Guid truckPlanId, string column = null, int? skip = null, int? take = null, Ordering ordering = Ordering.desc) // we add the overloads so it is visible on swagger
        {
            _entityService.TruckPlanId = truckPlanId;
            // pagination
            var config = GetPaginationParams();

            var result = await _entityService.GetAllAsync(config).ConfigureAwait(false);
            return Ok(result);
        }

        // GET: api/v1/TruckPlan/{guid}/GPSRecord/{guid}
        [HttpGet]
        [Route("{truckPlanId:guid}/GPSRecord/{gpsRecordId:guid}")]
        [ProducesDefaultResponseType(typeof(GPSRecordDTO))]
        public async Task<IActionResult> GetGPSRecord(Guid truckPlanId, Guid gpsRecordId)
        {
            _entityService.TruckPlanId = truckPlanId;
            var result = await _entityService.GetAsync(gpsRecordId).ConfigureAwait(false);
            return Ok(result);
        }

        // PUT: api/TruckPlan/{guid}/GPSRecord/{guid}
        [HttpPut]
        [Route("{truckPlanId:guid}/GPSRecord")]
        [ProducesDefaultResponseType(typeof(GPSRecordDTO))]
        public async Task<IActionResult> PutGPSRecord(Guid truckPlanId, [FromBody] GPSRecordBMO model)
        {
            _entityService.TruckPlanId = truckPlanId;
            var result = await _entityService.UpdateAsync(model.Id, model).ConfigureAwait(false);
            return Ok(result);
        }

        // POST: api/TruckPlan/{guid}/GPSRecord
        [HttpPost]
        [Route("{truckPlanId:guid}/GPSRecord")]
        [ProducesDefaultResponseType(typeof(GPSRecordDTO))]
        public async Task<IActionResult> PostGPSRecord(Guid truckPlanId, [FromBody] GPSRecordBMO model)
        {
            _entityService.TruckPlanId = truckPlanId;
            var result = await _entityService.CreateAsync(model).ConfigureAwait(false);
            return Ok(result);
        }

        // DELETE: api/TruckPlan/{guid}/GPSRecord/{guid}
        [HttpDelete]
        [Route("{truckPlanId:guid}/GPSRecord/{gpsRecordId:guid}")]
        public async Task<IActionResult> DeleteGPSRecord(Guid truckPlanId, Guid gpsRecordId)
        {
            _entityService.TruckPlanId = truckPlanId;
            await _entityService.DeleteAsync(gpsRecordId).ConfigureAwait(false);
            return Ok();
        }
    }

}
