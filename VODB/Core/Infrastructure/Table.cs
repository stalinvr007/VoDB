using System;
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
            return CollectionFields.FirstOrDefault(f => f.FieldName.Equals(Name, StringComparison.InvariantCultureIgnoreCase));
        }

        public Field FindFieldByBind(String bind)
        {
            bind = bind.ToLower();
            return FieldsByBind.FirstOrDefault(kv => kv.Key.EndsWith(bind)).Value;            
        }

        public Field FindField(String BindOrName)
        {
            BindOrName = BindOrName.ToLower();
            Field fieldFound;

            if (FieldsByName.TryGetValue(BindOrName, out fieldFound))
            {
                return fieldFound;
            }

            if (FieldsByBind.TryGetValue(BindOrName, out fieldFound))
            {
                return fieldFound;
            }

            if ((fieldFound = FindFieldByBind(BindOrName)) != null)
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