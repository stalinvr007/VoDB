using System;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.Statements
{
    internal class SelectByIdGetter : IStatementGetter
    {
        #region IStatementGetter Members

        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.SelectById;
        }

        #endregion
    }

    internal class SelectGetter : IStatementGetter
    {
        #region IStatementGetter Members

        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.Select;
        }

        #endregion
    }

    internal class CountGetter : IStatementGetter
    {
        #region IStatementGetter Members

        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.Count;
        }

        #endregion
    }

    internal class CountByIdGetter : IStatementGetter
    {
        #region IStatementGetter Members

        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.CountById;
        }

        #endregion
    }

    internal class UpdateGetter : IStatementGetter
    {
        #region IStatementGetter Members

        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.Update;
        }

        #endregion
    }

    internal class InsertGetter : IStatementGetter
    {
        #region IStatementGetter Members

        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.Insert;
        }

        #endregion
    }

    internal class DeleteGetter : IStatementGetter
    {
        #region IStatementGetter Members

        public String GetStatement(ITSqlCommandHolder holder)
        {
            return holder.Delete;
        }

        #endregion
    }

    internal class IdentityGetter : IStatementGetter
    {
        #region IStatementGetter Members

        public String GetStatement(ITSqlCommandHolder holder)
        {
            return "Select @@IDENTITY";
        }

        #endregion
    }
}