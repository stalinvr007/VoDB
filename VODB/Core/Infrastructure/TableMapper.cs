using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Annotations;
using VODB.Core.Infrastructure;

namespace VODB.Core.Infrastructure
{

    interface ITableMapper<TEntity>
    {
        Table Table { get; }
    }

    class TableMapper<TEntity> : ITableMapper<TEntity>
    {

        private readonly IFieldMapper<TEntity> _FieldMapper;
        private readonly ITSqlCommandHolder _SqlCommands;

        public TableMapper(IFieldMapper<TEntity> fieldMapper, ITSqlCommandHolder sqlCommands)
        {
            _SqlCommands = sqlCommands;
            _FieldMapper = fieldMapper;
        }

        public Table Table
        {
            get
            {
                return CreateTable();
            }
        }

        private Table CreateTable()
        {
            var table = new Table();

            Parallel.Invoke(
                () => table.TableName = GetTableName(typeof(TEntity)),
                () => table.Fields = _FieldMapper.GetFields().ToList(),
                () => table.KeyFields = _FieldMapper.GetFields().Where(f => f.IsKey).ToList(),
                () => { 
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

}
