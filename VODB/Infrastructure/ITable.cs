using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODB.Infrastructure
{
    /// <summary>
    /// Represents a Data Table.
    /// </summary>
    public interface ITable
    {

        /// <summary>
        /// Gets the table name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        String Name { get; }

        /// <summary>
        /// Gets the table fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        IList<IField> Fields { get; }

        /// <summary>
        /// Gets the table keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        IList<IField> Keys { get; }
    }

}
