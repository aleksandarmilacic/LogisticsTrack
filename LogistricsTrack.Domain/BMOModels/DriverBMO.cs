using System.ComponentModel.DataAnnotations;

namespace LogisticsTrack.Domain.BMOModels
{
    public class DriverBMO : ModelBaseBMO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }
    }
}
