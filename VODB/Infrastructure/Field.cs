using System;
using System.Reflection;

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

        public bool IsKey { get; internal set; }

        public bool IsIdentity { get; internal set; }

        public ITable Table { get; internal set; }

        public void SetValue(Object entity, Object value)
        {
            if (value == DBNull.Value)
            {
                value = null;
            }

            _ValueSetter(entity, value);
        }

        public object GetValue(Object entity)
        {
            return _ValueGetter(entity);
        }

        public object GetFieldFinalValue(object entity)
        {
            return GetValue(entity);
        }

        public IField BindToField { get; internal set; }

        public PropertyInfo Info { get; internal set; }

        public void SetFieldFinalValue(object entity, object value)
        {
            SetValue(entity, value);
        }

        public string BindOrName { get { return Name; } }
    }
}