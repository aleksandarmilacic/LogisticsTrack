using LogisticsTrack.Domain.BaseEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsTrack.Domain
{
    public class GPSRecord : SoftDeletableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TruckPlandId { get; set; }

        [ForeignKey(nameof(TruckPlandId))]
        public TruckPlan TruckPlan { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }

        public string? Country { get; set; }
    }

}
