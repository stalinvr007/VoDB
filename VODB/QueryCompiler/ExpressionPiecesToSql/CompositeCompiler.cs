using System;
using System.Collections.Generic;
using System.Text;

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
            forceCompile = true;
        }

        public void Insert(int index, ISqlCompiler compiler)
        {
            _Compilers.Insert(index, compiler);
            forceCompile = true;
        }

        public int Count { get { return _Compilers.Count; } }


        String compiledQuery;
        Boolean forceCompile;

        public String Compile()
        {
            if (!forceCompile)
            {
                return compiledQuery;
            }

            forceCompile = false;

            var sb = new StringBuilder();

            foreach (var compiler in _Compilers)
            {
                sb.Append(compiler.Compile());
            }

            return compiledQuery = sb.ToString();
        }
    }
}
