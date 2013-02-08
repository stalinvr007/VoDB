using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using VODB.Core.Infrastructure;

namespace VODB.Core.Loaders
{
    class DbReaderMapper : IDbReaderMapper
    {
        private readonly IDictionaryMapper _dictionaryMapper;
        readonly ConcurrentDictionary<int, IDictionary<string, object>> fullData = new ConcurrentDictionary<int, IDictionary<string, object>>();
        private volatile int status = 0;

        public DbReaderMapper(IDictionaryMapper dictionaryMapper)
        {
            _dictionaryMapper = dictionaryMapper;
        }

        #region Implementation of IDbReaderMapper

        public Task<IEnumerable<TEntity>> Map<TEntity>(IDataReader reader) where TEntity : new()
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
                        if (taskRows.IsCompleted)
                        {
                            return entities;
                        }

                        // Enable data to be loaded.
                        Thread.Sleep(0);
                    }

                    // fetch a row data
                    var data = fullData[++current];

                    // Creates a new TEntity instance to hold it in place.
                    var entity = new TEntity();
                    entities.Add(entity);

                    // Map the Diccionary<String, Object> to an TEntity.
                    Map(data, table, entity);

                } while (!taskRows.IsCompleted);

                // Wait until all instances of Entity are loaded.
                while (status > 0)
                {
                    Thread.Sleep(0);
                }

                return entities;
            });
        }

        #endregion

        private void Map<TEntity>(IDictionary<string, object> data, Table entityTable, TEntity entity)
        {
            Interlocked.Increment(ref status);

            Task.Factory.StartNew(() => _dictionaryMapper.Map(data, entityTable, entity))
                .ContinueWith(_ => Interlocked.Decrement(ref status));

        }

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