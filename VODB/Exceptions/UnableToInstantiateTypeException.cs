using System;

namespace VODB.Exceptions
{
    public class UnableToInstantiateTypeException : VodbException
    {


        public UnableToInstantiateTypeException(Type type, Exception ex)
            : base(ex, "The type {0} must have a parameterless constructor.", type.Name)
        { }


    }
}
