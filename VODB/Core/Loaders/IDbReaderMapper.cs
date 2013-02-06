using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace VODB.Core.Loaders
{
    /// <summary>
    /// Maps a data reader into an entity.
    /// </summary>
    interface IDbReaderMapper
    {
        /// <summary>
        /// Maps the specified reader.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> Map<TEntity>(IDataReader reader);
    }

    class DbReaderMapper : IDbReaderMapper
    {
        ConcurrentDictionary<int, IDictionary<String, Object>> fullData = new ConcurrentDictionary<int, IDictionary<String, Object>>();

        #region Implementation of IDbReaderMapper

        public Task<IEnumerable<TEntity>> Map<TEntity>(IDataReader reader)
        {
            var table = Engine.GetTable<TEntity>();

            var taskRows = FetchRowsData(reader);

            return Task<IEnumerable<TEntity>>.Factory.StartNew(() =>
            {
                var entities = new List<TEntity>();
                var current = 0;
                do
                {
                    while (fullData.Count == current)
                    {
                        // Enable data to be loaded.
                        Thread.Sleep(0);
                    }



                } while (!taskRows.IsCompleted);

                return entities;
            });
        }

        #endregion


        /// <summary>
        /// Fetches the data from the data reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private Task FetchRowsData(IDataReader reader)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var rowI = 0;
                    while (reader.Read())
                    {
                        var data = new Dictionary<String, Object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            data[reader.GetName(i)] = reader[i];
                        }

                        fullData[++rowI] = data;
                    }
                }
                finally
                {
                    reader.Close();
                }
            });
        }

    }


}
