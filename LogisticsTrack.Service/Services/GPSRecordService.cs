using AutoMapper;
using GraphQL;
using GraphQL.Types;
using LogisticsTrack.Database.Repository;
using LogisticsTrack.Domain;
using LogisticsTrack.Domain.BMOModels;
using LogisticsTrack.Domain.DTOModels;
using LogisticsTrack.Service.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LogisticsTrack.Service.Services
{
    
    public class GPSRecordService : BaseService<GPSRecord, GPSRecordDTO, GPSRecordDTO, GPSRecordBMO>
    {
        private readonly GeocodingService _geocodingService;

        public Guid? TruckPlanId { get; set; }

        public GPSRecordService(Repository<GPSRecord> repository, IMapper mapper, GeocodingService geocodingService) : base(repository, mapper)
        {
            _geocodingService = geocodingService;
        }

        protected override IQueryable<GPSRecord> GetAllEntitiesBase()
        {
            if(this.TruckPlanId == null)
                throw new NotFoundException("TruckPlanId is not set");

            return base.GetAllEntitiesBase().Where(a => a.TruckPlandId == this.TruckPlanId);
        }

        protected override async Task UpdateEntity(GPSRecord entity, GPSRecordBMO updateModel, bool created = false)
        {
            var address = await _geocodingService.GetCountryFromCoordinates(updateModel.Latitude, updateModel.Longitude).ConfigureAwait(false);
            entity.Country = address?.ToLowerInvariant(); // best solution is to lookup the country prior to saving and save that data

            await base.UpdateEntity(entity, updateModel, created).ConfigureAwait(false);
        }

        public async Task<double> CalculateDistanceDrivenAsync(List<GPSRecord> records = null)
        {
            var gpsRecords = records ?? await GetAllEntities().OrderBy(r => r.Timestamp)
                .ToListAsync().ConfigureAwait(false);

            double totalDistance = 0.0;
            GPSRecord previousRecord = null;

            foreach (var record in gpsRecords)
            {
                if (previousRecord != null)
                {
                    totalDistance += CalculateDistanceKm(
                        previousRecord.Latitude, previousRecord.Longitude,
                        record.Latitude, record.Longitude);
                }
                previousRecord = record;
            }

            return totalDistance;
        }

        private double CalculateDistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            // Implement Haversine formula source: https://en.wikipedia.org/wiki/Haversine_formula
            // potential improvement: use a library like GeoCoordinate to calculate the distance
            // use google maps api to calculate the distance
            // or other algorithms like Vincenty formula: https://en.wikipedia.org/wiki/Vincenty%27s_formulae
            

            // here im following the haversine formula proof from the Wikipedia page
            const double R = 6371e3; // Earth's radius in meters
            var φ1 = lat1 * Math.PI / 180; // φ, λ in radians
            var φ2 = lat2 * Math.PI / 180;
            var Δφ = (lat2 - lat1) * Math.PI / 180;
            var Δλ = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = R * c; // in meters

            return distance / 1000; // Convert to kilometers 
        }
    }
}
