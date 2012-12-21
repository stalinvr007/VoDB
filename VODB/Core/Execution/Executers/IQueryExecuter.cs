using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.Core.Execution.SqlPartialBuilders;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;
using VODB.Core.Loaders;
using VODB.Core.Loaders.Factories;
using VODB.ExpressionParser;

namespace VODB.Core.Execution.Executers
{
    interface IQueryExecuter
    {
        IEnumerable RunQuery(Type entityType, IInternalSession session, String query, IEnumerable<Parameter> parameters);
    }
}
