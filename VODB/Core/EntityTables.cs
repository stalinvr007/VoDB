using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core.Infrastructure;
using VODB.Exceptions;
using System.Threading.Tasks;

namespace VODB.Core
{
    interface IEntityTables
    {
        Table GetTable(Type type);        
        void Map(Type type);
        Boolean IsMapped(Type type);
    }

    static class IEntityTablesExtensions
    {
        public static Table GetTable<TEntity>(this IEntityTables entityTables)
        {
            return entityTables.GetTable(typeof(TEntity));
        }

        public static void Map<TEntity>(this IEntityTables entityTables)
        {
            entityTables.Map(typeof(TEntity));
        }

        public static Boolean IsMapped<TEntity>(this IEntityTables entityTables)
        {
            return entityTables.IsMapped(typeof(TEntity));
        }
    }

    internal class EntityTables : IEntityTables
    {

        readonly IDictionary<Type, Table> _tables = new Dictionary<Type, Table>();
        
        public Table GetTable(Type type)
        {
            if (type.Namespace.Equals("Castle.Proxies"))
            {
                type = type.BaseType;
            }

            Table table;
            if (_tables.TryGetValue(type, out table))
            {
                return table;
            }

            Map(type);

            return GetTable(type);
        }
        
        public void Map(Type type)
        {
            if (IsMapped(type))
            {
                return;
            }

            _tables[type] = new AsyncTable(
                new Task<Table>(() =>
                {
                    return Engine.Get<ITableMapper>().GetTable(type);
                }));
        }
        
        public bool IsMapped(Type type)
        {
            return _tables.ContainsKey(type) || _tables.ContainsKey(type.BaseType);
        }
    }
}
