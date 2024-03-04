namespace LogisticsTrack.Domain.DTOModels
{
    public class GPSRecordDTO : ModelBaseDTO
    {
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
