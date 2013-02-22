using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VODB.Core.Infrastructure;

namespace VODB.Core.Loaders
{
    interface IDataReaderLoader
    {
        /// <summary>
        /// Starts the loading process.
        /// </summary>
        void Start();

        /// <summary>
        /// Indicates that the loading process has ended.
        /// </summary>
        /// <value>
        /// The has completed.
        /// </value>
        Boolean HasCompleted { get; }

        /// <summary>
        /// Fetches the next record.
        /// </summary>
        /// <param name="rowData">The row data.</param>
        /// <returns>True if the rowData contains data false if ended.</returns>
        Boolean FetchNext(out IDictionary<String, Object> rowData);

    }

    class ConcurrentDataReader : IDataReaderLoader
    {
        private readonly IDataReader _Reader;
        Thread _thread;
        private volatile int _fetchCount;
        private readonly List<IDictionary<String, Object>> _data;
        private volatile Boolean done;
        private readonly Table _EntityTable;
        private readonly ConcurrentQueue<ManualResetEventSlim> lockers = new ConcurrentQueue<ManualResetEventSlim>();

        public ConcurrentDataReader(IDataReader reader, Table entityTable)
        {
            _EntityTable = entityTable;
            _Reader = reader;
            _data = new List<IDictionary<String, Object>>();
        }

        public void Start()
        {
            if (_thread != null)
            {
                throw new InvalidOperationException();
            }

            _thread = new Thread(ProcessDataFetch);
            _thread.Start();
        }

        private void ProcessDataFetch()
        {

            while (_Reader.Read())
            {

                var row = new Dictionary<String, Object>();

                foreach (var field in _EntityTable.Fields)
                {
                    row[field.FieldName] = _Reader[field.FieldName];
                }

                _data.Add(row);

                if (lockers.Count > 0)
                {
                    ManualResetEventSlim mre;
                    while (!lockers.TryDequeue(out mre)) ;
                    mre.Set();
                }
            }

            done = true;
        }

        public bool HasCompleted
        {
            get { return _data.Count == _fetchCount && done; }
        }

        public bool FetchNext(out IDictionary<string, object> rowData)
        {
            if (HasCompleted)
            {
                rowData = null;
                return false;
            }

            var val = Interlocked.Increment(ref _fetchCount) - 1;

            if (_data.Count > val)
            {
                rowData = _data[val];
                return true;
            }

            var mre = new ManualResetEventSlim();
            lockers.Enqueue(mre);
            mre.Wait();

            rowData = _data[val];
            return true;

        }
    }
}
