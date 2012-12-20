using System;
using VODB.Core.Infrastructure.TSqlCommands;

namespace VODB.Core.Infrastructure
{
    internal class TSqlCommandHolderLazy : ITSqlCommandHolder
    {

        public Table Table { get; set; }

        private static string FillCommand(ref String holder, ITSqlCommand builder)
        {
            if (holder == null)
            {
                holder = builder.BuildCmdStr();
            }
            return holder;
        }

        string _count;
        public string Count
        {
            get
            {
                return FillCommand(ref _count, new TCount(Table));
            }
        }

        string _delete;
        public string Delete
        {
            get
            {
                return FillCommand(ref _delete, new TDelete(Table));
            }
        }

        string _insert;
        public string Insert
        {
            get
            {
                return FillCommand(ref _insert, new TInsert(Table));
            }
        }

        string _select;
        public string Select
        {
            get
            {
                return FillCommand(ref _select, new TSelect(Table));
            }
        }

        string _selectById;
        public string SelectById
        {
            get
            {
                return FillCommand(ref _selectById, new TSelectById(Table));
            }
        }

        string _update;
        public string Update
        {
            get
            {
                return FillCommand(ref _update, new TUpdate(Table));
            }
        }

        string _countById;
        public string CountById
        {
            get
            {
                return FillCommand(ref _countById, new TCountById(Table));
            }
        }
    }
}
