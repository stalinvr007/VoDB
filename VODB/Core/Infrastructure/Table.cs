using System;
using System.Collections.Generic;

namespace VODB.Core.Infrastructure
{

    /// <summary>
    /// Represents a DataBase Table.
    /// </summary>
    internal class Table
    {

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public virtual String TableName { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public virtual IEnumerable<Field> Fields { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public virtual IEnumerable<Field> CollectionFields { get; set; }

        /// <summary>
        /// Gets or sets the key fields.
        /// </summary>
        /// <value>
        /// The key fields.
        /// </value>
        public virtual IEnumerable<Field> KeyFields { get; set; }

        /// <summary>
        /// Gets or sets the commands holder.
        /// </summary>
        /// <value>
        /// The commands holder.
        /// </value>
        public virtual ITSqlCommandHolder CommandsHolder { get; set; }

        /// <summary>
        /// Gets or sets the mapping of field by fieldName.
        /// </summary>
        public virtual IDictionary<String, Field> FieldsByName { get; set; }

        /// <summary>
        /// Gets or sets the mapping of field by fieldName.
        /// </summary>
        public virtual IDictionary<String, Field> FieldsByBind { get; set; }

        /// <summary>
        /// Gets or sets the mapping of field by fieldName.
        /// </summary>
        public virtual IDictionary<String, Field> FieldsByPropertyName { get; set; } 

        public Field FindField(String BindOrName)
        {
            Field fieldFound = null;

            if (FieldsByName.TryGetValue(BindOrName, out fieldFound))
            {
                return fieldFound;
            }

            if (FieldsByBind.TryGetValue(BindOrName, out fieldFound))
            {
                return fieldFound;
            }

            if (FieldsByPropertyName.TryGetValue(BindOrName, out fieldFound))
            {
                return fieldFound;
            }

            return null;
        }

    }
}