using LogisticsTrack.Domain.BMOModels;
using LogisticsTrack.Domain.DTOModels;
using LogisticsTrack.Domain.Enums;
using LogisticsTrack.Domain.HelperModels;
using LogisticsTrack.Service;
using LogisticsTrack.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsTrack.API.Controllers.v1
{
    [Route("api/v1/Truck")]
    public class TruckController : BaseController<TruckDTO, TruckDTO, TruckBMO>
    {
        public TruckController(TruckService entityService) : base(entityService)
        {
        }

        // GET: api/v1/Truck
        [HttpGet]
        [Route("")]
        [ProducesDefaultResponseType(typeof(PaginationList<IEnumerable<TruckDTO>>))]
        public async Task<IActionResult> GetTrucks(string column = null, int? skip = null, int? take = null, Ordering ordering = Ordering.desc) // we add the overloads so it is visible on swagger
        {
            return await GetAllAsync().ConfigureAwait(false);
        }

        // GET: api/v1/Truck/{guid}
        [HttpGet]
        [Route("{id:guid}")]
        [ProducesDefaultResponseType(typeof(TruckDTO))]
        public async Task<IActionResult> GetTruck(Guid id)
        {
            return await GetAsync(id).ConfigureAwait(false);
        }

        // PUT: api/Truck/{guid}
        [HttpPut]
        [Route("")]
        [ProducesDefaultResponseType(typeof(TruckDTO))]
        public async Task<IActionResult> PutTruck([FromBody] TruckBMO model)
        {
            return await UpdateAsync(model.Id, model).ConfigureAwait(false); 
        }

        // POST: api/Truck
        [HttpPost]
        [Route("")]
        [ProducesDefaultResponseType(typeof(TruckDTO))]
        public async Task<IActionResult> PostTruck([FromBody] TruckBMO model)
        {
            return await CreateAsync(model).ConfigureAwait(false);
        }

        // DELETE: api/Truck/{guid}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteTruck(Guid id)
        {
            return await DeleteAsync(id).ConfigureAwait(false);
        }
    }

}
