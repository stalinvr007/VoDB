using System;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.Statements
{

    /// <summary>
    /// Represents a Statement Getter
    /// </summary>
    interface IStatementGetter
    {

        /// <summary>
        /// Gets the statement.
        /// </summary>
        /// <param name="holder">The holder.</param>
        /// <returns></returns>
        String GetStatement(ITSqlCommandHolder holder);
    }
}
