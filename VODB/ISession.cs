using System.Collections.Generic;
using System.Data.Common;

namespace VODB
{
    public interface ISession
    {
        Transaction BeginTransaction();

        IEnumerable<TEntity> GetAll<TEntity>()
            where TEntity : DbEntity, new();
    }

    internal interface ISessionInternal
    {
        DbCommand CreateCommand();

        void Open();

        void Close();
    }
}