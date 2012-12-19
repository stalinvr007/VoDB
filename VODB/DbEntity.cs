using System;
using System.Collections.Generic;
using VODB.Caching;
using VODB.DbLayer.DbResults;
using VODB.Exceptions;
using VODB.Core.Infrastructure;

namespace VODB
{
    /// <summary>
    /// Gives extra funcionality to an entity. Also indicates this is an entity to map.
    /// </summary>
    public abstract class DbEntity : Entity
    {

        private readonly Type _Type;

        private Table _table;

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

        private readonly IDictionary<Type, Object> _ForeignEntities = new Dictionary<Type, Object>();

        protected TModel GetValue<TModel>()
            where TModel : Entity, new()
        {
            Object value;
            _ForeignEntities.TryGetValue(typeof(TModel), out value);

            var model = value as TModel;

            if (Session != null && model != null && !model.IsLoaded)
            {
                model = Session.GetById(model);
                SetValue(model);
            }

            return model;
        }

        protected IDbQueryResult<TEntity> GetValues<TEntity>()
            where TEntity : Entity, new()
        {
            if (Session == null)
            {
                throw new SessionNotFoundException(Table.TableName);
            }
            return Session.GetAll<TEntity>();
        }

        protected void SetValue<TModel>(TModel value)
        {
            _ForeignEntities[typeof(TModel)] = value;
        }

        #endregion

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        internal override Table Table
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