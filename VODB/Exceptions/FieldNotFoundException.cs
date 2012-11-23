using System;

namespace VODB.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class FieldNotFoundException : VodbException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldNotFoundException" /> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="ex">The ex.</param>
        public FieldNotFoundException(string fieldName, string tableName, Exception ex)
            : base(ex, "The field [{0}] was not found on the table [{1}].", fieldName, tableName) { }
    }
}
