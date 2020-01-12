using System;
using System.Runtime.Serialization;
using ArangoDBNetStandard.Models;

namespace ArangoDBNetStandard
{
    [Serializable]
    public class ApiErrorException : Exception
    {
        public ApiResponse ResponseDetails { get; set; }

        public ApiErrorException()
        {
        }

        public ApiErrorException(ApiResponse error) : base(error.ErrorMessage)
        {
            ResponseDetails = error;
        }

        public ApiErrorException(string message) : base(message)
        {
        }

        public ApiErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApiErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}