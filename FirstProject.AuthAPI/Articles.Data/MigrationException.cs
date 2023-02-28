using System;
using System.Runtime.Serialization;

namespace FirstProject.AuthAPI.Articles.Data
{
    [Serializable]
    internal class MigrationException : Exception
    {
        public MigrationException()
        {
        }

        public MigrationException(string message) : base(message)
        {
        }

        public MigrationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MigrationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}