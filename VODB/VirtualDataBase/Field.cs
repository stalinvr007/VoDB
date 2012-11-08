using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.VirtualDataBase
{
    /// <summary>
    /// Represents a DataBase Table Field.
    /// </summary>
    internal sealed class Field
    {

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public String FieldName { get; set; }

        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        /// <value>
        /// The type of the field.
        /// </value>
        public Type FieldType { get; set; }

        /// <summary>
        /// Gets or sets the is key.
        /// </summary>
        /// <value>
        /// The is key.
        /// </value>
        public Boolean IsKey { get; set; }

    }
}
