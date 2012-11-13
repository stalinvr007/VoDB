using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.DbLayer;

namespace VODB
{
    public static class Sessions
    {

        public static ISession GetEager(IDbConnectionCreator creator = null)
        {
            return new EagerSession(creator);
        }

    }
}
