using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Execution.Executers.DbResults;

namespace VODB.DbLayer
{
    class VodbConnection : IVodbConnection, IDbCommandFactory
    {
        private readonly IDbConnectionCreator _Creator;
        private DbConnection _Connection;
        private DbTransaction _Transaction;
        private Boolean _Opened;

        public VodbConnection(IDbConnectionCreator creator)
        {
            _Creator = creator;
        }

        #region IConnectionManager Implementation

        internal bool IsOpened
        {
            get
            {
                return _Opened;
            }
        }

        public void Open()
        {
            if (!IsOpened)
            {
                _Connection = _Creator.Create();
                _Connection.Open();
                _Opened = true;
            }
        }

        public void Close()
        {
            try
            {
                if (_Connection != null)
                {
                    _Connection.Close();
                }
            }
            finally
            {
                _Opened = false;
            }
        }

        public IVodbTransaction BeginTransaction()
        {
            if (_Transaction == null)
            {
                Open();
                _Transaction = _Connection.BeginTransaction();
                return new VodbTransaction(_Transaction);
            }

            return new VodbInnerTransaction(_Transaction);
        }

        #endregion

        #region IDbCommandFactory Implementation

        public IVodbCommand MakeCommand()
        {
            Open();
            var command = _Connection.CreateCommand();
            command.Transaction = _Transaction;

            return new VodbCommand(command);
        }

        #endregion

        public void Dispose()
        {
            Close();

            if (_Connection != null)
            {
                _Connection.Dispose();
                _Connection = null;
            }
        }

        
    }
}
