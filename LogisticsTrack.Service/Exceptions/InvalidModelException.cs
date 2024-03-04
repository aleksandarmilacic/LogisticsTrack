using LogisticsTrack.Domain.HelperModels;
using System.Runtime.Serialization;

namespace LogisticsTrack.Service.Exceptions
{
    public class InvalidModelException : Exception
    {
        public InvalidModelException()
        {
        }

        public InvalidModelException(IEnumerable<ValidationError> errors = null)
            : base("Invalid data")
        {
            Errors = errors ?? Enumerable.Empty<ValidationError>();
        }

        public InvalidModelException(string message)
            : base(message)
        {
        }

        public InvalidModelException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidModelException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}
