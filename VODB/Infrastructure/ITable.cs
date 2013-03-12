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
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        String Name { get; }

    }

}
