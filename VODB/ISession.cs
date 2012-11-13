using System;
using System.Data.Common;
namespace VODB
{

    interface ISession
    {
        Transaction BeginTransaction();
    }

    interface ISessionInternal
    {
        DbCommand CreateCommand();
    }
}
