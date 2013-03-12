using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODB.Infrastructure
{
    /// <summary>
    /// Represents a Data Table Field.
    /// </summary>
    public interface IField
    {

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        String Name { get; }

    }
}
