using System.Text;

namespace VODB.Core.Infrastructure.TSqlCommands
{
    /// <summary>
    /// Builder of Count Commands
    /// </summary>
    internal class TCount : TSqlCmdBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TSelectById"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public TCount(Table entity)
            : base(entity)
        {
        }

        /// <summary>
        /// Builds the CMD STR.
        /// </summary>
        /// <param name="sb">The sb.</param>
        protected override void BuildCmdStr(StringBuilder sb)
        {
            sb.Append("Select Count(*) ")
                .Append(" From [")
                .Append(Table.TableName)
                .Append("]");
        }
    }
}