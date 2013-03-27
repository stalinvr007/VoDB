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

        #region IConnectionManager Implementation

        internal bool IsOpened { get; private set; }

        public void Open()
        {
            if (IsOpened)
            {
                return;
            }

            _Connection = _Creator.Create();
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
            Open();
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

        
    }
}
