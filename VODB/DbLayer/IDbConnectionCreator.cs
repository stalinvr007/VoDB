using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace VODB.DbLayer
{
    internal interface IDbConnectionCreator
    {

        DbConnection Create();

    }
}
