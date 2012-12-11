namespace VODB.Exceptions
{
    public class WhereExpressionFormatterNotFoundException : VodbException
    {
        public WhereExpressionFormatterNotFoundException()
            : base("Not found...")
        {
        }
    }
}