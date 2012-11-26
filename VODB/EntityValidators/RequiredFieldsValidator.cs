using System.Linq;
using System.Text;
using VODB.Exceptions;
using VODB.Extensions;

namespace VODB.EntityValidators
{
    public class RequiredFieldsValidator : IEntityValidator
    {
        #region IEntityValidator Members

        /// <summary>
        /// Validates the specified entity. Should throw exception with the validation result if failed.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Validate(Entity entity)
        {
            var nonFilled = entity.Table.Fields
                .Where(field => field.IsRequired)
                .Where(field => !entity.IsFilled(field))
                .Select(field => field.FieldName);

            var sb = new StringBuilder();
            foreach (var field in nonFilled)
            {
                sb.AppendFormat("[{0}], ", field);
            }

            if (sb.Length <= 0) return;
            
            sb.Remove(sb.Length - 2, 2);
            throw new ValidationException(
                string.Format("Required fields not set: {{ {0} }}", sb), 
                entity.Table.Fields.Where(field => field.IsRequired).Where(field => !entity.IsFilled(field)));
        }

        #endregion
        
        public bool ShouldRunOn(On onCommand)
        {
            return onCommand == On.Insert ||
                    onCommand == On.Update;
        }
    }
}