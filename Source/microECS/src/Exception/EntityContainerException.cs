using System;
using System.Runtime.Serialization;

namespace microECS
{
    [Serializable]
    internal class EntityContainerException : Exception
    {
        public EntityContainerException()
        {
        }

        public EntityContainerException(string message) : base(message)
        {
        }

        public EntityContainerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityContainerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}