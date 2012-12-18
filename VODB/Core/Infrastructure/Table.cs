using System;
using System.Collections.Generic;

namespace VODB.Infrastructure
{

    /// <summary>
    /// Represents a DataBase Table.
    /// </summary>
    internal sealed class Table
    {

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public String TableName { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public IEnumerable<Field> Fields { get; set; }

        /// <summary>
        /// Gets or sets the key fields.
        /// </summary>
        /// <value>
        /// The key fields.
        /// </value>
        public IEnumerable<Field> KeyFields { get; set; }

        /// <summary>
        /// Gets or sets the commands holder.
        /// </summary>
        /// <value>
        /// The commands holder.
        /// </value>
        public ITSqlCommandHolder CommandsHolder { get; set; }

    }
}
