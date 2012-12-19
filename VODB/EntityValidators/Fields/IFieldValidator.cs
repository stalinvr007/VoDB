using System;
using VODB.Core.Infrastructure;

namespace VODB.EntityValidators.Fields
{
    /// <summary>
    /// Represents a field validator. Avaliates a field and returns true or false.
    /// </summary>
    public interface IFieldValidator
    {

        Boolean CanHandle(Field field);

        Boolean Verify(Field field, Entity entity);

    }
}
