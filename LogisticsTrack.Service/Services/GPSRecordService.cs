using AutoMapper;
using LogisticsTrack.Database.Repository;
using LogisticsTrack.Domain;
using LogisticsTrack.Domain.BMOModels;
using LogisticsTrack.Domain.DTOModels;
using LogisticsTrack.Service.Exceptions;

namespace LogisticsTrack.Service.Services
{
    public class GPSRecordService : BaseService<GPSRecord, GPSRecordDTO, GPSRecordDTO, GPSRecordBMO>
    {
        public Guid? TruckPlanId { get; set; }

        public GPSRecordService(Repository<GPSRecord> repository, IMapper mapper) : base(repository, mapper)
        {

        }

        protected override IQueryable<GPSRecord> GetAllEntitiesBase()
        {
            if(this.TruckPlanId == null)
                throw new NotFoundException("TruckPlanId is not set");

            return base.GetAllEntitiesBase().Where(a => a.TruckPlandId == this.TruckPlanId);
        }
    }
}
