﻿using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Gets the identity field.
        /// </summary>
        /// <value>
        /// The identity field.
        /// </value>
        public virtual Field IdentityField
        {
            get
            {
                return KeyFields.FirstOrDefault(f => f.IsIdentity);
            }
        }

        public Field FindCollectionField(String Name)
        {
            return CollectionFields.FirstOrDefault(f => f.FieldName.Equals(Name));
        }

        public Field FindField(String BindOrName)
        {
            Field fieldFound;

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