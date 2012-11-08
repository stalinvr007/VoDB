using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Caching;
using VODB.Modules;
using VODB.VirtualDataBase;

namespace VODB
{

    /// <summary>
    /// Gives extra funcionality to an entity. Also indicates this is an entity to map.
    /// </summary>
    public abstract class DbEntity
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DbEntity" /> class.
        /// </summary>
        public DbEntity()
        {
            TablesCache.AsyncAdd(
                this.GetType(), 
                new TableCreator(this.GetType())
            );
        }

    }

}
