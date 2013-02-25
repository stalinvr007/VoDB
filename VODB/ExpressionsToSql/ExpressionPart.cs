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
    class ExpressionPart
    {
        public String PropertyName { get; set; }
        public Field Field { get; set; }
        public Type EntityType { get; set; }
        public Table EntityTable { get; set; }
    }
}
