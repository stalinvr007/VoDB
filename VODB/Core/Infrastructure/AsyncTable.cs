using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VODB.Core.Infrastructure
{
    class AsyncTable : Table
    {

        private readonly Task<Table> _Task;

        public AsyncTable(Task<Table> mappingTask)
        {
            _Task = mappingTask;
            mappingTask.Start();
        }

        Table InnerTable { get { return _Task.Result; } }

        public override ITSqlCommandHolder CommandsHolder
        {
            get
            {
                return InnerTable.CommandsHolder;
            }
            set
            {
                InnerTable.CommandsHolder = value;
            }
        }


        public override IEnumerable<Field> Fields
        {
            get
            {
                return InnerTable.Fields;
            }
            set
            {
                InnerTable.Fields = value;
            }
        }

        public override IEnumerable<Field> KeyFields
        {
            get
            {
                return InnerTable.KeyFields;
            }
            set
            {
                InnerTable.KeyFields = value;
            }
        }

        public override string TableName
        {
            get
            {
                return InnerTable.TableName;
            }
            set
            {
                InnerTable.TableName = value;
            }
        }
    }
}
