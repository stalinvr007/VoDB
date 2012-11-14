using System;
using System.Threading.Tasks;
using VODB.VirtualDataBase.TSqlCommands;

namespace VODB.VirtualDataBase
{
    internal sealed class TSqlCommandHolder : ITSqlCommandHolder
    {
        private readonly Table _table;
        private Task<String> _th_delete;
        private Task<String> _th_insert;

        private Task<String> _th_select;
        private Task<String> _th_selectById;
        private Task<String> _th_update;

        public TSqlCommandHolder(Table table)
        {
            _table = table;
            RunBuildCommandThreads();
        }

        #region ITSqlCommandHolder Members

        public String Select
        {
            get { return _th_select.Result; }
        }

        public String SelectById
        {
            get { return _th_selectById.Result; }
        }

        public String Update
        {
            get { return _th_update.Result; }
        }

        public String Insert
        {
            get { return _th_insert.Result; }
        }

        public String Delete
        {
            get { return _th_delete.Result; }
        }

        public string Count { get; private set; }

        #endregion

        private void RunBuildCommandThreads()
        {
            _th_select = new Task<String>(() => new TSelect(_table).BuildCmdStr());
            _th_selectById = new Task<String>(() => new TSelectById(_table).BuildCmdStr());
            _th_update = new Task<String>(() => new TUpdate(_table).BuildCmdStr());
            _th_insert = new Task<String>(() => new TInsert(_table).BuildCmdStr());
            _th_delete = new Task<String>(() => new TDelete(_table).BuildCmdStr());
            Count = string.Format("Select count(*) From [{0}]", _table.TableName);

            new TasksCollection(_th_select, _th_selectById, _th_update, _th_insert, _th_delete)
                .StartAll();
        }
    }
}