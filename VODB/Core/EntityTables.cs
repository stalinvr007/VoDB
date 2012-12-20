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

        Table GetTable(Type type);

        void Map<TEntity>();

        void Map(Type type);
    }

    internal class EntityTables : IEntityTables
    {

        readonly IDictionary<Type, Table> _tables = new Dictionary<Type, Table>();

        public Table GetTable<TEntity>()
        {
            return GetTable(typeof(TEntity));
        }

        public Table GetTable(Type type)
        {
            Table table;
            if (_tables.TryGetValue(type, out table))
            {
                return table;
            }

            Map(type);

            return GetTable(type);
        }

        public void Map<TEntity>()
        {
            Map(typeof(TEntity));
        }

        public void Map(Type type)
        {
            if (_tables.ContainsKey(type))
            {
                return;
            }

            _tables[type] = new AsyncTable(
                new Task<Table>(() =>
                {
                    return Engine.Get<ITableMapper>().GetTable(type);
                }));
        }
    }
}
