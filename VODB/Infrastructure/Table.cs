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

        public IEnumerable<IField> Fields { get; internal set; }

        public IEnumerable<IField> Keys { get; internal set; }
    }
}
