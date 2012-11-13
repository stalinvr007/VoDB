using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Exceptions;
using VODB.VirtualDataBase;

namespace VODB.EntityValidators
{
    public class RequiredFieldsValidator : IEntityValidator
    {
        /// <summary>
        /// Validates the specified entity. Should throw exception with the validation result if failed.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Validate(DbEntity entity)
        {

            var nonFilled = entity.Table.Fields
                            .Where(field => field.IsRequired)
                            .Where(field => NotFilled(field, entity))
                            .ToList();

            if (nonFilled.Count > 0)
            {
                throw new ValidationException();
            }
        }
        
        private bool NotFilled(Field field, DbEntity entity)
        {
            var value = field.GetValue(entity);

            if (value == null)
            {
                return true;
            }

            return false;
        }

    }
}
