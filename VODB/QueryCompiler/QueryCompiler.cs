﻿using System.Collections.Generic;

namespace VODB.QueryCompiler
{
    class QueryCompiler<TEntity> : IQuery<TEntity>
    {
        public QueryCompiler(IEnumerable<TEntity> query)
        {
            
        }

        public IEnumerable<TEntity> Execute(ISession session)
        {
            throw new System.NotImplementedException();
        }
    }
}
