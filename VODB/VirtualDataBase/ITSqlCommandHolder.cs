
namespace VODB.VirtualDataBase
{
    internal interface ITSqlCommandHolder
    {
        string Count { get; }
        string Delete { get; }
        string Insert { get; }
        string Select { get; }
        string SelectById { get; }
        string Update { get; }
        string CountById { get; }
    }
}
