using System.Text;

namespace VODB.VirtualDataBase.TSqlCommands
{
    /// <summary>
    /// Builder of Delete Commands
    /// </summary>
    internal sealed class TDelete : TSqlCmdBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="TDelete" /> class.
        /// </summary>
        /// <param name="table">The table.</param>
        public TDelete(Table table)
            : base(table) {

        }
        /// <summary>
        /// Builds the CMD STR.
        /// </summary>
        /// <param name="sb">The sb.</param>
        protected override void BuildCmdStr(StringBuilder sb) {
            sb.Append("Delete [")
                .Append(Table.TableName)
                .Append("] Where ")
                .Append(new TWhere(Table).BuildCmdStr());
        }
    }
}
