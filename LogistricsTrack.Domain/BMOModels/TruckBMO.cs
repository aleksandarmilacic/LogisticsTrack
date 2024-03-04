

using System.ComponentModel.DataAnnotations;

namespace LogisticsTrack.Domain.BMOModels
{
    public class TruckBMO : ModelBaseBMO
    {
        [Required]
        public string LicensePlate { get; set; }
        [Required]
        public string GPSDeviceId { get; set; }
    }
}
