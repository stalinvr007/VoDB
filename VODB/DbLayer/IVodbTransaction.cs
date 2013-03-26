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
    /// <summary>
    /// Represents a scope of execution.
    /// </summary>
    public interface IVodbTransaction
    {
        /// <summary>
        /// Commits the changes made within this scope to the database.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks all changes made within this scope.
        /// </summary>
        void Rollback();
    }
}
