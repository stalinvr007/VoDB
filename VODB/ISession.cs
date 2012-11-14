using System.Collections.Generic;
using System.Data.Common;
using VODB.Sessions;

namespace VODB
{
    public interface ISession
    {
        ITransaction BeginTransaction();

        IEnumerable<TEntity> GetAll<TEntity>()
            where TEntity : DbEntity, new();
    }

    internal interface IInternalSession
    {
        DbCommand CreateCommand();

        void Open();

        void Close();
    }
}