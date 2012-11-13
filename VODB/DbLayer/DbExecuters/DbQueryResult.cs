using System;
using System.Collections.Generic;

namespace VODB.DbLayer.DbExecuters
{

    internal sealed class DbQueryResult
    {

        public DbQueryResult(IEnumerable<Object> values)
        {
            Values = values;
        }

        public IEnumerable<Object> Values { get; private set; }

    }
}
