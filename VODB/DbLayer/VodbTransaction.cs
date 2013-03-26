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
    public class VodbTransaction : IVodbTransaction
    {
        private readonly DbTransaction _Transaction;
        public VodbTransaction(DbTransaction transaction)
        {
            _Transaction = transaction;
        }

        public void Commit()
        {
            _Transaction.Commit();
        }

        public void Rollback()
        {
            _Transaction.Rollback();
        }
    }
}
