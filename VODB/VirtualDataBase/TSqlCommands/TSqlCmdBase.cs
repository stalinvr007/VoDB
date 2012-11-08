using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;


namespace VODB.VirtualDataBase.TSqlCommands
{

    /// <summary>
    /// Base to build a TSqlCommand
    /// </summary>
    internal abstract class TSqlCmdBase {
        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        protected Table Table { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlCmdBase" /> class.
        /// </summary>
        /// <param name="Table">The table.</param>
        public TSqlCmdBase(Table table) {
            Table = table;
        }

        /// <summary>
        /// Builds the CMD STR.
        /// </summary>
        /// <returns></returns>
        public String BuildCmdStr() {
            StringBuilder sb = new StringBuilder();
            BuildCmdStr(sb);
            return sb.ToString();
        }

        /// <summary>
        /// Builds the CMD STR.
        /// </summary>
        /// <param name="sb">The sb.</param>
        protected abstract void BuildCmdStr(StringBuilder sb);

    }
}
