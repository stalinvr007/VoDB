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
    internal sealed class EntityConverter<TModel>
        where TModel : class, new()
    {

        public TModel Convert(DbDataReader reader)
        {
            return new TModel();
        }

    }
}
