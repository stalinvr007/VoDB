using System;
using System.Threading;
using VODB.Caching;
using VODB.VirtualDataBase;

namespace VODB
{

    /// <summary>
    /// Gives extra funcionality to an entity. Also indicates this is an entity to map.
    /// </summary>
    public abstract class DbEntity
    {

        private readonly Type _Type;

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
                Table t;
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
        protected DbEntity()
        {
            _Type = GetType();
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
