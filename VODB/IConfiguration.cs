using System.Collections.Generic;
using VODB.Core.Execution.DbParameterSetters;
using VODB.Core.Loaders.FieldSetters;
using VODB.EntityValidators;
using VODB.EntityValidators.Fields;
using VODB.Exceptions.Handling;
using VODB.ExpressionParser;
using VODB.ExpressionParser.ExpressionHandlers;
using VODB.ExpressionParser.TSqlBuilding;

namespace VODB
{
    internal interface IConfiguration
    {
        ICollection<ITSqlBuilder> TSqlBuilders { get; }

        /// <summary>
        /// Gets the where expression handlers.
        /// </summary>
        /// <value>
        /// The where expression handlers.
        /// </value>
        ICollection<IWhereExpressionHandler> WhereExpressionHandlers { get; }

        /// <summary>
        /// Gets or sets the where expression formatters.
        /// </summary>
        /// <value>
        /// The where expression formatters.
        /// </value>
        ICollection<IWhereExpressionFormatter> WhereExpressionFormatters { get; }

        /// <summary>
        /// Gets the entity validators.
        /// </summary>
        /// <value>
        /// The entity validators.
        /// </value>
        ICollection<IEntityValidator> EntityValidators { get; }

        /// <summary>
        /// Gets the field setters.
        /// </summary>
        /// <value>
        /// The field setters.
        /// </value>
        ICollection<IFieldSetter> FieldSetters { get; }

        /// <summary>
        /// Gets the parameter setters. Used to set data into DbParameters.
        /// </summary>
        /// <value>
        /// The parameter setters.
        /// </value>
        ICollection<IParameterSetter> ParameterSetters { get; }

        /// <summary>
        /// Gets the field is filled validators.
        /// </summary>
        /// <value>
        /// The field is filled validators.
        /// </value>
        ICollection<IFieldValidator> FieldIsFilledValidators { get; }

        /// <summary>
        /// Gets the exception handlers.
        /// </summary>
        /// <value>
        /// The exception handlers.
        /// </value>
        ICollection<IExceptionHandler> ExceptionHandlers { get; }
    }
}