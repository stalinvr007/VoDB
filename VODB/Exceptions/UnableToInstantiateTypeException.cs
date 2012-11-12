using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.DbLayer.Exceptions
{
    public class UnableToInstantiateTypeException : Exception
    {


        public UnableToInstantiateTypeException(Type type, Exception ex)
            : base(String.Format("The type {0} must have a parameterless constructor.", type.Name), ex)
        { }


    }
}
