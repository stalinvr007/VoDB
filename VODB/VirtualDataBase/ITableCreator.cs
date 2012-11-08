using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODB.VirtualDataBase
{
    /// <summary>
    /// Creates Tables with fields.
    /// </summary>
    internal interface ITableCreator<TEntity>
    {

        /// <summary>
        /// Creates a table.
        /// </summary>
        /// <returns></returns>
        Table Create();

    }
}
