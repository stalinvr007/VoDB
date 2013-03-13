using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODB.Infrastructure
{
    class Table : ITable
    {

        public Table(String name, IList<IField> fields, IList<IField> keys)
        {
            Name = name;
            Fields = fields;
            Keys = keys;
        }

        public String Name { get; private set; }

        public IList<IField> Fields { get; private set; }

        public IList<IField> Keys { get; private set; }
    }
}
