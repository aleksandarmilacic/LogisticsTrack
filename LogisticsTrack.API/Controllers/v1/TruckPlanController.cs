using LogisticsTrack.Domain.BMOModels;
using LogisticsTrack.Domain.DTOModels;
using LogisticsTrack.Domain.Enums;
using LogisticsTrack.Domain.HelperModels;
using LogisticsTrack.Service;
using LogisticsTrack.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsTrack.API.Controllers.v1
{
    [Route("api/v1/TruckPlan")]
    public class TruckPlanController : BaseController<TruckPlanDTO, TruckPlanDTO, TruckPlanBMO>
    {
        public TruckPlanController(TruckPlanService entityService) : base(entityService)
        {
        }

        // GET: api/v1/TruckPlan
        [HttpGet]
        [Route("")]
        [ProducesDefaultResponseType(typeof(PaginationList<IEnumerable<TruckPlanDTO>>))]
        public async Task<IActionResult> GetTruckPlans(string column = null, int? skip = null, int? take = null, Ordering ordering = Ordering.desc) // we add the overloads so it is visible on swagger
        {
            return await GetAllAsync().ConfigureAwait(false);
        }

        // GET: api/v1/TruckPlan/{guid}
        [HttpGet]
        [Route("{id:guid}")]
        [ProducesDefaultResponseType(typeof(TruckPlanDTO))]
        public async Task<IActionResult> GetTruckPlan(Guid id)
        {
            return await GetAsync(id).ConfigureAwait(false);
        }

        // PUT: api/TruckPlan/{guid}
        [HttpPut]
        [Route("")]
        [ProducesDefaultResponseType(typeof(TruckPlanDTO))]
        public async Task<IActionResult> PutTruckPlan([FromBody] TruckPlanBMO model)
        {
            return await UpdateAsync(model.Id, model).ConfigureAwait(false); 
        }

        // POST: api/TruckPlan
        [HttpPost]
        [Route("")]
        [ProducesDefaultResponseType(typeof(TruckPlanDTO))]
        public async Task<IActionResult> PostTruckPlan([FromBody] TruckPlanBMO model)
        {
            return await CreateAsync(model).ConfigureAwait(false);
        }

        // DELETE: api/TruckPlan/{guid}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteTruckPlan(Guid id)
        {
            return await DeleteAsync(id).ConfigureAwait(false);
        }
    }

}
