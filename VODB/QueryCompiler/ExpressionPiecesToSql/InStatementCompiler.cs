using System;
using System.Collections.Generic;
using System.Text;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class InStatementCompiler : ISqlCompiler
    {
        private readonly IEnumerable<ISqlCompiler> _Values;

        public InStatementCompiler(IEnumerable<ISqlCompiler> values)
        {
            _Values = values;
        }

        public String Compile()
        {
            var sb = new StringBuilder(" In (");

            foreach (var value in _Values)
            {
                sb.Append(value.Compile()).Append(", ");
            }

            return sb.Remove(sb.Length-2, 2).Append(")").ToString();
        }
    }
}
