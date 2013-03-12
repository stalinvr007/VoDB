using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODB.Infrastructure
{
    public class Table : ITable
    {

        public Table(String name)
        {
            Name = name;
        }

        public String Name { get; private set; }
    }
}
