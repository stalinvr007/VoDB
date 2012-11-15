using System;
using System.Collections.Generic;
using VODB.Caching;
using VODB.DbLayer.DbExecuters;
using VODB.VirtualDataBase;

namespace VODB
{
    /// <summary>
    /// Gives extra funcionality to an entity. Also indicates this is an entity to map.
    /// </summary>
    public abstract class DbEntity
    {
        private readonly Type _Type;

        private Table _table;

        private readonly IDictionary<Field, Object> OriginalKeyValues = new Dictionary<Field, object>();

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

        internal Object GetKeyOriginalValue(Field field)
        {
            return OriginalKeyValues[field];
        }

        internal void AddKeyOriginalValue(Field field, Object value)
        {
            OriginalKeyValues[field] = value;
        }

        #region FOREIGN KEYS GETTER SETTER

        private readonly IDictionary<Type, Object> _ForeignEntities = new Dictionary<Type, Object>();

        internal ISession Session { get; set; }

        protected TModel GetValue<TModel>()
            where TModel : DbEntity, new()
        {
            Object value;
            _ForeignEntities.TryGetValue(typeof(TModel), out value);

            TModel model = value as TModel;

            if (Session != null && model != null && !model.IsLoaded)
            {
                model = Session.GetById<TModel>(model) as TModel;
                SetValue(model);
            }

            return model;
        }

        protected IDbQueryResult<TEntity> GetValues<TEntity>()
            where TEntity : DbEntity, new()
        {
            return Session.GetAll<TEntity>();
        }

        protected void SetValue<TModel>(TModel value)
        {
            _ForeignEntities[typeof(TModel)] = value;
        }

        #endregion

        /// <summary>
        /// Indicates that this entity has been loaded with database data.
        /// </summary>
        internal Boolean IsLoaded { get; set; }

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
    }
}