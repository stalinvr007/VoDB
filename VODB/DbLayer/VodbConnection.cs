using System;
using System.Data.Common;

namespace VODB.DbLayer
{
    class VodbConnection : IVodbConnection
    {
        private readonly IDbConnectionCreator _Creator;
        private DbConnection _DbConnection;
        private IVodbTransaction _Transaction;
        private DbTransaction _DbTransaction;

        public VodbConnection(IDbConnectionCreator creator)
        {
            _Creator = creator;
        }

        private void CreateConnection()
        {
            if (_DbConnection == null)
            {
                _DbConnection = _Creator.Create();
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
            _DbConnection.Open();
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
                if (_DbConnection != null)
                {
                    _DbConnection.Close();
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
                _DbTransaction = _DbConnection.BeginTransaction();
                return _Transaction = new VodbTransaction(_DbTransaction);
            }

            return new VodbInnerTransaction((VodbTransaction)_Transaction);
        }

        #endregion

        #region IVodbCommandFactory Implementation

        public IVodbCommand MakeCommand()
        {
            CreateConnection();
            return new VodbCommand(_DbConnection.CreateCommand());
        }

        #endregion

        public void Dispose()
        {
            Close();

            if (_DbConnection == null)
            {
                return;
            }

            _DbConnection.Dispose();
            _DbConnection = null;

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
                cmd.SetConnection(_DbConnection);
                cmd.SetTransaction(_DbTransaction);
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
