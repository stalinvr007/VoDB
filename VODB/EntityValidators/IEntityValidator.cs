namespace VODB.EntityValidators
{

    public enum On
    {
        Insert,
        Update,
        Select,
        SelectById,
        Delete,
        Count
    }

    public interface IEntityValidator
    {

        /// <summary>
        /// Verifies if this validator should run on what commands.
        /// </summary>
        /// <param name="onCommand"></param>
        /// <returns></returns>
        bool ShouldRunOn(On onCommand);

        /// <summary>
        /// Validates the specified entity. Should throw exception with the validation result if failed.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Validate(DbEntity entity);


    }
}