using System;
using System.Collections.Generic;
using VODB.VirtualDataBase;

namespace VODB.Exceptions
{
    public class ValidationException : VodbException
    {
        public IEnumerable<Field> Fields { get; private set; }
        
        public ValidationException(string message, IEnumerable<Field> fields)
            : base(message)
        {
            Fields = fields;
        }


    }
}