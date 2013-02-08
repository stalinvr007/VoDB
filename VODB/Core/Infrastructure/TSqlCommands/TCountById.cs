using System.Text;

namespace VODB.Core.Infrastructure.TSqlCommands
{
    /// <summary>
    /// Builder of Count By Id Commands
    /// </summary>
    internal sealed class TCountById : TCount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TSelectById"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public TCountById(Table entity)
            : base(entity)
        {
        }

        /// <summary>
        /// Builds the CMD STR.
        /// </summary>
        /// <param name="sb">The sb.</param>
        protected override void BuildCmdStr(StringBuilder sb)
        {
            base.BuildCmdStr(sb);

            sb.Append(" Where ")
                .Append(new TWhere(Table).BuildCmdStr());
        }
    }
}