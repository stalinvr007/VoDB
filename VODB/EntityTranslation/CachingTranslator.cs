using System;
using VODB.Infrastructure;
using Fasterflect;
using VODB.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using VODB.Exceptions;
using System.Collections;
using VODB.TableToSql;

namespace VODB.EntityTranslation
{
    class CachingTranslator : IEntityTranslator
    {
        private static IDictionary<Type, Task<ITable>> tables = new Dictionary<Type, Task<ITable>>();
        private readonly IEntityTranslator _Translator;

        public CachingTranslator(IEntityTranslator translator)
        {
            _Translator = translator;
        }

        public ITable Translate(Type entityType)
        {
            Task<ITable> cached;

            if (tables.TryGetValue(entityType, out cached))
            {
                return cached.Result;
            }

            cached = tables[entityType] = Task<ITable>.Factory.StartNew(() => _Translator.Translate(entityType));

            return cached.Result;
        }
    }
}
