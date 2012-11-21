using System;
using System.Collections.Generic;
using System.Text;
using VODB.VirtualDataBase;
using System.Linq;

namespace VODB.Exceptions
{
    public class WrongFieldContentException : VodbException
    {
        private const int SYSTEM_NVARCHAR_TYPE_CODE = 231;
        private const int SYSTEM_VARCHAR_TYPE_CODE = 231;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrongFieldContentException" /> class.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <param name="session">The session.</param>
        /// <param name="entity">The entity.</param>
        public WrongFieldContentException(IEnumerable<Field> fields, ISession session, DbEntity entity)
            : base(BuildErrorString(fields, session, entity))
        { }

        private static String BuildErrorString(IEnumerable<Field> fields, ISession session, DbEntity entity)
        {
            StringBuilder sb = new StringBuilder();

            var obj = session.GetById(new Sys.Objects() { Name = entity.Table.TableName });

            var stringFields = fields.Where(f => f.FieldType == typeof(String));

            foreach (var item in obj.Columns.Where(c => IsString(c)))
            {

                var field = stringFields.FirstOrDefault(f => f.FieldName.ToLower().Equals(item.Name.ToLower()));

                if (field != null && field.GetValue(entity) != null)
                {
                    if (field.GetValue(entity).ToString().Length > item.Max_Length)
                        sb.AppendFormat("O campo {2} está limitado a [{0}] caracteres mas o campo foi afectado com {1}",
                            item.Max_Length, field.GetValue(entity).ToString().Length, field.FieldName)
                        .AppendLine();
                }

            }

            return sb.ToString();
        }

        private static bool IsString(Sys.All_Columns c)
        {
            return c.System_type_id == SYSTEM_NVARCHAR_TYPE_CODE || c.System_type_id == SYSTEM_VARCHAR_TYPE_CODE;
        }
    }

    
}
