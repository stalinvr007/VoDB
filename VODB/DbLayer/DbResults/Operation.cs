using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.ExpressionParser;
using VODB.Extensions;
using VODB.Infrastructure;
using System.Linq;

namespace VODB.DbLayer.DbResults
{

    internal static class Operations
    {
        private static String[] strings = new[] 
        {
            " And ",
            " Or ",
            " Where ",
            " Like ",
            " Order By ",
            " Desc "
        };

        public static String GetString(this Operation operation)
        {
            if ((int)operation < 0)
            {
                return null;
            }

            return strings[(int)operation];
        }

    }

    public enum Operation
    {
        Between = -2,
        In = -1,
        
        And = 0,
        Or = 1,
        Where = 2,
        Like = 3,
        OrderBy = 4,
        Desc = 5        

    }
}
