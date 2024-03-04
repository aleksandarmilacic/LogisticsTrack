using System.ComponentModel.DataAnnotations;

namespace LogisticsTrack.Domain.HelperModels
{
    public class QueryModel
    {
        [Required]
        public string Property { get; set; }

        public string Value { get; set; }
    }
}
