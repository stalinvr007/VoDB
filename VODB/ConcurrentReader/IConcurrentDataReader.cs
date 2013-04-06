using System.Collections.Generic;
using System.Data;

namespace VODB.ConcurrentReader
{
    public interface IConcurrentDataReader : IDataReader
    {
        ITuple GetData();
        IEnumerable<ITuple> GetTuples();
    }


}
