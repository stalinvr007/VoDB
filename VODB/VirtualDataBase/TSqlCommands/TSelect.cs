using System.Text;




namespace VODB.VirtualDataBase.TSqlCommands
{


    /// <summary>
    /// Builder of Select By Id Commands
    /// </summary>
    internal sealed class TSelect : TSqlCmdBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="TSelect"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public TSelect(Table entity)
            : base(entity) {

        }/// <summary>
        /// Builds the CMD STR.
        /// </summary>
        /// <param name="sb">The sb.</param>
        protected override void BuildCmdStr(StringBuilder sb) {
            if (Table.TableName.ToLower().StartsWith("sys."))
                sb.Append("Select * ")
                    .Append(" From ")
                    .Append(Table.TableName);
            else
                sb.Append("Select * ")
                    .Append(" From [")
                    .Append(Table.TableName)
                    .Append("]");
        }
    }
}
