using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VODB
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
