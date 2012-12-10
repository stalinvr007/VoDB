namespace VODB.Exceptions
{
    public class SessionNotFoundException : VodbException
    {
        public SessionNotFoundException(string tableName)
            : base("The session object was not set for a tuple of {0}.", tableName)
        {

        }
    }
}
