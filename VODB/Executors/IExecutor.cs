using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.DbLayer;
using VODB.EntityMapping;
using VODB.EntityTranslation;

namespace VODB.Executors
{

    /// <summary>
    /// Represents a gathering of IDbExecutors to perform multiple actions.
    /// </summary>
    public interface IExecutor
    {

        void Delete<T>(T entity);
        void Update<T>(T entity);
        T Insert<T>(T entity);
        bool Exists<T>(T entity);
        T Query<T>(T entity);
        IEnumerable<T> Query<T>();

    }
}
