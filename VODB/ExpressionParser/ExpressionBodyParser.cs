using System;
using System.Linq;
using System.Linq.Expressions;
using VODB.Exceptions;

namespace VODB.ExpressionParser
{
    class ExpressionBodyParser
    {

        public Entity Entity { get; set; }

        public String FieldName { get; private set; }

        public ExpressionBodyParser BodyParser { get; private set; }

        public void Parse(BinaryExpression expression)
        {            
            Parse(expression.Left as MemberExpression);
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
                var field = Entity.Table.Fields
                    .FirstOrDefault(f => f.FieldName.Equals(FieldName));

                if (field != null)
                {
                    Entity = field.GetValue(Entity) as Entity;
                }
            }
            else
            {
                BodyParser = new ExpressionBodyParser();
                BodyParser.Parse(expression.Expression as MemberExpression);
            }
        }
    }
}
