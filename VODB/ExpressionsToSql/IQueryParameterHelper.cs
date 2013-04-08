using System;
using System.Collections.Generic;
using VODB.Infrastructure;

namespace VODB.ExpressionsToSql
{
    static class IQueryParameterHelper
    {

        public static String Add(this ICollection<IQueryParameter> collection, int level, IField field, object value)
        {
            var parameter = new QueryParameter
            {
                Name = "@p" + level,
                Value = value,
                Field = field
            };

            collection.Add(parameter);

            return parameter.Name;
        }

    }
}
