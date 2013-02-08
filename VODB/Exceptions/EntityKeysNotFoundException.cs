namespace VODB.Exceptions
{
    public class EntityKeysNotFoundException : VodbException
    {
        public EntityKeysNotFoundException(string tableName)
            : base("No keys found for the model that represents [{0}] table.", tableName)
        {
        }
    }
}