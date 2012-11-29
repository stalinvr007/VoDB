using System.Collections.Generic;
using VODB.DbLayer.DbCommands.DbParameterSetters;
using VODB.DbLayer.Loaders.FieldSetters;
using VODB.EntityValidators;
using VODB.EntityValidators.Fields;
using VODB.Exceptions.Handling;
using VODB.ExpressionParser;
using VODB.ExpressionParser.ExpressionHandlers;
using System;

namespace VODB
{
    /// <summary>
    /// Allows the end user to configure some aspects of the VODB Framework.
    /// </summary>
    public static class Configuration
    {
        
        static Configuration()
        {

            FieldIsFilledValidators = new List<IFieldValidator>
            {
                new StringFieldIsFilled(),
                new NumberFieldIsFilled(),
                new DateTimeFieldIsFilled(),
                new DbEntityFieldIsFilled(),
                new RefTypeFieldIsFilled()
            };

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
                new DecimalParameterSetter(),
                new ByteArrayParameterSetter()
            };

            ExceptionHandlers = new List<IExceptionHandler>
            {
                new PrimaryKeyExceptionHandler(),
                new UniqueKeyExceptionHandler()
            };


            WhereExpressionHandlers = new List<IWhereExpressionHandler>
            {
                new SimpleWhereExpressionHandler()
            };

            WhereExpressionFormatters = new List<IWhereExpressionFormatter>
            {
                new EqualityWhereExpressionFormatter(),
                new NonEqualityWhereExpressionFormatter(),
                new GreaterOrEqualWhereExpressionFormatter(),
                new SmallerOrEqualWhereExpressionFormatter(),
                new SmallerWhereExpressionFormatter(),
                new GreaterWhereExpressionFormatter()
            };

        }

        /// <summary>
        /// Gets the where expression handlers.
        /// </summary>
        /// <value>
        /// The where expression handlers.
        /// </value>
        public static ICollection<IWhereExpressionHandler> WhereExpressionHandlers { get; private set; }

        /// <summary>
        /// Gets or sets the where expression formatters.
        /// </summary>
        /// <value>
        /// The where expression formatters.
        /// </value>
        public static ICollection<IWhereExpressionFormatter> WhereExpressionFormatters { get; private set; }

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

        /// <summary>
        /// Gets the field is filled validators.
        /// </summary>
        /// <value>
        /// The field is filled validators.
        /// </value>
        public static ICollection<IFieldValidator> FieldIsFilledValidators { get; private set; }

        /// <summary>
        /// Gets the exception handlers.
        /// </summary>
        /// <value>
        /// The exception handlers.
        /// </value>
        public static ICollection<IExceptionHandler> ExceptionHandlers { get; private set; }

    }
}