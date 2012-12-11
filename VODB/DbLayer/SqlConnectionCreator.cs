namespace VODB.DbLayer
{
    public sealed class SqlConnectionCreator : NameConventionDbConnectionCreator
    {
        public SqlConnectionCreator()
            : base("System.Data.SqlClient")
        {
        }
    }
}