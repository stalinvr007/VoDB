using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VODB.Core.Infrastructure;
using VODB.ExpressionParser;

namespace VODB.ExpressionsToSql
{
    public interface IQueryParameter
    {
        String Name { get; }
        Object Value { get; }
    }
}
