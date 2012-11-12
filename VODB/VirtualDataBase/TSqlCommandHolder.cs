using System;
using System.Threading;
using VODB.VirtualDataBase.TSqlCommands;

namespace VODB.VirtualDataBase
{
    internal sealed class TSqlCommandHolder : ITSqlCommandHolder
    {
        private readonly Table _table;
        private string _select;
        private string _update;
        private string _selectById;
        private string _insert;
        private string _delete;
        private string _count;
        
        private Thread _th_select;
        private Thread _th_selectById;
        private Thread _th_update;
        private Thread _th_insert;
        private Thread _th_delete;


        public TSqlCommandHolder(Table table)
        {
            _table = table;
            RunBuildCommandThreads();
        }

        private void RunBuildCommandThreads()
        {    
            _th_select = new Thread(() => _select = new TSelect(_table).BuildCmdStr());
            _th_selectById = new Thread(() => _selectById = new TSelectById(_table).BuildCmdStr());
            _th_update = new Thread(() => _update = new TUpdate(_table).BuildCmdStr());
            _th_insert = new Thread(() => _insert = new TInsert(_table).BuildCmdStr());
            _th_delete = new Thread(() => _delete = new TDelete(_table).BuildCmdStr());
            _count = string.Format("Select count(*) From [{0}]", _table.TableName);

            new ThreadCollection(_th_select, _th_selectById, _th_update, _th_insert, _th_delete)
                .StartAll();
        }

        private  static void Wait(ref Thread thread)
        {
            if (thread != null)
            {
                thread.Join();
            }
            thread = null;
        }

        public String Select
        {
            get
            {
                Wait(ref _th_select);
                return _select;
            }
        }

        public String SelectById
        {
            get
            {
                Wait(ref _th_selectById); 
                return _selectById;
            }
        }

        public String Update
        {
            get
            {
                Wait(ref _th_update); 
                return _update;
            }
        }

        public String Insert
        {
            get
            {
                Wait(ref _th_insert); 
                return _insert;
            }
        }

        public String Delete
        {
            get
            {
                Wait(ref _th_delete); 
                return _delete;
            }
        }

        public String Count
        {
            get { return _count; }
        }
    }
}
