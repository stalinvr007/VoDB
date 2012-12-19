using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core.Infrastructure;
using VODB.Exceptions;
using VODB.Core.Infrastructure;
using System.Threading.Tasks;

namespace VODB.Core
{
    interface IEntityTables
    {
        Table GetTable<TEntity>();

        void Map<TEntity>();
    }

    internal class EntityTables : IEntityTables
    {

        readonly IDictionary<Type, Table> _tables = new Dictionary<Type, Table>();

        public Table GetTable<TEntity>()
        {
            Table table;
            if (_tables.TryGetValue(typeof(TEntity), out table))
            {
                return table;
            }

            throw new EntityMapNotFoundException<TEntity>();
        }

        public void Map<TEntity>()
        {
            _tables[typeof(TEntity)] = new AsyncTable(
                new Task<Table>(() =>
                {
                    return Engine.Get<ITableMapper<TEntity>>().Table;
                }));
        }

    }
}
