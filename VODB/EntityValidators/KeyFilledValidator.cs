using System.Linq;
using System.Text;
using VODB.Exceptions;
using VODB.Extensions;

namespace VODB.EntityValidators
{
    class KeyFilledValidator : IEntityValidator
    {
        public bool ShouldRunOn(On onCommand)
        {
            return onCommand == On.SelectById ||
                onCommand == On.Delete;
        }

        public void Validate(Entity entity)
        {
            var nonFilled = entity.Table.KeyFields
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
                string.Format("Key fields not set: {{ {0} }}", sb), 
                entity.Table.KeyFields.Where(field => !entity.IsFilled(field)));
        }
    }
}
