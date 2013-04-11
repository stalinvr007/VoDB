using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VODB.Sessions.EntityFactories
{
    /// <summary>
    /// The semanthics of this type if the following:
    /// A value is loaded when the counter is bigger than 1. (set once by the mapper, and another by the lazy load)
    /// </summary>
    class PropertyValue
    {

        private Object _Value;
        public Object Value
        {
            get
            {
                return _Value;
            }
            set
            {
                // this value will be set by the entity loader/mapper with the identity keys
                // filled with data from the db.
                if (!IsLoaded) ++SettedCount;
                _Value = value;
            }
        }

        public int SettedCount { get; set; }

        public Boolean IsLoaded
        {
            get { return SettedCount > 1; }
            set { SettedCount = value ? 2 : 1; }
        }
    }
}
