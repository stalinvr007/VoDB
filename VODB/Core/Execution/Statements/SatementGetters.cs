using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.Statements
{
    class SelectByIdGetter : IStatementGetter
    {
        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.SelectById;
        }
    }

    class SelectGetter : IStatementGetter
    {
        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.Select;
        }
    }

    class CountGetter : IStatementGetter
    {
        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.Count;
        }
    }

    class CountByIdGetter : IStatementGetter
    {
        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.CountById;
        }
    }

    class UpdateGetter : IStatementGetter
    {
        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.Update;
        }
    }

    class InsertGetter : IStatementGetter
    {
        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.Insert;
        }
    }

    class DeleteGetter : IStatementGetter
    {
        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.Delete;
        }
    }
}
