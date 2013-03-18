using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;

namespace VODB.TableToSql
{

    public enum SqlBuilderType
    {
        Select = 0,
        SelectById = 1,
        Update = 3,
        Insert = 4,
        Delete = 5,
        Count = 6,
        CountById = 7,
        WhereById = 8
    }

    /// <summary>
    /// Builds a SQL Command.
    /// </summary>
    public interface ISqlBuilder
    {

        /// <summary>
        /// Gets the type of the builder.
        /// </summary>
        /// <value>
        /// The type of the builder.
        /// </value>
        SqlBuilderType BuilderType { get; }

        /// <summary>
        /// Builds the SQL for the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        String Build(ITable table);
    }
}