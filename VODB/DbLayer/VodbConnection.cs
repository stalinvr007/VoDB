using System;
using System.Data.Common;

namespace VODB.DbLayer
{
    class VodbConnection : IVodbConnection, IVodbCommandFactory
    {
        private readonly IDbConnectionCreator _Creator;
        private DbConnection _Connection;
        private IVodbTransaction _Transaction;
        private DbTransaction _DbTransaction;

        public VodbConnection(IDbConnectionCreator creator)
        {
            _Creator = creator;
        }

        private void CreateConnection()
        {
            if (_Connection == null)
            {
                _Connection = _Creator.Create();
            }
        }

        #region IConnectionManager Implementation
        
        internal bool IsOpened { get; private set; }

        public void Open()
        {
            if (IsOpened)
            {
                return;
            }

            CreateConnection();
            _Connection.Open();
            IsOpened = true;
        }

        public void Close()
        {

            if (_Transaction != null && _Transaction.IsActive)
            {
                return;
            }

            try
            {
                if (_Connection != null)
                {
                    _Connection.Close();
                }
            }
            finally
            {
                IsOpened = false;
            }
        }

        public IVodbTransaction BeginTransaction()
        {            
            if (_Transaction == null)
            {
                Open();
                _DbTransaction = _Connection.BeginTransaction();
                return _Transaction = new VodbTransaction(_DbTransaction);
            }

            return new VodbInnerTransaction((VodbTransaction)_Transaction);
        }

        #endregion

        #region IVodbCommandFactory Implementation

        public IVodbCommand MakeCommand()
        {
            CreateConnection();
            var command = _Connection.CreateCommand();
            command.Transaction = _DbTransaction;

            return new VodbCommand(command);
        }

        #endregion

        public void Dispose()
        {
            Close();

            if (_Connection == null)
            {
                return;
            }

            _Connection.Dispose();
            _Connection = null;

            if (_DbTransaction == null)
            {
                return;
            }

            _DbTransaction.Dispose();
            _DbTransaction = null;
        }

        private TResult Execute<TResult>(IVodbCommand cmd, Func<IVodbCommand, TResult> action)
        {
            Open();
            try
            {
                return action(cmd);
            }
            finally
            {
                Close();
            }
        }

        public int ExecuteNonQuery(IVodbCommand command)
        {
            return Execute(command, c => c.ExecuteNonQuery());
        }

        public System.Data.IDataReader ExecuteReader(IVodbCommand command)
        {
            return Execute(command, c => c.ExecuteReader());
        }

        public object ExecuteScalar(IVodbCommand command)
        {
            return Execute(command, c => c.ExecuteScalar());
        }
    }
}
