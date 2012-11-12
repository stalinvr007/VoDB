using System;
using System.Text;


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
        /// <param name="table">The table.</param>
        protected TSqlCmdBase(Table table) {
            Table = table;
        }

        /// <summary>
        /// Builds the CMD STR.
        /// </summary>
        /// <returns></returns>
        public String BuildCmdStr() {
            var sb = new StringBuilder();
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
