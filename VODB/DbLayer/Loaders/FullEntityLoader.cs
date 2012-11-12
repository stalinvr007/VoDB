using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace VODB.DbLayer.Loaders
{
    /// <summary>
    /// Loads all the data to the entity from the DataReader.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal class FullEntityLoader<TModel> : EntityLoader<TModel>
            where TModel : DbEntity, new()
    {

        /// <summary>
        /// Loads the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override void Load(TModel entity, DbDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
