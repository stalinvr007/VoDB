using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Execution.Executers.DbResults;
using VODB.DbLayer;

namespace VODB.Sessions
{
    class ConnectionManager : IConnectionManager, IDbCommandFactory
    {
        private readonly IDbConnectionCreator _Creator;
        private DbConnection _Connection;
        private Boolean _Opened;

        public ConnectionManager(IDbConnectionCreator creator)
        {
            _Creator = creator;
        }
        
        #region IConnectionManager Implementation

        private bool IsOpened
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
                _Opened = true;
            }
        }

        public void Close()
        {
            try
            {
                _Connection.Close();
            }
            finally
            {
                _Opened = false;
            }            
        }

        #endregion

        #region IDbCommandFactory Implementation
        
        public DbCommand MakeCommand()
        {
            Open();
            return _Connection.CreateCommand();
        } 

        #endregion
    }
    /// <summary>
    /// The connection manager is responsable to open the connection 
    /// and close it when it is no longer needed.
    /// </summary>
    public interface IConnectionManager
    {

        /// <summary>
        /// Asks the manager to open the connection.
        /// </summary>
        void Open();

        /// <summary>
        /// Asks the manager to close the connection.
        /// </summary>
        void Close();

    }
}
