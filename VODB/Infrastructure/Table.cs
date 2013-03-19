using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODB.Infrastructure
{
    class Table : ITable
    {

        public Table(String name)
        {
            Name = name;
        }

        public String Name { get; private set; }

        public Type EntityType { get; internal set; }

        public IEnumerable<IField> Fields { get; internal set; }

        public IEnumerable<IField> Keys { get; internal set; }

        public override string ToString()
        {
            return Name;
        }

        public string SqlSelect { get; internal set; }

        public string SqlSelectById { get; internal set; }

        public string SqlCount { get; internal set; }

        public string SqlCountById { get; internal set; }

        public string SqlDeleteById { get; internal set; }

        public string SqlInsert { get; internal set; }

        public string SqlUpdate { get; internal set; }

        public IField IdentityField { get; internal set; }

        public void SetIdentityValue(object entity, object value)
        {
            if (IdentityField != null)
            {
                IdentityField.SetFieldFinalValue(entity, value);
            }
        }
    }
}
