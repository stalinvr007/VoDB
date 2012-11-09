using System.Linq;
using System.Text;


namespace VODB.VirtualDataBase.TSqlCommands
{
    /// <summary>
    /// Builder for the update command.
    /// </summary>
    internal sealed class TUpdate : TSqlCmdBase {
        /// <summary>
        /// Initializes a new instance of the <see cref="TUpdate"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public TUpdate(Table entity)
            : base(entity) {

        }

        /// <summary>
        /// Builds the CMD STR.
        /// </summary>
        /// <param name="sb">The sb.</param>
        protected override void BuildCmdStr(StringBuilder sb) {
            sb.Append("Update [")
                .Append(Table.TableName)
                .Append("] Set");

            foreach (var field in Table.Fields.Where(f => !f.IsIdentity))
                sb.AppendFormat(" [{0}] = @{0}, ", field.FieldName);

            sb.Remove(sb.Length - 2, 2);

            sb.Append(" Where ")
                .Append(new TWhere(Table, true).BuildCmdStr());
        }
    }
}
