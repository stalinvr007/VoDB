using System;
using System.Linq;
using System.Linq.Expressions;
using VODB.Exceptions;
using VODB.Extensions;
using VODB.VirtualDataBase;

namespace VODB.ExpressionParser
{

    internal interface IExpressionBodyParser
    {
        Entity Entity { get; set; }
        String FieldName { get; }
        Field Field { get; }
        ExpressionBodyParser BodyParser { get; }
        ExpressionType NodeType { get; }
        Object Value { get; }
        Boolean IsComplex { get; }
        void Parse();
    }

    class ExpressionBodyParser : IExpressionBodyParser
    {

        private readonly Expression _Expression;
        
        public ExpressionBodyParser(Expression expression)
        {
            _Expression = expression;
        }

        public Entity Entity { get; set; }

        public String FieldName { get; private set; }

        public Field Field { get; private set; }

        public ExpressionBodyParser BodyParser { get; private set; }

        public ExpressionType NodeType { get; private set; }

        public Object Value { get; private set; }

        public Boolean IsComplex { get { return BodyParser != null; } }

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
            

            if (typeof(Entity).IsAssignableFrom(expression.Type))
            {
                Entity = Activator.CreateInstance(expression.Type) as Entity;
                Field = Entity.FindField(FieldName);    
            }
            else
            {
                Field = Entity.FindField(FieldName);
                BodyParser = new ExpressionBodyParser(expression.Expression)
                {
                    Entity = Entity
                };
                BodyParser.Parse();
            }

            
        }
    }
}
