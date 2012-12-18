using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Infrastructure;

namespace VODB.Core.Infrastructure
{

    interface ITableMapper<TEntity>
    {
        Table Table { get; }
    }

    class TableMapper<TEntity> : ITableMapper<TEntity>
    {

        private readonly IFieldMapper<TEntity> _FieldMapper;

        public TableMapper(IFieldMapper<TEntity> fieldMapper)
        {
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

            table.Fields = _FieldMapper.GetFields().ToList();
            table.KeyFields = table.Fields.Where(f => f.IsKey).ToList();

            return table;
        }


    }

}
