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
    public class VodbInnerTransaction : IVodbTransaction
    {
        private readonly DbTransaction _Transaction;

        public VodbInnerTransaction(DbTransaction transaction)
        {
            _Transaction = transaction;
        }

        public void Commit() { /* */ }

        public void Rollback()
        {
            _Transaction.Rollback();
        }
    }
}
