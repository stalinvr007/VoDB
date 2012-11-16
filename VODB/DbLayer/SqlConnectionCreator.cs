namespace VODB.DbLayer
{
    public sealed class SqlConnectionCreator : DbConnectionCreator
    {
        public SqlConnectionCreator()
            : base("System.Data.SqlClient", null)
        {
        }
    }
}