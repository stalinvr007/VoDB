using System;
using System.Reflection;
using Fasterflect;

namespace VODB.Infrastructure
{
    /// <summary>
    /// This class is a wrapper for a IField that redefines the GetFieldFinalValue.
    /// Resolving the GetValue recursively.
    /// 
    /// Removes the comparations overhead when calling this method.
    /// </summary>
    class BindedField : IField
    {
        private readonly IField _Field;

        public BindedField(IField field)
        {
            _Field = field;            
        }

        public Object GetFieldFinalValue(object entity)
        {
            object value = GetValue(entity);
            return BindToField != null && value != null ? BindToField.GetValue(value) : value;
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
            if (BindToField != null)
            {
                var instance = BindToField.EntityType.CreateInstance();
                SetValue(entity, instance);
                BindToField.SetFieldFinalValue(instance, value);
            }
        }


        public string BindOrName { get { return BindToField != null ? BindToField.Name : Name; } }
    }
}
