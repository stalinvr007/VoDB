using System;

namespace VODB.Exceptions
{
    public class FieldSetterNotFoundException : VodbException
    {
        public FieldSetterNotFoundException(Type fieldType)
            : base("No handler was found for type [{0}].", fieldType.Name)
        {
        }
    }
}