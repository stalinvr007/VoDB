using System;
using System.Linq.Expressions;
using VODB.Core;
using VODB.Core.Infrastructure;

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

        public ExpressionBodyParser Previous { get; private set; }

        #region IExpressionBodyParser Members

        public Object Entity { get; set; }

        public String FieldName { get; private set; }

        public Field Field { get; private set; }

        public ExpressionBodyParser Next { get; private set; }

        public ExpressionType NodeType { get; private set; }

        public Object Value { get; private set; }

        public Boolean IsComplex
        {
            get { return Next != null; }
        }

        public void Parse()
        {
            if (_Expression is BinaryExpression)
            {
                var body = (BinaryExpression) _Expression;

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

        #endregion

        private void Parse(LambdaExpression expression)
        {
            Value = expression.Compile().DynamicInvoke();
            Type valueType = Value.GetType();
            if (valueType.IsEntity())
            {
                Value = Engine.GetTable(valueType)
                    .FindField(Field.BindedTo ?? Field.FieldName).GetValue(Value);
            }
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
            Field = Engine.GetTable(Entity.GetType()).FindField(FieldName);

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
                    Previous.Field = Engine.GetTable(Field.FieldType)
                        .FindField(Previous.FieldName);
                }
            }
        }
    }
}