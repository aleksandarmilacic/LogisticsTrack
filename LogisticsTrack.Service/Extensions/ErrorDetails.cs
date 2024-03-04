namespace LogisticsTrack.Service.Extensions
{
    public class ErrorDetails
    {
        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}