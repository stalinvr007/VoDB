using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.Loaders
{
    /// <summary>
    /// Loads the key fields from the datareader.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal class EntityKeyLoader<TModel> : EntityLoader<TModel>
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
            entity.Table.KeyFields
                .Select(field => SetValue(entity, field, GetValue(reader, field.FieldName)))
                .Count();
        }
    }
}
