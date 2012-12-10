using System;
using System.Collections.Generic;
using VODB.VirtualDataBase;

namespace VODB
{

    /// <summary>
    /// Base class that marks a derived class as an Entity type.
    /// </summary>
    public abstract class Entity
    {
        private readonly IDictionary<Field, Object> OriginalKeyValues = new Dictionary<Field, object>();


        internal abstract Table Table { get; }
        /// <summary>
        /// Indicates if this entity has been fully loaded.
        /// </summary>
        /// <value>
        /// The is loaded.
        /// </value>
        internal Boolean IsLoaded { get; set; }
        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        internal ISession Session { get; set; }


        internal Object GetKeyOriginalValue(Field field)
        {
            return OriginalKeyValues[field];
        }

        internal void AddKeyOriginalValue(Field field, Object value)
        {
            OriginalKeyValues[field] = value;
        }

    }
}
