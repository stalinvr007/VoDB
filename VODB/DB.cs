using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core;

namespace VODB
{
    
    public static class DB
    {

        static IEntityTables _tables = Engine.Get<IEntityTables>();
        
        public static void Map<TEntity>()
        {
            _tables.Map<TEntity>();
        }

    }

}
