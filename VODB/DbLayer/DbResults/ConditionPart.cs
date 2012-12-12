using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.ExpressionParser;
using VODB.Extensions;
using VODB.VirtualDataBase;
using System.Linq;

namespace VODB.DbLayer.DbResults
{
    public class ConditionPart
    {

        public Operation Operation { get; set; }

        public String Condition { get; set; }

    }
}
