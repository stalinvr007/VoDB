using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using VODB.VirtualDataBase;

namespace VODB.Caching
{

    /// <summary>
    /// Virtual Table Cache.
    /// </summary>
    internal static class TablesCache
    {

        static readonly IDictionary<Type, Table> _tables = new Dictionary<Type, Table>();

        /// <summary>
        /// Adds the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="table">The table.</param>
        public static void Add(Type type, Table table)
        {
            _tables[type] = table;
        }

        public static void AsyncAdd<TEntity>(ITableCreator<TEntity> creator)
        {

            if (_tables.ContainsKey(typeof(TEntity)))
            {
                return;
            }
            
            new Thread(() =>
            {
                var table = creator.Create();
                _tables.Add(typeof(TEntity), table);
            }).Start();

        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <returns></returns>
        public static Table GetTable<TType>()
        {
            return GetTable(typeof(TType));
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Table GetTable(Type type)
        {
            Table value = null;

            _tables.TryGetValue(type, out value);

            return value;
        }

    }

}
