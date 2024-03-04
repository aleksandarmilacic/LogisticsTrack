using AutoMapper;
using LogisticsTrack.Database.Repository;
using LogisticsTrack.Domain;
using LogisticsTrack.Domain.BMOModels;
using LogisticsTrack.Domain.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsTrack.Service.Services
{
    public class DriversService : BaseService<Driver, DriverDTO, DriverDTO, DriverBMO>
    { 

        public DriversService(Repository<Driver> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
