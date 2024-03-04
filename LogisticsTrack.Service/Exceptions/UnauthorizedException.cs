namespace LogisticsTrack.Service.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
            : base("Unauthorized")
        {
        }

        public UnauthorizedException(string message)
            : base(message)
        {
        }

        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
