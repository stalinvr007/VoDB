namespace VODB.Exceptions
{
    public class WhereExpressionHandlerNotFoundException : VodbException
    {
        public WhereExpressionHandlerNotFoundException()
            : base("Not found...")
        {
        }
    }
}