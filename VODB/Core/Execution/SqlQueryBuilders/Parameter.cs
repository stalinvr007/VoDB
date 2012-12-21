using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core.Infrastructure;
using VODB.ExpressionParser;

namespace VODB.Core.Execution.SqlPartialBuilders
{
    class Parameter : Key
    {
        private readonly Object _Entity;
        public Parameter(Field field, string paramName, Object entity)
            : base(field, paramName)
        {
            _Entity = entity;
        }

        public Object Value { get { return Field.GetValue(_Entity); } }
    }
}
