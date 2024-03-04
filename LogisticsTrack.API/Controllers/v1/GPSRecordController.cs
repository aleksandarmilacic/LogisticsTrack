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

        // GET: api/v1/TruckPlan/{guid}/GPSRecord
        [HttpGet]
        [Route("{id:guid}/GPSRecord")]
        [ProducesDefaultResponseType(typeof(PaginationList<IEnumerable<GPSRecordDTO>>))]
        public async Task<IActionResult> GetGPSRecords(Guid id, string column = null, int? skip = null, int? take = null, Ordering ordering = Ordering.desc) // we add the overloads so it is visible on swagger
        {
            _entityService.TruckPlanId = id;
            // pagination
            var config = GetPaginationParams();

            var result = await _entityService.GetAllAsync(config).ConfigureAwait(false);
            return Ok(result);
        }

        // GET: api/v1/TruckPlan/{guid}/GPSRecord/{guid}
        [HttpGet]
        [Route("{id:guid}/GPSRecord/{gpsRecordId:guid}")]
        [ProducesDefaultResponseType(typeof(GPSRecordDTO))]
        public async Task<IActionResult> GetGPSRecord(Guid id, Guid gpsRecordId)
        {
            _entityService.TruckPlanId = id;
            var result = await _entityService.GetAsync(id).ConfigureAwait(false);
            return Ok(result);
        }

        // PUT: api/TruckPlan/{guid}/GPSRecord/{guid}
        [HttpPut]
        [Route("{id:guid}/GPSRecord")]
        [ProducesDefaultResponseType(typeof(GPSRecordDTO))]
        public async Task<IActionResult> PutGPSRecord(Guid id, [FromBody] GPSRecordBMO model)
        {
            _entityService.TruckPlanId = id;
            var result = await _entityService.UpdateAsync(id, model).ConfigureAwait(false);
            return Ok(result);
        }

        // POST: api/TruckPlan/{guid}/GPSRecord
        [HttpPost]
        [Route("{id:guid}/GPSRecord")]
        [ProducesDefaultResponseType(typeof(GPSRecordDTO))]
        public async Task<IActionResult> PostGPSRecord(Guid id, [FromBody] GPSRecordBMO model)
        {
            _entityService.TruckPlanId = id;
            var result = await _entityService.CreateAsync(model).ConfigureAwait(false);
            return Ok(result);
        }

        // DELETE: api/TruckPlan/{guid}/GPSRecord/{guid}
        [HttpDelete]
        [Route("{id:guid}/GPSRecord/{gpsRecordId:guid}")]
        public async Task<IActionResult> DeleteGPSRecord(Guid id, Guid gpsRecordId)
        {
            _entityService.TruckPlanId = id;
            await _entityService.DeleteAsync(id).ConfigureAwait(false);
            return Ok();
        }
    }

}
