﻿using System;
using System.Data.Common;
using VODB.DbLayer.DbExecuters;

namespace VODB.DbLayer.DbResults
{
    internal sealed class DbQueryScalarExecuter<TResult> : ICommandExecuter<TResult>
    {
        private readonly DbCommand _Command;

        /// <summary>
        /// Initializes a new instance of the <see>
        ///                                     <cref>DbQueryEagerExecuter</cref>
        ///                                   </see>  class.
        /// </summary>
        /// <param name="command">The command.</param>
        public DbQueryScalarExecuter(DbCommand command)
        {
            _Command = command;            
        }
        public TResult Execute()
        {
            return  (TResult)Convert.ChangeType(_Command.ExecuteScalar(), typeof(TResult));
        }
    }
}