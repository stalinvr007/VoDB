using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.VirtualDataBase
{
    /// <summary>
    /// Creates Tables with fields.
    /// Convention based and property Annotations to find out the table fields of an Entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    internal sealed class TableCreator<TEntity> : ITableCreator
    {

        public Table Create()
        {

        }

    }
}
