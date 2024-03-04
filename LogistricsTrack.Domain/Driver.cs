using LogisticsTrack.Domain.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace LogisticsTrack.Domain
{
    public class Driver : SoftDeletableEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }
    }

}
