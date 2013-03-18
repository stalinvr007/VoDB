using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;

namespace VODB.TableToSql
{
    abstract class SqlBuilderBase : ISqlBuilder
    {
        public SqlBuilderBase(SqlBuilderType type)
        {
            BuilderType = type;
        }

        public SqlBuilderType BuilderType { get; private set; }

        public abstract string Build(ITable table);

        public override string ToString()
        {
            return BuilderType.ToString();
        }
    }

    abstract class SqlBuilderBaseById : SqlBuilderBase
    {
        protected static ISqlBuilder Where = new WhereIdBuilder();

        public SqlBuilderBaseById(SqlBuilderType type) : base(type){ }

        public override abstract string Build(ITable table);
    }
}
