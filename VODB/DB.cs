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
            Tables.Map<TEntity>();
        }

        internal static IEntityTables Tables
        {
            get
            {
                return _tables;
            }
        }

    }

}
