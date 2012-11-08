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

        /// <summary>
        /// Adds a Table to the cache asynchronously.
        /// </summary>
        /// <param name="creator">The creator.</param>
        public static void AsyncAdd<TEntity>(ITableCreator creator)
        {
            AsyncAdd(typeof(TEntity), creator);
        }

        /// <summary>
        /// Adds a Table to the cache asynchronously.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="creator">The creator.</param>
        public static void AsyncAdd(Type type, ITableCreator creator)
        {

            if (_tables.ContainsKey(type))
            {
                return;
            }

            new Thread(() =>
            {
                var table = creator.Create();
                _tables.Add(type, table);
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

        /// <summary>
        /// Gets the tables.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Table> GetTables()
        {
            return _tables.Values;
        }

    }

}
