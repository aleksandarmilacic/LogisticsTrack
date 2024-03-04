using LogisticsTrack.Domain.BMOModels;
using LogisticsTrack.Domain.DTOModels;
using LogisticsTrack.Domain.Enums;
using LogisticsTrack.Domain.HelperModels;
using LogisticsTrack.Service;
using LogisticsTrack.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsTrack.API.Controllers.v1
{
    [Route("api/v1/Driver")]
    public class DriverController : BaseController<DriverDTO, DriverDTO, DriverBMO>
    {
        public DriverController(DriversService entityService) : base(entityService)
        {
        }

        // GET: api/v1/Driver
        [HttpGet]
        [Route("")]
        [ProducesDefaultResponseType(typeof(PaginationList<IEnumerable<DriverDTO>>))]
        public async Task<IActionResult> GetDrivers(string column = null, int? skip = null, int? take = null, Ordering ordering = Ordering.desc) // we add the overloads so it is visible on swagger
        {
            return await GetAllAsync().ConfigureAwait(false);
        }

        // GET: api/v1/Driver/{guid}
        [HttpGet]
        [Route("{id:guid}")]
        [ProducesDefaultResponseType(typeof(DriverDTO))]
        public async Task<IActionResult> GetDriver(Guid id)
        {
            return await GetAsync(id).ConfigureAwait(false);
        }

        // PUT: api/Driver/{guid}
        [HttpPut]
        [Route("")]
        [ProducesDefaultResponseType(typeof(DriverDTO))]
        public async Task<IActionResult> PutDriver([FromBody] DriverBMO model)
        {
            return await UpdateAsync(model.Id, model).ConfigureAwait(false); 
        }

        // POST: api/Driver
        [HttpPost]
        [Route("")]
        [ProducesDefaultResponseType(typeof(DriverDTO))]
        public async Task<IActionResult> PostDriver([FromBody] DriverBMO model)
        {
            return await CreateAsync(model).ConfigureAwait(false);
        }

        // DELETE: api/Driver/{guid}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteDriver(Guid id)
        {
            return await DeleteAsync(id).ConfigureAwait(false);
        }


    }

}
