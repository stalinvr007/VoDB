﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VODB.VirtualDataBase;

namespace VODB.Caching
{
    /// <summary>
    /// Virtual Table Cache.
    /// </summary>
    internal static class TablesCache
    {
        private static readonly IDictionary<Type, Table> _tables = new Dictionary<Type, Table>();
        private static readonly IDictionary<Type, Task> _pendingTasks = new Dictionary<Type, Task>();

        /// <summary>
        /// Adds the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="table">The table.</param>
        public static void Add(Type type, Table table)
        {
            lock (_tables)
            {
                _tables[type] = table;
            }
        }

        /// <summary>
        /// Adds a Table to the cache asynchronously.
        /// </summary>
        /// <param name="creator">The creator.</param>
        public static void AsyncAdd<TEntity>(ITableCreator creator)
        {
            AsyncAdd(typeof (TEntity), creator);
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

            var task = new Task<Table>(() =>
                                           {
                                               Table table = creator.Create();

                                               lock (_tables)
                                               {
                                                   _tables.Add(type, table);
                                               }

                                               return table;
                                           });

            lock (_pendingTasks)
            {
                _pendingTasks.Add(type, task);
            }

            task.Start();
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <returns></returns>
        public static Table GetTable<TType>()
        {
            return GetTable(typeof (TType));
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Table GetTable(Type type)
        {
            Table value = null;

            lock (_pendingTasks)
            {
                Task task;
                if (_pendingTasks.TryGetValue(type, out task))
                {
                    value = ((Task<Table>) task).Result;
                    _pendingTasks.Remove(type);
                }
            }

            if (value != null)
            {
                return value;
            }

            lock (_tables)
            {
                _tables.TryGetValue(type, out value);
            }

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