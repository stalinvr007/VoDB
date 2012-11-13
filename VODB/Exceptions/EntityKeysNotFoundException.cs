using System;
using System.Text;

namespace VODB.Exceptions
{
    public class EntityKeysNotFoundException : Exception
    {
        public EntityKeysNotFoundException(string tableName)
            : base(String.Format("No keys found for the model that represents [{0}] table.", tableName))
        {

        }
    }
}
