using System.Data.Common;

namespace VODB.DbLayer.DbCommands
{
    internal interface IDbCommandFactory
    {
        DbCommand Make();
    }
}