using LogisticsTrack.Domain.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace LogisticsTrack.Domain
{
    public class Truck : SoftDeletableEntity
    {
        [Required]
        public string LicensePlate { get; set; }

        [Required]
        public string GPSDeviceId { get; set; }
    }

}
