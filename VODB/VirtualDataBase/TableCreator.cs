using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VODB.VirtualDataBase
{
    /// <summary>
    /// Creates Tables with fields.
    /// Convention based and property Annotations to find out the table fields of an Entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    internal sealed class TableCreator<TEntity> : ITableCreator
    {

        Type _EntityType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCreator{TEntity}" /> class.
        /// </summary>
        public TableCreator()
        {
            _EntityType = typeof(TEntity);
        }

        public Table Create()
        {
            var table = new Table();

            var thread = new Thread(() =>
            {
                var t1 = new Thread(() => table.TableName = GetTableName(_EntityType));
                var t2 = new Thread(() => table.Fields = GetTableFields(_EntityType));
                var t3 = new Thread(() => table.KeyFields = GetTableKeyFields(_EntityType));

                t3.Start();
                t2.Start();
                t1.Start();                

                t1.Join();
                t2.Join();
                t3.Join();
            });

            thread.Start();
            thread.Join();

            return table;
        }

        private static String GetTableName(Type type)
        {
            /* Todo: Check for annotations and get name from one if any. Else its the name of the type. */
            return type.Name;
        }

        private static IEnumerable<Field> GetTableFields(Type type)
        {

            return type.GetProperties()
                .Select(i => new Field
                {
                    FieldName = i.Name,
                    FieldType = i.PropertyType
                })
                .ToList();

        }

        private static IEnumerable<Field> GetTableKeyFields(Type type)
        {
            return GetTableFields(type).Where(f => f.IsKey);
        }



    }
}
