using System;
using System.Reflection;

namespace VODB.Core.Infrastructure
{
    /// <summary>
    /// Represents a DataBase Table Field.
    /// </summary>
    public sealed class Field
    {

        private readonly PropertyInfo _Prop;
        /// <summary>
        /// Initializes a new instance of the <see cref="Field" /> class.
        /// </summary>
        /// <param name="prop">The prop.</param>
        public Field(PropertyInfo prop)
        {
            _Prop = prop;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public String PropertyName
        {
            get { return _Prop.Name; }
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <value>
        /// The property.
        /// </value>
        public PropertyInfo Property { get { return _Prop; } }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        internal Table Table { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public String FieldName { get; set; }

        /// <summary>
        /// Gets or sets the binded to.
        /// </summary>
        /// <value>
        /// The binded to.
        /// </value>
        public String BindedTo { get; set; }

        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        /// <value>
        /// The type of the field.
        /// </value>
        public Type FieldType { get; set; }

        /// <summary>
        /// Gets or sets isKey flag.
        /// </summary>
        /// <value>
        /// The is key.
        /// </value>
        public Boolean IsKey { get; set; }

        /// <summary>
        /// Gets or sets isIdentity flag.
        /// </summary>
        /// <value>
        /// The is identity.
        /// </value>
        public Boolean IsIdentity { get; set; }

        /// <summary>
        /// Gets or sets isRequired flag.
        /// </summary>
        /// <value>
        /// The is required.
        /// </value>
        public Boolean IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the is a collection.
        /// </summary>
        /// <value>
        /// The is collection.
        /// </value>
        public Boolean IsCollection { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public Object GetValue(Object entity)
        {
            return _Prop.GetValue(entity, null);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="value">The value.</param>
        public void SetValue(Object entity, Object value)
        {
            _Prop.SetValue(entity, value, null);
        }

    }
}