using System.Text;

namespace VODB.Infrastructure.TSqlCommands
{
    /// <summary>
    /// Builder of Select By Id Commands
    /// </summary>
    internal sealed class TCountById : TSqlCmdBase
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
            if (Table.TableName.ToLower().StartsWith("sys."))
                sb.Append("Select Count(*) ")
                    .Append(" From ")
                    .Append(Table.TableName);
            else
                sb.Append("Select Count(*) ")
                    .Append(" From [")
                    .Append(Table.TableName)
                    .Append("]");

            sb.Append(" Where ")
              .Append(new TWhere(Table).BuildCmdStr());
        }
    }
}
