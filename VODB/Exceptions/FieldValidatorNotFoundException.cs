using System;

namespace VODB.Exceptions
{
    public class FieldValidatorNotFoundException : Exception
    {
        public FieldValidatorNotFoundException(Type fieldType)
            : base(String.Format("No handler was found for type [{0}].", fieldType.Name))
        {
        }
    }
}
