using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.DbLayer;

namespace VODB.Infrastructure
{
    /// <summary>
    /// Represents a Data Table.
    /// </summary>
    public interface ITable
    {

        /// <summary>
        /// Gets the table name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        String Name { get; }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        Type EntityType { get;}

        /// <summary>
        /// Gets the SQL select.
        /// </summary>
        /// <value>
        /// The SQL select.
        /// </value>
        String SqlSelect { get; }

        /// <summary>
        /// Gets the SQL select by id.
        /// </summary>
        /// <value>
        /// The SQL select by id.
        /// </value>
        String SqlSelectById { get; }

        /// <summary>
        /// Gets the SQL count.
        /// </summary>
        /// <value>
        /// The SQL count.
        /// </value>
        String SqlCount { get; }

        /// <summary>
        /// Gets the SQL count by id.
        /// </summary>
        /// <value>
        /// The SQL count by id.
        /// </value>
        String SqlCountById { get; }

        /// <summary>
        /// Gets the SQL delete by id.
        /// </summary>
        /// <value>
        /// The SQL delete by id.
        /// </value>
        String SqlDeleteById { get; }

        /// <summary>
        /// Gets the SQL insert.
        /// </summary>
        /// <value>
        /// The SQL insert.
        /// </value>
        String SqlInsert { get; }

        /// <summary>
        /// Gets the SQL update.
        /// </summary>
        /// <value>
        /// The SQL update.
        /// </value>
        String SqlUpdate { get; }

        /// <summary>
        /// Gets the identity field.
        /// </summary>
        /// <value>
        /// The identity field.
        /// </value>
        IField IdentityField { get; }

        /// <summary>
        /// Sets the identity value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="value">The value.</param>
        void SetIdentityValue(Object entity, Object value);

        /// <summary>
        /// Gets the table fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        IEnumerable<IField> Fields { get; }

        /// <summary>
        /// Gets the table keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        IEnumerable<IField> Keys { get; }


        /// <summary>
        /// Gets the select all command.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        IVodbCommand GetSelectAllCommand(IVodbCommandFactory factory);
        /// <summary>
        /// Gets the select by id command.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        IVodbCommand GetSelectByIdCommand(IVodbCommandFactory factory);
        /// <summary>
        /// Gets the count command.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        IVodbCommand GetCountCommand(IVodbCommandFactory factory);
        /// <summary>
        /// Gets the count by id command.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        IVodbCommand GetCountByIdCommand(IVodbCommandFactory factory);
        /// <summary>
        /// Gets the delete command.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        IVodbCommand GetDeleteCommand(IVodbCommandFactory factory);
        /// <summary>
        /// Gets the insert command.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        IVodbCommand GetInsertCommand(IVodbCommandFactory factory);
        /// <summary>
        /// Gets the update command.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        IVodbCommand GetUpdateCommand(IVodbCommandFactory factory);
        

    }

}
