using System;

namespace VODB.Core.Infrastructure
{
    internal interface ITSqlCommandHolder
    {

        Table Table { get; set; }

        string Count { get; }
        string Delete { get; }
        string Insert { get; }
        string Select { get; }
        string SelectById { get; }
        string Update { get; }
        string CountById { get; }
    }
}
