using System;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.Executers.DbResults
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
