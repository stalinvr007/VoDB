using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.DbLayer.Exceptions;
using VODB.VirtualDataBase;

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
