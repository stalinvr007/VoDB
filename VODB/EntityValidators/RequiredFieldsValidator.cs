using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core;
using VODB.Exceptions;

namespace VODB.EntityValidators
{
    public class RequiredFieldsValidator : IEntityValidator
    {
        #region IEntityValidator Members

        /// <summary>
        /// Validates the specified entity. Should throw exception with the validation result if failed.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Validate<TEntity>(TEntity entity)
        {
            IEnumerable<string> nonFilled = Engine.GetTable(entity.GetType()).Fields
                .Where(field => field.IsRequired)
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
                string.Format("Required fields not set: {{ {0} }}", sb),
                Engine.GetTable(entity.GetType()).Fields.Where(field => field.IsRequired).Where(
                    field => !entity.IsFilled(field)));
        }

        public bool ShouldRunOn(On onCommand)
        {
            return onCommand == On.Insert ||
                   onCommand == On.Update;
        }

        #endregion
    }
}