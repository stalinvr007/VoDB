using System.Data.Common;

namespace VODB.Core.Execution.Factories
{
    internal interface IDbCommandFactory
    {
        DbCommand Make();
    }
}