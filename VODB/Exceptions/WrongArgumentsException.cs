using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.ExpressionsToSql;

namespace VODB.Exceptions
{
    public class WrongArgumentsException : VodbException
    {
        public WrongArgumentsException(IQueryCondition internalQuery, Object[] args)
            : base(MakeMessage(internalQuery, args))
        {
        }

        private static String MakeMessage(IQueryCondition internalQuery, Object[] args)
        {
            var sb = new StringBuilder();

            sb.AppendLine()
              .AppendLine("QUERY:")
              .AppendLine(internalQuery.Compile())
              .AppendLine("-------------------------------------------------------------------------");

            int i = 0;
            foreach (var parameter in internalQuery.Parameters)
            {
                if (i < args.Length)
                    sb.AppendLine(MakeLine(parameter.Name, args[i++]));
                else
                    sb.AppendLine(MakeLine(parameter.Name, "none"));

            }

            return sb.ToString();
        }

        private static String MakeLine(String name, Object value)
        {
            return string.Format("{0} -> {1}", name, value);
        }
    }
}
