using System;

namespace VODB.Infrastructure
{
    class Field : IField
    {
        private readonly Func<Object, Object> _ValueGetter;
        private readonly Action<Object, Object> _ValueSetter;

        public Field(String name, Type entityType, Action<Object, Object> ValueSetter, Func<Object, Object> ValueGetter)
        {
            _ValueSetter = ValueSetter;
            _ValueGetter = ValueGetter;
            Name = name;
            EntityType = entityType;
        }

        public String Name { get; private set; }

        public Type EntityType { get; private set; }

        public void SetValue(Object entity, Object value)
        {
            _ValueSetter(entity, value);
        }

        public object GetValue(Object entity)
        {
            return _ValueGetter(entity);
        }
    }
}
