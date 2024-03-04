using AutoMapper;
using LogisticsTrack.Database.Repository;
using LogisticsTrack.Domain;
using LogisticsTrack.Domain.BMOModels;
using LogisticsTrack.Domain.DTOModels;

namespace LogisticsTrack.Service.Services
{
    public class TruckPlanService : BaseService<TruckPlan, TruckPlanDTO, TruckPlanDTO, TruckPlanBMO>
    {
        public TruckPlanService(Repository<TruckPlan> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
