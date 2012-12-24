using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core.Infrastructure;

namespace VODB.Exceptions
{
    public class EntityMapNotFoundException<TEntity> : EntityMapNotFoundException
    {
        public EntityMapNotFoundException()
            : base(typeof(TEntity)) { }

    }

    public class EntityMapNotFoundException : VodbException
    {
        public EntityMapNotFoundException(Type type)
            : base("No entity map was found for [{0}] type.", type)
        { }
    }
}
