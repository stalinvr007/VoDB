using System.Collections.Generic;
using VODB.Core.Infrastructure;

namespace VODB.Exceptions
{
    public class ValidationException : VodbException
    {
        public ValidationException(string message, IEnumerable<Field> fields)
            : base(message)
        {
            Fields = fields;
        }

        public IEnumerable<Field> Fields { get; private set; }
    }
}