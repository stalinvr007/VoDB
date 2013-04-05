using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Expressions;
using VODB.ExpressionsToSql;
using VODB.Infrastructure;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class CompositeCompiler : ISqlCompiler
    {

        IList<ISqlCompiler> _Compilers;

        public CompositeCompiler()
        {
            _Compilers = new List<ISqlCompiler>();
        }
                
        public void Add(ISqlCompiler compiler)
        {
            _Compilers.Add(compiler);
        }

        public void Insert(int index, ISqlCompiler compiler)
        {
            _Compilers.Insert(index, compiler);
        }

        public int Count { get { return _Compilers.Count; } }

        public String Compile()
        {
            var sb = new StringBuilder();

            foreach (var compiler in _Compilers)
            {
                sb.Append(compiler.Compile());
            }

            return sb.ToString();
        }
    }
}
