using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;

namespace VODB.DbLayer
{

    
    public interface IDbParameterFactory
    {
        DbParameter CreateParameter(DbCommand cmd, IField field, Object entity);
    }

    abstract class DbParameterFactoryBase : IDbParameterFactory
    {
        private readonly string PREFIX;

        public DbParameterFactoryBase(String prefix)
        {
            PREFIX = prefix;
        }

        public DbParameter CreateParameter(DbCommand cmd, IField field, Object entity)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = PREFIX + field.Name;
            param.Value = field.GetFieldFinalValue(entity);

            if (param.Value == null)
            {
                param.Value = DBNull.Value;
            }

            return param;
        }
    }

    class DbOldParameterFactory : DbParameterFactoryBase
    {
        private const string PREFIX = "@old";

        public DbOldParameterFactory() : base(PREFIX) { }
    }

    class DbParameterFactory : DbParameterFactoryBase
    {
        private const string PREFIX = "@";

        public DbParameterFactory() : base(PREFIX) { }
    }

}
