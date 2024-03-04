using AutoMapper;
using LogisticsTrack.Domain;
using LogisticsTrack.Domain.BMOModels;
using LogisticsTrack.Domain.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LogisticsTrack.Service.Mapper
{
    public class MappingProfile : Profile
    {
        // lets map the Business Model Object to the entities and the entities to the Data Transfer Object
        public MappingProfile()
        {
            CreateMap<DriverBMO, Driver>();
            CreateMap<Driver, DriverDTO>();

            CreateMap<GPSRecordBMO, GPSRecord>();
            CreateMap<GPSRecord, GPSRecordDTO>();

            CreateMap<TruckBMO, Truck>();
            CreateMap<Truck, TruckDTO>();

            CreateMap<TruckPlanBMO, TruckPlan>();
            CreateMap<TruckPlan, TruckPlanDTO>();
        }
    }

    
}
