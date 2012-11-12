using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace VODB.DbLayer.DbQuery
{

    /// <summary>
    /// Creates an entity from an DataReader.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal abstract class EntityConverter<TModel>
        where TModel : class, new()
    {

        /// <summary>
        /// Converts the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public TModel Convert(DbDataReader reader)
        {
            TModel newTModel = new TModel();
            LoadFieldsData(newTModel, reader);

            return newTModel;
        }

        /// <summary>
        /// Loads the fields data.
        /// </summary>
        /// <param name="newTModel">The new T model.</param>
        /// <param name="reader">The reader.</param>
        protected abstract void LoadFieldsData(TModel newTModel, DbDataReader reader);

    }


}
