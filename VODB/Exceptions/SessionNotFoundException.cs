using System;
using System.Collections.Generic;
using VODB.Caching;
using VODB.DbLayer.DbExecuters;
using VODB.VirtualDataBase;

namespace VODB.Exceptions
{
    public class SessionNotFoundException : VodbException
    {
        public SessionNotFoundException(string tableName)
            : base("The session object was not set for a tuple of {0}.", tableName)
        {

        }
    }
}
