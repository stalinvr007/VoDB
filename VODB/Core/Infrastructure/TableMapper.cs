using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Annotations;
using VODB.Core.Infrastructure;

namespace VODB.Core.Infrastructure
{


    interface ITableMapper
    {
        Table GetTable(Type type);
    }

    interface ITableMapper<TEntity>
    {
        Table Table { get; }
    }

    class TableMapper : ITableMapper
    {
        private readonly IFieldMapper _FieldMapper;
        private readonly ITSqlCommandHolder _SqlCommands;

        public TableMapper(IFieldMapper fieldMapper, ITSqlCommandHolder sqlCommands)
        {
            _SqlCommands = sqlCommands;
            _FieldMapper = fieldMapper;
        }

        public Table GetTable(Type type)
        {
            var table = new Table();

            Parallel.Invoke(
                () => table.TableName = GetTableName(type),
                () => table.Fields = _FieldMapper.GetFields(type).Where(f => !f.IsCollection) .ToList(),
                () => table.CollectionFields = _FieldMapper.GetFields(type).Where(f => f.IsCollection).ToList(),
                () => table.KeyFields = _FieldMapper.GetFields(type).Where(f => f.IsKey).ToList(),
                () =>
                {
                    table.CommandsHolder = _SqlCommands;
                    table.CommandsHolder.Table = table;
                }
            );

            Parallel.ForEach(table.Fields, f => f.Table = table);

            return table;
        }

        private static String GetTableName(Type type)
        {
            var tableAttr = type.GetCustomAttributes(typeof(DbTableAttribute), true).FirstOrDefault() as DbTableAttribute;

            return tableAttr == null ? type.Name : tableAttr.TableName;
        }

    }

    class TableMapper<TEntity> : ITableMapper<TEntity>
    {

        private readonly ITableMapper _TableMapper;
        public TableMapper(ITableMapper tableMapper)
        {
            _TableMapper = tableMapper;
        }

        public Table Table
        {
            get
            {
                return _TableMapper.GetTable(typeof(TEntity));
            }
        }

    }

}
