using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VODB.Exceptions;
using VODB.VirtualDataBase;

namespace VODB.Extensions
{

    static class ReflectionExtensions
    {

        public static TAttribute GetAttribute<TAttribute>(this PropertyInfo info)
            where TAttribute : class
        {
            return info.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
        }



    }
}
