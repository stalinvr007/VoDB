using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        private Type _Type;

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        internal Table Table
        {
            get
            {
                Table t = null;
                while((t= TablesCache.GetTable(_Type)) == null)
                {
                    Thread.Yield();
                }
                return t;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbEntity" /> class.
        /// </summary>
        public DbEntity()
        {
            _Type = this.GetType();
            TablesCache.AsyncAdd(
                _Type,
                new TableCreator(_Type)
            );
        }

        protected TModel GetValue<TModel>()
        {
            return default(TModel);
        }

        protected void SetValue<TModel>(TModel value)
        {

        }

    }

}
