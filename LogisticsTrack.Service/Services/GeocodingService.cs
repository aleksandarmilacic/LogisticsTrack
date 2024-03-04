using LogisticsTrack.Domain;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace LogisticsTrack.Service.Services
{
 
    public class GeocodingService : GroundBaseService
    {
        private readonly HttpClient _httpClient;

        public GeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // just one of many ways how to reverse geocode coordinates to a country
        public async Task<string?> GetCountryFromCoordinates(double latitude, double longitude)
        {
            string requestUri = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitude}&lon={longitude}";

            var response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var result = await JsonSerializer.DeserializeAsync<GeocodeResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);

                return result?.Address?.Country;
            }

            return null;
        }

        // this is my off the top of my head solution to the problem of checking if a trip is relevant to a country
        // I would need to know more about the business logic to provide a more accurate solution
        // i think it is a bad approach to overload the api with requests for every single point in the trip
        public async Task<bool> IsTripRelevantByCountry(IEnumerable<GPSRecord> gpsRecords, string targetCountry)
        {
            if (!gpsRecords.Any())
                return false;

            int sampleRate = CalculateSampleRate(gpsRecords);

            var sampledPoints = gpsRecords
                .Where((x, index) => index % sampleRate == 0);

            foreach (var point in sampledPoints)
            {
                var country = await GetCountryFromCoordinates(point.Latitude, point.Longitude);
                if (country == targetCountry)
                    return true;
            }

            return false;
        }

        private int CalculateSampleRate(IEnumerable<GPSRecord> gpsRecords)
        {
            // Simple example: sample one point for every hour of the trip
            // Adjust the logic based on trip duration, distance, or other factors
            var durationHours = (gpsRecords.Last().Timestamp - gpsRecords.First().Timestamp).TotalHours;
            var rate = Math.Max(1, (int)durationHours); // Ensure at least one sample, adjust as needed
            return rate;

            // Another example: sample one point for every 100 km of the trip
            // or Border proximity analysis, or country inclusion logic
        } 

        private class GeocodeResponse
        {
            public GeocodeAddress Address { get; set; }
        }

        private class GeocodeAddress
        {
            public string Country { get; set; }
        }
    }
}
