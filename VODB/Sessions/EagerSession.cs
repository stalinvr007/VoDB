using System;
using System.Collections.Generic;
using VODB.DbLayer;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.DbLayer.Loaders;

namespace VODB.Sessions
{

    internal class EagerInternalSession : InternalSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EagerInternalSession"/> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        internal EagerInternalSession(IDbConnectionCreator creator = null)
            : base(creator)
        {
        }

        private TResult RunAndClose<TResult>(Func<TResult> action)
        {
            Open();
            try
            {
                return action();
            }
            finally
            {
                Close();
            }
        }

        public override IEnumerable<TEntity> GetAll<TEntity>()
        {
            return RunAndClose(() => new DbEntityQueryExecuterEager<TEntity>(
                                         new DbEntitySelectCommandFactory<TEntity>(this),
                                         new FullEntityLoader<TEntity>()
                                         ).Execute());
        }

        
    }
}