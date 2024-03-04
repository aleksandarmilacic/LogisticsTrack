using LogisticsTrack.Domain.BaseEntities;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsTrack.Domain
{
    public class TruckPlan : SoftDeletableEntity
    {

        public Guid DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public Driver Driver { get; set; }

        public Guid TruckId { get; set; }

        [ForeignKey(nameof(TruckId))]
        public Truck Truck { get; set; }


        public virtual ICollection<GPSRecord> GPSRecords { get; set; } = new HashSet<GPSRecord>();

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }

}
