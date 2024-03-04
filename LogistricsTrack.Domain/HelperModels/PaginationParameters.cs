using LogisticsTrack.Domain.Enums;

namespace LogisticsTrack.Domain.HelperModels
{
    public class PaginationParameters
    {
        private const int DEFAULT_TAKE_VALUE = 15;

        // default values for skip and take

        private int _skip = 0;
        private int _take = DEFAULT_TAKE_VALUE;

        public string Column { get; set; }
        public int Skip { get => _skip; set => _skip = value < 0 ? 0 : value; } // cannot be less than 0
        public int Take { get => _take; set => _take = value < 1 ? DEFAULT_TAKE_VALUE : value; } // cannot be less than 1, so put the default value
        public Ordering Ordering { get; set; } = Ordering.desc;

        public QueryModel[] Query { get; set; } = null;
    }
}
