using System;

namespace VODB.Annotations
{

    public class DbFieldBase : Attribute
    {

        public DbFieldBase(String fieldName)
        {
            FieldName = fieldName;
        }

        internal String FieldName { get; private set; }
    }

    /// <summary>
    /// Indicates that this is a DataBase Table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DbTableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbTableAttribute" /> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public DbTableAttribute(String tableName)
        {
            TableName = tableName;
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        internal String TableName { get; private set; }
    }

    /// <summary>
    /// Indicates that this is a DataBase Field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DbFieldAttribute : DbFieldBase 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbFieldAttribute" /> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public DbFieldAttribute(String fieldName) : base(fieldName)
        {
            
        }

    }

    /// <summary>
    /// Indicates that this is a DataBase Key Field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DbKeyAttribute : DbFieldBase
    {
        public DbKeyAttribute(String fieldName = null)
            : base(fieldName)
        {
            
        }
       
    }

    /// <summary>
    /// Indicates that this is a DataBase Identity Field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DbIdentityAttribute : DbFieldBase
    {
        public DbIdentityAttribute(String fieldName = null)
            : base(fieldName)
        {
            
        }
        
    }

    /// <summary>
    /// Indicates that this is a Required Field (Not null).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DbRequiredAttribute : Attribute
    {
    }

    /// <summary>
    /// Indicates that this should be ignored.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DbIgnoreAttribute : Attribute
    {
    }

    /// <summary>
    /// Indicates the Foreign key to bind.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DbBindAttribute : DbFieldBase
    {
        public DbBindAttribute(String fieldName)
            : base(fieldName)
        {
            
        }
        
    }
}