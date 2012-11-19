using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;
using VODB.Extensions;

namespace VODB.EntityValidators.Fields
{
    public class RefTypeFieldIsFilled : FieldIsFilled
    {
        protected override Boolean IsFilled(object value)
        {
            return value != null;
        }
        protected override Boolean CanHandle(Type fieldType)
        {
            return fieldType.IsClass;
        }
    }
}
