using System;

namespace VODB.Exceptions
{
    public class ParameterSetterNotFoundException : Exception
    {
        public ParameterSetterNotFoundException(Type fieldType)
            : base(String.Format("No handler was found for type [{0}].", fieldType.Name))
        {
        }
    }
}