using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.ExpressionsToSql
{
    public interface IQueryComposite
    {

        void Add(IQuery query);

    }
}
