using System.ComponentModel.DataAnnotations;

namespace LogisticsTrack.Domain.BMOModels
{
    public class TruckPlanBMO : ModelBaseBMO
    {
        [Required]
        public Guid DriverId { get; set; }
        [Required]
        public Guid TruckId { get; set; }

        [Required]
        public DateTime StartDate { get; set; } 
        public DateTime? EndDate { get; set; }

    }
}
