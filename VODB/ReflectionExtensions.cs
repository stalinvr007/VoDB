using System.Linq;
using System.Reflection;

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
