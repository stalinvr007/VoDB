using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.EntityValidators
{
    public interface IEntityValidator
    {

        /// <summary>
        /// Validates the specified entity. Should throw exception with the validation result if failed.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Validate(DbEntity entity);

    }

}
