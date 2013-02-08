using System;
using System.Linq.Expressions;

namespace VODB.ExpressionParser
{
    internal class EqualityWhereExpressionFormatter : IWhereExpressionFormatter
    {
        #region IWhereExpressionFormatter Members

        public String Format(String fieldName, String parameterName)
        {
            return String.Format("{0} = @{1}", fieldName, parameterName);
        }

        public bool CanFormat(ExpressionType type)
        {
            return type == ExpressionType.Equal;
        }

        #endregion
    }

    internal class NonEqualityWhereExpressionFormatter : IWhereExpressionFormatter
    {
        #region IWhereExpressionFormatter Members

        public String Format(String fieldName, String parameterName)
        {
            return String.Format("{0} != @{1}", fieldName, parameterName);
        }

        public bool CanFormat(ExpressionType type)
        {
            return type == ExpressionType.NotEqual;
        }

        #endregion
    }

    internal class GreaterWhereExpressionFormatter : IWhereExpressionFormatter
    {
        #region IWhereExpressionFormatter Members

        public String Format(String fieldName, String parameterName)
        {
            return String.Format("{0} > @{1}", fieldName, parameterName);
        }

        public bool CanFormat(ExpressionType type)
        {
            return type == ExpressionType.GreaterThan;
        }

        #endregion
    }

    internal class GreaterOrEqualWhereExpressionFormatter : IWhereExpressionFormatter
    {
        #region IWhereExpressionFormatter Members

        public String Format(String fieldName, String parameterName)
        {
            return String.Format("{0} >= @{1}", fieldName, parameterName);
        }

        public bool CanFormat(ExpressionType type)
        {
            return type == ExpressionType.GreaterThanOrEqual;
        }

        #endregion
    }

    internal class SmallerWhereExpressionFormatter : IWhereExpressionFormatter
    {
        #region IWhereExpressionFormatter Members

        public String Format(String fieldName, String parameterName)
        {
            return String.Format("{0} < @{1}", fieldName, parameterName);
        }

        public bool CanFormat(ExpressionType type)
        {
            return type == ExpressionType.LessThan;
        }

        #endregion
    }

    internal class SmallerOrEqualWhereExpressionFormatter : IWhereExpressionFormatter
    {
        #region IWhereExpressionFormatter Members

        public String Format(String fieldName, String parameterName)
        {
            return String.Format("{0} <= @{1}", fieldName, parameterName);
        }

        public bool CanFormat(ExpressionType type)
        {
            return type == ExpressionType.LessThanOrEqual;
        }

        #endregion
    }
}