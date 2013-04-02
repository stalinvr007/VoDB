using System;
using System.Reflection;

namespace VODB.Infrastructure
{
    class DateTimeField : IField
    {
        private readonly IField _Field;

        public DateTimeField(IField field)
        {
            _Field = field;
        }

        public Object GetFieldFinalValue(object entity)
        {
            return GetValue(entity);
        }

        public string Name
        {
            get { return _Field.Name; }
        }

        public Type EntityType
        {
            get { return _Field.EntityType; }
        }

        public bool IsKey
        {
            get { return _Field.IsKey; }
        }

        public bool IsIdentity
        {
            get { return _Field.IsIdentity; }
        }

        public void SetValue(object entity, object value)
        {
            _Field.SetValue(entity, value);
        }

        public object GetValue(object entity)
        {
            return _Field.GetValue(entity);
        }

        public IField BindToField
        {
            get { return _Field.BindToField; }
        }

        public ITable Table
        {
            get { return _Field.Table; }
        }

        public PropertyInfo Info
        {
            get { return _Field.Info; }
        }

        public void SetFieldFinalValue(object entity, object value)
        {
            if (value == DBNull.Value)
            {
                value = null;
            }

            SetValue(entity, value ?? default(DateTime));
        }

        public string BindOrName { get { return _Field.BindOrName; } }
    }
}
