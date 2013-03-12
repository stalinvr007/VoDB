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

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        Type EntityType { get; }

        /// <summary>
        /// Sets the value of the field.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="value">The value.</param>
        void SetValue(Object entity, Object value);

        /// <summary>
        /// Gets the value of the field.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Object GetValue(Object entity);

    }
}
