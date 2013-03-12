using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODB.Infrastructure
{
    class Table : ITable
    {

        public Table(String name, IList<IField> fields)
        {
            Name = name;
            Fields = fields;
        }

        public String Name { get; private set; }

        public IList<IField> Fields { get; private set; }

    }
}
