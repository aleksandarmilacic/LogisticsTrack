using AutoMapper;
using LogisticsTrack.Database.Repository;
using LogisticsTrack.Domain;
using LogisticsTrack.Domain.BMOModels;
using LogisticsTrack.Domain.DTOModels;

namespace LogisticsTrack.Service.Services
{
    public class TruckService : BaseService<Truck, TruckDTO, TruckDTO, TruckBMO>
    {
        public TruckService(Repository<Truck> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
