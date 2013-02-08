using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core;
using VODB.Exceptions;

namespace VODB.EntityValidators
{
    internal class KeyFilledValidator : IEntityValidator
    {
        #region IEntityValidator Members

        public bool ShouldRunOn(On onCommand)
        {
            return onCommand == On.SelectById ||
                   onCommand == On.Delete;
        }

        public void Validate<TEntity>(TEntity entity)
        {
            IEnumerable<string> nonFilled = Engine.GetTable(entity.GetType()).KeyFields
                .Where(field => !entity.IsFilled(field))
                .Select(field => field.FieldName);

            var sb = new StringBuilder();
            foreach (string field in nonFilled)
            {
                sb.AppendFormat("[{0}], ", field);
            }

            if (sb.Length <= 0) return;

            sb.Remove(sb.Length - 2, 2);
            throw new ValidationException(
                string.Format("Key fields not set: {{ {0} }}", sb),
                Engine.GetTable(entity.GetType()).KeyFields.Where(field => !entity.IsFilled(field)));
        }

        #endregion
    }
}