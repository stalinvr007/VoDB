using System;
using System.Collections.Generic;
using System.Diagnostics;
using VODB.Core.Infrastructure;
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
            Debug.Assert(type.Namespace != null, "type.Namespace != null");
            if (type.Namespace.Equals("Castle.Proxies"))
            {
                type = type.BaseType;
            }

            Table table;
            Debug.Assert(type != null, "type != null");
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
                new Task<Table>(() => Engine.Get<ITableMapper>().GetTable(type)));
        }
        
        public bool IsMapped(Type type)
        {
            Debug.Assert(type.BaseType != null, "type.BaseType != null");
            return _tables.ContainsKey(type) || _tables.ContainsKey(type.BaseType);
        }
    }
}
