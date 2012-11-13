using System;
using System.Collections;
using System.Collections.Generic;
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
        /// Indicates that this entity has been loaded with database data.
        /// </summary>
        internal Boolean IsLoaded { get; set; }

        private Table _table;

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
                if (_table != null)
                {
                    return _table;
                }

                _table = TablesCache.GetTable(_Type);                
                return _table;
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

        #region FOREIGN KEYS GETTER SETTER

        readonly IDictionary<Type, Object> _ForeignEntities = new Dictionary<Type, Object>();

        protected TModel GetValue<TModel>()
            where TModel : class
        {
            object value;
            _ForeignEntities.TryGetValue(typeof(TModel), out value);
            return value as TModel;
        }

        protected void SetValue<TModel>(TModel value)
        {
            _ForeignEntities[typeof(TModel)] = value;
        }

        #endregion
    }

}
