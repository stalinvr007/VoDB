using System.Linq;
using System.Text;

namespace VODB.Core.Infrastructure.TSqlCommands
{
    /// <summary>
    /// Builder of Insert Commands
    /// </summary>
    internal sealed class TInsert : TSqlCmdBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TInsert"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public TInsert(Table entity)
            : base(entity)
        {
        }

        /// <summary>
        /// Builds the CMD STR.
        /// </summary>
        /// <param name="sb">The sb.</param>
        protected override void BuildCmdStr(StringBuilder sb)
        {
            sb.Append("Insert into [")
                .Append(Table.TableName).Append("]");

            var sbFields = new StringBuilder("( ");
            foreach (Field field in Table.Fields.Where(field => !field.IsIdentity))
                sbFields.Append("[").Append(field.FieldName).Append("], ");

            sbFields.Remove(sbFields.Length - 2, 2);
            sbFields.Append(")");

            sb.Append(sbFields)
                .Append(" values ")
                .Append(sbFields.Replace("[", "").Replace("]", "").Replace(' ', '@'));
        }
    }
}