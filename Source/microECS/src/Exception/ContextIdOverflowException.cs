using System;
using System.Runtime.Serialization;

namespace microECS
{
    [Serializable]
    internal class ContextIdOverflowException : Exception
    {
        public ContextIdOverflowException()
        {
        }

        public ContextIdOverflowException(string message) : base(message)
        {
        }

        public ContextIdOverflowException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ContextIdOverflowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}