namespace LogisticsTrack.Service.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base("Object not found")
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
