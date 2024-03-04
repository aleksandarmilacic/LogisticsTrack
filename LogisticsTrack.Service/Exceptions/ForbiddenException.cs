namespace LogisticsTrack.Service.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class ForbiddenException : Exception
    {
        public ForbiddenException()
            : base("Forbidden")
        {
        }

        public ForbiddenException(string message)
            : base(message)
        {
        }

        public ForbiddenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ForbiddenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
