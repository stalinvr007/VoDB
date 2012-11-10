using System.Data.Common;

namespace VODB.DbLayer
{
    internal interface IDbConnectionCreator
    {

        DbConnection Create();

    }
}
