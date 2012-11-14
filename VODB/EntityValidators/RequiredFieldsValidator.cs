using System.Collections.Generic;
using System.Linq;
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
            List<Field> nonFilled = entity.Table.Fields
                .Where(field => field.IsRequired)
                .Where(field => NotFilled(field, entity))
                .ToList();

            if (nonFilled.Count > 0)
            {
                throw new ValidationException();
            }
        }

        #endregion

        private bool NotFilled(Field field, DbEntity entity)
        {
            object value = field.GetValue(entity);

            if (value == null)
            {
                return true;
            }

            return false;
        }
    }
}