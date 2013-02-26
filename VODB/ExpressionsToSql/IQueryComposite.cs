using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.ExpressionsToSql
{
    public interface IQueryConditionComposite : IQueryCondition
    {

        void Add(IQueryCondition query);

    }
}
