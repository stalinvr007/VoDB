using System;

namespace VODB.Exceptions
{
    public class FieldSetterNotFoundException : Exception
    {
        public FieldSetterNotFoundException(Type fieldType)
            : base(String.Format("No handler was found for type [{0}].", fieldType.Name))
        {
        }
    }
}