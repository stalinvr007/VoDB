using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VODB.Core;
using VODB.Core.Infrastructure;

namespace VODB.ExpressionsToSql
{
    static class ExpressionDecoderExtensions
    {
        public static IList<ExpressionPart> Reduce(this IList<ExpressionPart> parts)
        {
            for (int i = 1; i < parts.Count; i++)
            {
                if (AreBinded(parts[i].Field, parts[i - 1].Field))
                {
                    parts.RemoveAt(i);                    
                }
            }
            return parts;
        }

        private static bool AreBinded(Field field1, Field field2)
        {
            return field1.FieldName.Equals(field2.BindedTo, StringComparison.InvariantCultureIgnoreCase)
                || field1.FieldName.Equals(field2.FieldName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
