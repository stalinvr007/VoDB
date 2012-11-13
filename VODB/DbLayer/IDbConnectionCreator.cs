using System.Data.Common;

namespace VODB.DbLayer
{
    public interface IDbConnectionCreator
    {

        DbConnection Create();

    }
}
