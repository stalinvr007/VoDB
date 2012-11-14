using System;

namespace VODB.Exceptions
{
    public class UnableToInstantiateTypeException : Exception
    {


        public UnableToInstantiateTypeException(Type type, Exception ex)
            : base(String.Format("The type {0} must have a parameterless constructor.", type.Name), ex)
        { }


    }
}
