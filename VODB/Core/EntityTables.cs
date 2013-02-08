using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VODB.Core.Infrastructure;
using VODB.Exceptions;

namespace VODB.Core
{
    internal interface IEntityTables
    {
        Table GetTable(Type type);
        void Map(Type type);
        Boolean IsMapped(Type type);
    }

    internal static class IEntityTablesExtensions
    {
        public static Table GetTable<TEntity>(this IEntityTables entityTables)
        {
            return entityTables.GetTable(typeof (TEntity));
        }

        public static void Map<TEntity>(this IEntityTables entityTables)
        {
            entityTables.Map(typeof (TEntity));
        }

        public static Boolean IsMapped<TEntity>(this IEntityTables entityTables)
        {
            return entityTables.IsMapped(typeof (TEntity));
        }
    }

    internal class EntityTables : IEntityTables
    {
        private readonly IDictionary<Type, Table> _tables = new Dictionary<Type, Table>();

        #region IEntityTables Members

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

            if (!type.Namespace.StartsWith("System"))
            {
                Config.MapNameSpace(type);
                if (_tables.TryGetValue(type, out table))
                {
                    return table;
                }
            }

            throw new EntityMapNotFoundException(type);
        }

        public void Map(Type type)
        {
            if (!type.IsClass || type.Namespace.StartsWith("System") || IsMapped(type))
            {
                return;
            }

            _tables[type] = new AsyncTable(
                new Task<Table>(() => Engine.Get<ITableMapper>().GetTable(type)));
        }

        public bool IsMapped(Type type)
        {
            if (type.Namespace.Equals("Castle.Proxies"))
            {
                type = type.BaseType;
            }

            return _tables.ContainsKey(type);
        }

        #endregion
    }
}