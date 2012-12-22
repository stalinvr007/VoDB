using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core.Infrastructure;

namespace VODB.Exceptions
{
    public class EntityMapNotFoundException<TEntity> : VodbException
    {
        public EntityMapNotFoundException()
            : base("No entity map was found for [{0}] type.", typeof(TEntity))
        {
            
        }
        
    }
}
