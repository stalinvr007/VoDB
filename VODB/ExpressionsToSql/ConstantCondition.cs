using System;
using System.Collections.Generic;
using System.Text;

namespace VODB.ExpressionsToSql
{

    abstract class ParameterLessCondition : IQueryCondition
    {
        static IEnumerable<IQueryParameter> PARAMETERS = new List<IQueryParameter>();
        public abstract string Compile(ref int level);

        public IEnumerable<IQueryParameter> Parameters
        {
            get { return PARAMETERS; }
        }
    }

    class StubCondition : ParameterLessCondition
    {
        const String EMPTY_CONDITION = "";

        public override string Compile(ref int level)
        {
            return EMPTY_CONDITION;
        }

    }

    class ConstantCondition : ParameterLessCondition
    {
        private readonly String _Condition;
        public ConstantCondition(String condition)
        {
            _Condition = condition;
        }

        public override string Compile(ref int level)
        {
            return _Condition;
        }

    }

    class ParameterCondition : ParameterLessCondition
    {
        private readonly String _Condition;
        public ParameterCondition(String condition)
        {
            _Condition = condition;
        }

        public override string Compile(ref int level)
        {
            return _Condition + ++level;
        }

    }
}