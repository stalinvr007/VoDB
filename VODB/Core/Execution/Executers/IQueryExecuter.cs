using System;
using System.Collections;
using System.Collections.Generic;
using VODB.Core.Execution.SqlPartialBuilders;

namespace VODB.Core.Execution.Executers
{
    interface IQueryExecuter
    {
        IEnumerable RunQuery(Type entityType, IInternalSession session, String query, IEnumerable<Parameter> parameters);
    }
}
