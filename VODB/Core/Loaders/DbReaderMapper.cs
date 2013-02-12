using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using VODB.Core.Infrastructure;
using VODB.Core.Loaders.Factories;

namespace VODB.Core.Loaders
{
    internal class DbReaderMapper : IDbReaderMapper
    {
        private readonly IDictionaryMapper _dictionaryMapper;

        private readonly IEntityFactory _EntityFactory;
        private readonly ConcurrentDictionary<int, IDictionary<string, object>> fullData =
            new ConcurrentDictionary<int, IDictionary<string, object>>();

        private readonly IInternalSession _Session;
        private volatile int status;

        public DbReaderMapper(
            IInternalSession session,
            IDictionaryMapper dictionaryMapper, 
            IEntityFactory entityFactory)
        {
            _Session = session;
            _EntityFactory = entityFactory;
            _dictionaryMapper = dictionaryMapper;
        }

        #region Implementation of IDbReaderMapper

        public Task<IEnumerable<TEntity>> Map<TEntity>(IDataReader reader) where TEntity : class, new()
        {
            Table table = Engine.GetTable<TEntity>();
            Type entityType = typeof(TEntity);

            Task taskRows = FetchRowsData(reader, table);

            return Task<IEnumerable<TEntity>>.Factory.StartNew(() =>
            {
                var entities = new List<TEntity>();
                int current = 0;
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
                    IDictionary<string, object> data =
                        fullData[++current];

                    // Creates a new TEntity instance to hold it in place.
                    var entity = _EntityFactory.Make(entityType, _Session) as TEntity;
                    entities.Add(entity);

                    // Map the Diccionary<String, Object> to an TEntity.
                    Map(data, table, entity);
                } while (!taskRows.IsCompleted ||
                         fullData.Count > current);

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

            Task.Factory.StartNew(() =>
            {
                try
                {
                    _dictionaryMapper.Map(data, entityTable, entity).Wait();
                }
                catch (AggregateException)
                {
                    throw;
                }
                finally
                {
                    Interlocked.Decrement(ref status);
                }
            });
        }

        private Task FetchRowsData(IDataReader reader, Table table)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    int rowI = 0;
                    while (reader.Read())
                    {
                        var data = new Dictionary<String, Object>();

                        foreach (Field field in table.Fields)
                        {
                            data[field.FieldName] = reader[field.FieldName];
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