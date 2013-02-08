using System;

namespace VODB.Exceptions
{
    public class FieldValidatorNotFoundException : VodbException
    {
        public FieldValidatorNotFoundException(Type fieldType)
            : base("No handler was found for type [{0}].", fieldType.Name)
        {
        }
    }
}