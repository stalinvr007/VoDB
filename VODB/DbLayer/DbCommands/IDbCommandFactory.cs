using System.Data.Common;
using System;

namespace VODB.DbLayer.DbCommands
{
    internal interface IDbCommandFactory
    {
        DbCommand Make();
    }
}