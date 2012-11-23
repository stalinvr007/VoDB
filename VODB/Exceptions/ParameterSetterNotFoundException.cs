using System;

namespace VODB.Exceptions
{
    public class ParameterSetterNotFoundException : VodbException
    {
        public ParameterSetterNotFoundException(Type fieldType)
            : base("No handler was found for type [{0}].", fieldType.Name)
        {
        }
    }
}