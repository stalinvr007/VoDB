using System;
using System.Text;


namespace VODB.VirtualDataBase.TSqlCommands
{
    /// <summary>
    /// Builder for Where Condition
    /// </summary>
    internal sealed class TWhere : TSqlCmdBase {

        private readonly bool withOldValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="TWhere" /> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="withOldValues">if set to <c>true</c> [with old values].</param>
        public TWhere(Table entity, bool withOldValues = false)
            : base(entity) {

            this.withOldValues = withOldValues;
        }

        /// <summary>
        /// Builds the CMD STR.
        /// </summary>
        /// <param name="sb">The sb.</param>
        protected override void BuildCmdStr(StringBuilder sb) {
            String mask = withOldValues ?
                " [{0}] = @Old{0} and" : // Just for update when using a table with key fields only.
                " [{0}] = @{0} and";

            foreach (var field in Table.KeyFields)
                sb.AppendFormat(mask, field.FieldName);

            int sepLength = " and".Length;
            sb.Remove(sb.Length - sepLength, sepLength);
        }
    }
}
