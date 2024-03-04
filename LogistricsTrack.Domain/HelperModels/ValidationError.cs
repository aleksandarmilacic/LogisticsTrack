namespace LogisticsTrack.Domain.HelperModels
{
    public class ValidationError
    {
        public string ErrorMessage { get; set; }

        public List<string> PropertyNames { get; }
    }
}
