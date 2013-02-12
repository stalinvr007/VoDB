using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VODB.Core.Infrastructure;

namespace VODB.Core.Loaders
{
    internal class DictionaryMapper : IDictionaryMapper
    {

        private readonly IInternalSession _Session;
        public DictionaryMapper(IInternalSession session)
        {
            _Session = session;
        }

        #region Implementation of IDictionaryMapper

        public Task<TEntity> Map<TEntity>(IDictionary<string, object> data, Table entityTable, TEntity entity)
        {
            return Task<TEntity>.Factory.StartNew(() =>
            {

                Parallel.ForEach(entityTable.Fields, field =>
                {
                    entity.SetValue(_Session, field, data[field.FieldName], f => data[f.FieldName]);
                });

                return entity;
            });
        }

        #endregion
    }
}
