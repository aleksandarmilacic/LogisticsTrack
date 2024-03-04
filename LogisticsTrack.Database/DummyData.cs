using LogisticsTrack.Domain;
using Microsoft.EntityFrameworkCore;

namespace LogisticsTrack.Database
{
    public static class DummyData
    {
        public static void FillDatabase()
        {
            // here we would fill the database with some dummy data for all the entities
            var drivers = new Driver[]
            {
                new Driver { Name = "John Doe", Birthdate = new System.DateTime(1980, 1, 1) },
                new Driver { Name = "Jane Doe", Birthdate = new System.DateTime(1985, 1, 1) },
                new Driver { Name = "John Smith", Birthdate = new System.DateTime(1990, 1, 1) },
                new Driver { Name = "Jane Smith", Birthdate = new System.DateTime(1995, 1, 1) },
            };
            var trucks = new Truck[]
            {
                new Truck { LicensePlate = "ABC-123", GPSDeviceId = "GPS-123" },
                new Truck { LicensePlate = "DEF-456", GPSDeviceId = "GPS-456" },
                new Truck { LicensePlate = "GHI-789", GPSDeviceId = "GPS-789" },
                new Truck { LicensePlate = "JKL-012", GPSDeviceId = "GPS-012" },
            };
            var truckPlans = new TruckPlan[]
            {
                new TruckPlan { Driver = drivers[0], Truck = trucks[0], StartDate = new System.DateTime(2021, 1, 1) },
                new TruckPlan { Driver = drivers[1], Truck = trucks[1], StartDate = new System.DateTime(2021, 1, 1) },
                new TruckPlan { Driver = drivers[2], Truck = trucks[2], StartDate = new System.DateTime(2021, 1, 1) },
                new TruckPlan { Driver = drivers[3], Truck = trucks[3], StartDate = new System.DateTime(2021, 1, 1) },
            };
            var gpsRecords = new GPSRecord[]
            {
                new GPSRecord { Latitude = 1.0, Longitude = 1.0, Timestamp = new System.DateTime(2021, 1, 1), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 1.1, Longitude = 1.1, Timestamp = new System.DateTime(2021, 1, 1), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 1.2, Longitude = 1.2, Timestamp = new System.DateTime(2021, 1, 2), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 1.3, Longitude = 1.3, Timestamp = new System.DateTime(2021, 1, 3), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 1.4, Longitude = 1.4, Timestamp = new System.DateTime(2021, 1, 4), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 1.5, Longitude = 1.5, Timestamp = new System.DateTime(2021, 1, 5), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 1.6, Longitude = 1.6, Timestamp = new System.DateTime(2021, 1, 6), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 1.7, Longitude = 1.7, Timestamp = new System.DateTime(2021, 1, 7), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 1.8, Longitude = 1.8, Timestamp = new System.DateTime(2021, 1, 8), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 1.8, Longitude = 1.88, Timestamp = new System.DateTime(2021, 1, 8), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 1.9, Longitude = 1.9, Timestamp = new System.DateTime(2021, 1, 9), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 2.0, Longitude = 2.0, Timestamp = new System.DateTime(2021, 1, 10), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 2.1, Longitude = 2.1, Timestamp = new System.DateTime(2021, 1, 11), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 2.2, Longitude = 2.2, Timestamp = new System.DateTime(2021, 1, 12), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 2.3, Longitude = 2.3, Timestamp = new System.DateTime(2021, 1, 13), TruckPlan = truckPlans[0] },
                new GPSRecord { Latitude = 2.0, Longitude = 2.0, Timestamp = new System.DateTime(2021, 1, 2), TruckPlan = truckPlans[1] },
                new GPSRecord { Latitude = 3.0, Longitude = 3.0, Timestamp = new System.DateTime(2021, 1, 3), TruckPlan = truckPlans[2] },
                new GPSRecord { Latitude = 4.0, Longitude = 4.0, Timestamp = new System.DateTime(2021, 1, 4), TruckPlan = truckPlans[3] },

            };

            // now we instantiate the context and add the data
            using (var context = new LogisticsContext(new DbContextOptionsBuilder<LogisticsContext>()
                               .UseInMemoryDatabase("LogisticsTrack")
                                              .Options))
            {
                context.Drivers.AddRange(drivers);
                context.Trucks.AddRange(trucks);
                context.TruckPlans.AddRange(truckPlans);
                context.GPSRecords.AddRange(gpsRecords);
                context.SaveChanges();
            }
        }
    }
}
