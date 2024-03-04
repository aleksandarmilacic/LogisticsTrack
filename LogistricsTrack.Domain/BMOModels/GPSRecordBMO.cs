using System.ComponentModel.DataAnnotations;

namespace LogisticsTrack.Domain.BMOModels
{
    public class GPSRecordBMO : ModelBaseBMO
    {
        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }

    }
}
