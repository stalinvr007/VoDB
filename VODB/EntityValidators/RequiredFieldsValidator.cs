using System;
using System.Linq;
using System.Text;
using VODB.Exceptions;
using VODB.VirtualDataBase;

namespace VODB.EntityValidators
{
    public class RequiredFieldsValidator : IEntityValidator
    {
        #region IEntityValidator Members

        /// <summary>
        /// Validates the specified entity. Should throw exception with the validation result if failed.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Validate(DbEntity entity)
        {
            var nonFilled = entity.Table.Fields
                .Where(field => field.IsRequired)
                .Where(field => NotFilled(field, entity))
                .Select(field => field.FieldName);

            var sb = new StringBuilder();
            foreach (var field in nonFilled)
            {
                sb.AppendFormat("[{0}], ", field);
            }

            if (sb.Length <= 0) return;
            
            sb.Remove(sb.Length - 2, 2);    
            throw new ValidationException(string.Format("Required fields not set: {{ {0} }}", sb.ToString()));
        }

        #endregion

        private static bool NotFilled(Field field, DbEntity entity)
        {
            var value = field.GetValue(entity);

            if (value == null)
            {
                return true;
            }

            if (typeof(DateTime).IsAssignableFrom(field.FieldType) && ((DateTime)value).Year == 1)
            {
                return true;
            }

            return (typeof(int).IsAssignableFrom(field.FieldType) ||
                    typeof(Double).IsAssignableFrom(field.FieldType) ||
                    typeof(float).IsAssignableFrom(field.FieldType) ||
                    typeof(Decimal).IsAssignableFrom(field.FieldType) ||
                    typeof(long).IsAssignableFrom(field.FieldType)) && value.Equals(0);
        }

        public bool ShouldRunOn(On onCommand)
        {
            return onCommand == On.Insert ||
                    onCommand == On.Update;
        }
    }
}