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

        Type _EntityType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCreator{TEntity}" /> class.
        /// </summary>
        public TableCreator()
        {
            _EntityType = typeof(TEntity);
        }

        /// <summary>
        /// Creates a table.
        /// </summary>
        /// <returns></returns>
        public Table Create()
        {
            var table = new Table()
            {
                TableName = GetTableName(_EntityType)
            };


            return table;
        }

        public static String GetTableName(Type type)
        {
            /* Todo: Check for annotations and get name from one if any. Else its the name of the type. */
            return type.Name;
        }

    }
}
