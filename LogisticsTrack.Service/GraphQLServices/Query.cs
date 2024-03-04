using GraphQL;
using GraphQL.Types;
using HotChocolate;
using HotChocolate.Types;
using LogisticsTrack.Database.Repository;
using LogisticsTrack.Domain;
using LogisticsTrack.Service.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsTrack.Service.GraphQLServices
{


    public class DriverCriteriaInputType : InputObjectType<DriverCriteria>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DriverCriteria> descriptor)
        {
            descriptor.Name("DriverCriteriaInput");

            descriptor.Field(t => t.MinAge).Type<IntType>()
                .Description("The minimum age of the driver.");

            descriptor.Field(t => t.MaxAge).Type<IntType>()
                .Description("The maximum age of the driver.");

            descriptor.Field(t => t.Country).Type<StringType>()
                .Description("The country the driver drove in.");
        }
    }


    public class TripQueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field("tripsByCriteria")
                .Type<NonNullType<FloatType>>() // Adjust the return type as needed
                .Argument("criteria", a => a.Type<NonNullType<DriverCriteriaInputType>>())
                .ResolveWith<TripAnalysisServiceResolver>(resolver => resolver.GetDriversByCriteriaAsync(default!, default!)) // Use ResolveWith for async and DI
                .Name("GetDriversByCriteria");
        }

        private class TripAnalysisServiceResolver
        {
            public async Task<double> GetDriversByCriteriaAsync([Service] ITripAnalysisService service, DriverCriteria criteria)
            {
                return await service.GetDriversByCriteriaAsync(criteria);
            }
        }
    }



    public class DriverDistanceResponseType : ObjectGraphType
    {
        public DriverDistanceResponseType()
        {
            Field<NonNullGraphType<FloatGraphType>>("totalKilometers", resolve: context => context.Source);
        }
    }

    public interface ITripAnalysisService
    { 
        Task<double> GetDriversByCriteriaAsync(DriverCriteria criteria);
    }

    public class TripAnalysisService : ITripAnalysisService
    {
        private readonly Repository<GPSRecord> _gpsRepo;
        private readonly GPSRecordService _recordService;

        public TripAnalysisService(Repository<GPSRecord> gpsRepo, GPSRecordService recordService)
        {
            _gpsRepo = gpsRepo;
            _recordService = recordService;
        }
    
 

        public async Task<double> GetDriversByCriteriaAsync(DriverCriteria criteria)
        {
            var currentDate = DateTime.Today; 

            // Calculate the oldest possible birthdate to be of minAge
            var minBirthDate = criteria.MinAge.HasValue
                ? currentDate.AddYears(-criteria.MinAge.Value)
                : DateTime.MinValue; // If no minAge is specified, set to DateTime.MinValue to include all possible dates

            // Calculate the youngest possible birthdate to still be of maxAge
            var maxBirthDate = criteria.MaxAge.HasValue
                ? currentDate.AddYears(-criteria.MaxAge.Value - 1).AddDays(1)
                : DateTime.MaxValue; // If no maxAge is specified, set to DateTime.MaxValue to include all possible dates

            var drivers = await _gpsRepo.Context.GPSRecords
                .Include(tp => tp.TruckPlan.Driver)
                .Where(tp => tp.Country.ToLower() == criteria.Country.ToLower() && tp.TruckPlan.Driver.Birthdate <= minBirthDate && tp.TruckPlan.Driver.Birthdate >= maxBirthDate)
                .ToListAsync().ConfigureAwait(false); 
            return await _recordService.CalculateDistanceDrivenAsync(drivers).ConfigureAwait(false);
        }

    }
}
