using System;
using VODB.Core.Infrastructure;
using VODB.Infrastructure;

namespace VODB.Expressions
{
    public interface IExpressionPiece
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        String PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        IField Field { get; set; }
        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        Type EntityType { get; set; }
        /// <summary>
        /// Gets or sets the entity table.
        /// </summary>
        /// <value>
        /// The entity table.
        /// </value>
        ITable EntityTable { get; set; }
    }
}
