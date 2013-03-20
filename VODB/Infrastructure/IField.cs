using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        /// Indicates the field is a key.
        /// </summary>
        /// <value>
        /// The is key.
        /// </value>
        Boolean IsKey { get; }

        /// <summary>
        /// Indicates the field is a identity field.
        /// </summary>
        /// <value>
        /// The is key.
        /// </value>
        Boolean IsIdentity { get; }

        /// <summary>
        /// Sets the value of the field.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="value">The value.</param>
        void SetValue(Object entity, Object value);

        /// <summary>
        /// Sets the field final value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="value">The value.</param>
        void SetFieldFinalValue(Object entity, Object value);

        /// <summary>
        /// Gets the value of the field.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Object GetValue(Object entity);

        /// <summary>
        /// Gets the field final value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Object GetFieldFinalValue(Object entity);

        /// <summary>
        /// Gets the field this field in binded to.
        /// </summary>
        /// <value>
        /// The field this field in binded.
        /// </value>
        IField BindToField { get; }

        /// <summary>
        /// Gets the name of the binded field or the name of the current field 
        /// if there's no bind.
        /// </summary>
        /// <value>
        /// The name of the bind or.
        /// </value>
        String BindOrName { get; }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        ITable Table { get; }

        /// <summary>
        /// Gets the info.
        /// </summary>
        /// <value>
        /// The info.
        /// </value>
        PropertyInfo Info { get; }
    }
}
