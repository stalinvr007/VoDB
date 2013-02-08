using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.SqlPartialBuilders
{
    internal class SqlQueryBuilder : ISqlQueryBuilder
    {
        private readonly Field _Field;
        private readonly ICollection<Parameter> _Parameters;
        private readonly Table _Table;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlQueryBuilder" /> class.
        /// </summary>
        /// <param name="field">The field that should return a query.</param>
        public SqlQueryBuilder(Field field)
        {
            _Field = field;
            _Parameters = new List<Parameter>();

            Type ienumType = field.FieldType.GetGenericArguments().FirstOrDefault();

            _Table = Engine.GetTable(ienumType);
        }

        #region ISqlQueryBuilder Members

        /// <summary>
        /// Adds the condition.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ISqlQueryBuilder AddCondition(Field field, Object value)
        {
            _Parameters.Add(new Parameter(field, "p" + _Parameters.Count, value));
            return this;
        }

        public string Query
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(_Table.CommandsHolder.Select)
                    .Append(" Where ");

                foreach (Parameter param in _Parameters)
                {
                    sb.AppendFormat("{0} = @{1}", param.Field.FieldName, param.ParamName);
                }

                return sb.ToString();
            }
        }


        public IEnumerable<Parameter> Parameters
        {
            get { return _Parameters; }
        }

        #endregion
    }
}