using System;

namespace VODB.Exceptions
{
    public class EntityKeysNotFoundException : VodbException
    {
        public EntityKeysNotFoundException(string tableName)
            : base(String.Format("No keys found for the model that represents [{0}] table.", tableName))
        {
        }
    }
}