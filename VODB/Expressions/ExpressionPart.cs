using System;
using VODB.Core.Infrastructure;
using VODB.Infrastructure;

namespace VODB.Expressions
{
    /// <summary>
    /// Represents a piece of an Expression.
    /// </summary>
    class ExpressionPart
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public String PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        public IField Field { get; set; }
        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public Type EntityType { get; set; }
        /// <summary>
        /// Gets or sets the entity table.
        /// </summary>
        /// <value>
        /// The entity table.
        /// </value>
        public ITable EntityTable { get; set; }
        
    }
}
