using System.Collections.Generic;
using System.Data.Common;

namespace VODB
{
    /// <summary>
    /// Allows users to interact with a database using strongly typed Objects.
    /// </summary>
    public interface ISession
    {
        ITransaction BeginTransaction();


        /// <summary>
        /// Gets all entities from this session.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : DbEntity, new();



    }

    internal interface IInternalSession
    {
        DbCommand CreateCommand();

        void Open();

        void Close();
    }
}