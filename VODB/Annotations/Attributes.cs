﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Annotations
{

    /// <summary>
    /// Indicates that this is a DataBase Table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DbTableAttribute : Attribute
    {

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        internal String TableName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTableAttribute" /> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public DbTableAttribute(String tableName)
        {
            TableName = tableName;
        }

    }

    /// <summary>
    /// Indicates that this is a DataBase Field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public sealed class DbFieldAttribute : Attribute
    {

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        internal String FieldName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbFieldAttribute" /> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public DbFieldAttribute(String fieldName)
        {
            FieldName = fieldName;
        }

    }

    /// <summary>
    /// Indicates that this is a DataBase Key Field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DbKeyAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        internal String FieldName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbFieldAttribute" /> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public DbKeyAttribute(String fieldName)
        {
            FieldName = fieldName;
        }
    }

    /// <summary>
    /// Indicates that this is a DataBase Identity Field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DbIdentityAttribute : Attribute
    {

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        internal String FieldName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbFieldAttribute" /> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public DbIdentityAttribute(String fieldName)
        {
            FieldName = fieldName;
        }

    }

}
