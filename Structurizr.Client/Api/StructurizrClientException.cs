using System;

namespace Structurizr.Api
{
    public class StructurizrClientException : Exception
    {
        public StructurizrClientException(string message) : base(message)
        {
        }

        public StructurizrClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}