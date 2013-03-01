using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODB.ExpressionsToSql
{
    static class IQueryParameterHelper
    {

        public static String Add(this ICollection<IQueryParameter> collection, int level, object value)
        {
            var parameter = new QueryParameter
            {
                Name = "@p" + level,
                Value = value
            };

            collection.Add(parameter);

            return parameter.Name;
        }

    }
}
