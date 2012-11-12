using System;
using System.Reflection;

namespace VODB.VirtualDataBase
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
        /// Gets or sets the is key.
        /// </summary>
        /// <value>
        /// The is key.
        /// </value>
        public Boolean IsKey { get; set; }

        /// <summary>
        /// Gets or sets the is identity.
        /// </summary>
        /// <value>
        /// The is identity.
        /// </value>
        public Boolean IsIdentity { get; set; }

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
