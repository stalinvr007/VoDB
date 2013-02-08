using System.Text;

namespace VODB.Core.Infrastructure.TSqlCommands
{
    /// <summary>
    /// Builder of Select By Id Commands
    /// </summary>
    internal sealed class TSelectById : TSqlCmdBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TSelectById"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public TSelectById(Table entity)
            : base(entity)
        {
        }

        /// <summary>
        /// Builds the CMD STR.
        /// </summary>
        /// <param name="sb">The sb.</param>
        protected override void BuildCmdStr(StringBuilder sb)
        {
            sb.Append(new TSelect(Table).BuildCmdStr())
                .Append(" Where ")
                .Append(new TWhere(Table).BuildCmdStr());
        }
    }
}