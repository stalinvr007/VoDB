using System;
using System.Linq.Expressions;

namespace VODB.ExpressionParser
{
    internal class EqualityWhereExpressionFormatter : IWhereExpressionFormatter
    {
        public String Format(String fieldName, String parameterName)
        {
            return String.Format("{0} = @{1}", fieldName, parameterName);
        }
        public bool CanFormat(ExpressionType type)
        {
            return type == ExpressionType.Equal;
        }        
    }

    internal class NonEqualityWhereExpressionFormatter : IWhereExpressionFormatter
    {
        public String Format(String fieldName, String parameterName)
        {
            return String.Format("{0} != @{1}", fieldName, parameterName);
        }
        public bool CanFormat(ExpressionType type)
        {
            return type == ExpressionType.NotEqual;
        }
    }

    internal class GreaterWhereExpressionFormatter : IWhereExpressionFormatter
    {
        public String Format(String fieldName, String parameterName)
        {
            return String.Format("{0} > @{1}", fieldName, parameterName);
        }
        public bool CanFormat(ExpressionType type)
        {
            return type == ExpressionType.GreaterThan;
        }
    }

    internal class GreaterOrEqualWhereExpressionFormatter : IWhereExpressionFormatter
    {
        public String Format(String fieldName, String parameterName)
        {
            return String.Format("{0} >= @{1}", fieldName, parameterName);
        }
        public bool CanFormat(ExpressionType type)
        {
            return type == ExpressionType.GreaterThanOrEqual;
        }
    }

    internal class SmallerWhereExpressionFormatter : IWhereExpressionFormatter
    {
        public String Format(String fieldName, String parameterName)
        {
            return String.Format("{0} < @{1}", fieldName, parameterName);
        }
        public bool CanFormat(ExpressionType type)
        {
            return type == ExpressionType.LessThan;
        }
    }

    internal class SmallerOrEqualWhereExpressionFormatter : IWhereExpressionFormatter
    {
        public String Format(String fieldName, String parameterName)
        {
            return String.Format("{0} <= @{1}", fieldName, parameterName);
        }
        public bool CanFormat(ExpressionType type)
        {
            return type == ExpressionType.LessThanOrEqual;
        }
    }
}
