using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace VODB.DbLayer.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class FieldNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldNotFoundException" /> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="ex">The ex.</param>
        public FieldNotFoundException(string fieldName, string tableName, Exception ex)
            : base(String.Format("The field [{0}] was not found on the table [{1}].", fieldName, tableName), ex) { }
    }
}
