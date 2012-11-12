using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.DbLayer.Loaders.TypeConverter;

namespace VODB
{
    /// <summary>
    /// Allows the end user to configure some aspects of the VODB Framework.
    /// </summary>
    public static class Configuration
    {

        /// <summary>
        /// Gets the field setters.
        /// </summary>
        /// <value>
        /// The field setters.
        /// </value>
        public static ICollection<IFieldSetter> FieldSetters { get; private set; }


        static Configuration()
        {
            FieldSetters = new List<IFieldSetter>()
            {
                new DbEntityFieldSetter(),
                new BasicFieldSetter()
            };
        }


    }
}
