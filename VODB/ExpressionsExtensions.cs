using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VODB
{
    internal static class ExpressionsExtensions
    {
        public static Boolean HasParameters(this LambdaExpression expression)
        {
            return expression.Body is BinaryExpression;
        }

        public static ExpressionType NodeType(this LambdaExpression expression)
        {
            return expression.Body.NodeType;
        }

        public static Object GetRightValue(this LambdaExpression expression)
        {
            var exp = expression.Body as BinaryExpression;

            if (exp == null)
            {
                return null;
            }

            var constantExpression = exp.Right as ConstantExpression;
            if (constantExpression != null)
            {
                return constantExpression.Value;
            }

            return Expression.Lambda(exp.Right).Compile().DynamicInvoke();
        }

    }
}
