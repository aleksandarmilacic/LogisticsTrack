namespace LogisticsTrack.Domain.DTOModels
{
    public class TruckPlanDTO : ModelBaseDTO
    {

        public Guid DriverId { get; set; }
        public DriverDTO Driver { get; set; }

        public Guid TruckId { get; set; }

        public TruckDTO Truck { get; set; }


        public IEnumerable<GPSRecord> GPSRecords { get; set; } = new List<GPSRecord>();
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
