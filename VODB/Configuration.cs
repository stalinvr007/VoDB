using System.Collections.Generic;
using VODB.DbLayer.DbCommands.DbParameterSetters;
using VODB.DbLayer.Loaders.FieldSetters;
using VODB.EntityValidators;

namespace VODB
{
    /// <summary>
    /// Allows the end user to configure some aspects of the VODB Framework.
    /// </summary>
    public static class Configuration
    {
        static Configuration()
        {
            EntityValidators = new List<IEntityValidator>
                                   {
                                       new RequiredFieldsValidator(),
                                       new KeyFilledValidator()
                                   };

            FieldSetters = new List<IFieldSetter>
                               {
                                   new BasicFieldSetter(),
                                   new DbEntityFieldSetter()
                               };

            ParameterSetters = new List<IParameterSetter>
                                   {
                                       new BasicParameterSetter(),
                                       new DbEntityParameterSetter(),
                                       new DateTimeParameterSetter(),
                                       new ByteArrayParameterSetter()
                                   };
        }

        /// <summary>
        /// Gets the entity validators.
        /// </summary>
        /// <value>
        /// The entity validators.
        /// </value>
        public static ICollection<IEntityValidator> EntityValidators { get; private set; }

        /// <summary>
        /// Gets the field setters.
        /// </summary>
        /// <value>
        /// The field setters.
        /// </value>
        public static ICollection<IFieldSetter> FieldSetters { get; private set; }

        /// <summary>
        /// Gets the parameter setters. Used to set data into DbParameters.
        /// </summary>
        /// <value>
        /// The parameter setters.
        /// </value>
        public static ICollection<IParameterSetter> ParameterSetters { get; private set; }
    }
}