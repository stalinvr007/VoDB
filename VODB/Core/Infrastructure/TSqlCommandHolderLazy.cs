using System;
using VODB.Core.Infrastructure.TSqlCommands;

namespace VODB.Core.Infrastructure
{
    internal interface ITSqlCommandHolder
    {
        Table Table { get; set; }

        string Count { get; }
        string Delete { get; }
        string Insert { get; }
        string Select { get; }
        string SelectById { get; }
        string Update { get; }
        string CountById { get; }
    }

    internal class TSqlCommandHolderLazy : ITSqlCommandHolder
    {
        private string _count;
        private string _countById;

        private string _delete;

        private string _insert;

        private string _select;

        private string _selectById;

        private string _update;

        #region ITSqlCommandHolder Members

        public Table Table { get; set; }

        public string Count
        {
            get { return FillCommand(ref _count, new TCount(Table)); }
        }

        public string Delete
        {
            get { return FillCommand(ref _delete, new TDelete(Table)); }
        }

        public string Insert
        {
            get { return FillCommand(ref _insert, new TInsert(Table)); }
        }

        public string Select
        {
            get { return FillCommand(ref _select, new TSelect(Table)); }
        }

        public string SelectById
        {
            get { return FillCommand(ref _selectById, new TSelectById(Table)); }
        }

        public string Update
        {
            get { return FillCommand(ref _update, new TUpdate(Table)); }
        }

        public string CountById
        {
            get { return FillCommand(ref _countById, new TCountById(Table)); }
        }

        #endregion

        private static string FillCommand(ref String holder, ITSqlCommand builder)
        {
            return holder ?? (holder = builder.BuildCmdStr());
        }
    }
}