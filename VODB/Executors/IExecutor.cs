using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODB.Executors
{
    /// <summary>
    /// Executes 
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
