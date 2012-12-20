using System;
using System.Linq;
using System.Linq.Expressions;
using VODB.Exceptions;
using VODB.Extensions;
using VODB.Core.Infrastructure;
using VODB.Core;

namespace VODB.ExpressionParser
{

    public interface IExpressionBodyParser
    {
        Object Entity { get; set; }
        String FieldName { get; }
        Field Field { get; }
        ExpressionBodyParser Next { get; }
        ExpressionType NodeType { get; }
        Object Value { get; }
        Boolean IsComplex { get; }
        void Parse();
    }

    public class ExpressionBodyParser : IExpressionBodyParser
    {

        private readonly Expression _Expression;

        public ExpressionBodyParser(Expression expression)
        {
            _Expression = expression;
        }

        public Object Entity { get; set; }

        public String FieldName { get; private set; }

        public Field Field { get; private set; }

        public ExpressionBodyParser Next { get; private set; }

        public ExpressionBodyParser Previous { get; private set; }

        public ExpressionType NodeType { get; private set; }

        public Object Value { get; private set; }

        public Boolean IsComplex
        {
            get
            {
                return Next != null;
            }
        }

        public void Parse()
        {
            if (_Expression is BinaryExpression)
            {
                BinaryExpression body = (BinaryExpression)_Expression;

                NodeType = body.NodeType;

                Parse(body.Left as MemberExpression);

                if (body.Right is ConstantExpression)
                {
                    Parse(body.Right as ConstantExpression);
                }
                else
                {
                    Parse(Expression.Lambda(body.Right));
                }
            }
            else if (_Expression is MemberExpression)
            {
                Parse(_Expression as MemberExpression);
            }
        }
        private void Parse(LambdaExpression expression)
        {
            Value = expression.Compile().DynamicInvoke();
        }
        private void Parse(ConstantExpression expression)
        {
            Value = expression.Value;
        }
        private void Parse(MemberExpression expression)
        {
            if (expression == null)
            {
                return;
            }

            FieldName = expression.Member.Name;
            Field = Entity.FindField(FieldName);

            if (!Engine.IsMapped(expression.Type))
            {
                if (expression.Expression.NodeType != ExpressionType.Parameter)
                {
                    Next = new ExpressionBodyParser(expression.Expression)
                    {
                        Entity = Entity,
                        Previous = this
                    };
                    Next.Parse();
                }
            }
            else
            {
                if (Previous != null && Previous.Field == null)
                {
                    Previous.Field = Activator.CreateInstance(Field.FieldType)
                                              .FindField(Previous.FieldName);
                }
            }


        }
    }
}
