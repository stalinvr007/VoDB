namespace VODB.DbLayer
{
    internal sealed class SqlConnectionCreator : DbConnectionCreator
    {
        public SqlConnectionCreator()
            : base("System.Data.SqlClient", null)
        {
        }
    }
}