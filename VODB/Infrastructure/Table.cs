using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.DbLayer;

namespace VODB.Infrastructure
{
    class Table : ITable
    {
        private const string PARAMETER_PREFIX = "@";
        private const string PARAMETER_PREFIX_OLD = "@old";

        private IVodbCommand _InsertCommand;
        private IVodbCommand _UpdateCommand;
        private IVodbCommand _DeleteCommand;
        private IVodbCommand _CountByIdCommand;
        private IVodbCommand _CountCommand;
        private IVodbCommand _SelectByIdCommand;
        private IVodbCommand _SelectCommand;

        public Table(String name)
        {
            Name = name;
        }

        public String Name { get; private set; }

        public Type EntityType { get; internal set; }

        public IEnumerable<IField> Fields { get; internal set; }

        public IEnumerable<IField> Keys { get; internal set; }

        public override string ToString()
        {
            return Name;
        }

        public string SqlSelect { get; internal set; }

        public string SqlSelectById { get; internal set; }

        public string SqlCount { get; internal set; }

        public string SqlCountById { get; internal set; }

        public string SqlDeleteById { get; internal set; }

        public string SqlInsert { get; internal set; }

        public string SqlUpdate { get; internal set; }

        public IField IdentityField { get; internal set; }

        public void SetIdentityValue(object entity, object value)
        {
            if (IdentityField != null)
            {
                IdentityField.SetFieldFinalValue(entity, value);
            }
        }


        private void SetKeysAsParameters(String prefix, IVodbCommand command)
        {
            command.SetParametersNames(Keys.Select(k => GetParameterName(prefix, k)).ToArray());
        }

        private void SetFieldsAsParameters(String prefix, IVodbCommand command)
        {
            command.SetParametersNames(Fields.Where(f => !f.IsIdentity).Select(k => GetParameterName(prefix, k)).ToArray());
        }

        private String GetParameterName(String prefix, IField field)
        {
            return PARAMETER_PREFIX + field.Name.Replace(" ", "");
        }

        public IVodbCommand GetSelectAllCommand(IVodbCommandFactory factory)
        {
            if (_SelectCommand != null)
            {
                return _SelectCommand;
            }

            return _SelectCommand = factory.MakeCommand(SqlSelect);
        }

        public IVodbCommand GetSelectByIdCommand(IVodbCommandFactory factory)
        {
            if (_SelectByIdCommand != null)
            {
                return _SelectByIdCommand;
            }

            _SelectByIdCommand = factory.MakeCommand(SqlSelectById);
            SetKeysAsParameters(PARAMETER_PREFIX, _SelectByIdCommand);

            return _SelectByIdCommand;
        }

        public IVodbCommand GetCountCommand(IVodbCommandFactory factory)
        {
            if (_CountCommand != null)
            {
                return _CountCommand;
            }

            return _CountCommand = factory.MakeCommand(SqlCount);
        }

        public IVodbCommand GetCountByIdCommand(IVodbCommandFactory factory)
        {
            if (_CountByIdCommand != null)
            {
                return _CountByIdCommand;
            }

            _CountByIdCommand = factory.MakeCommand(SqlCountById);
            SetKeysAsParameters(PARAMETER_PREFIX, _CountByIdCommand);

            return _CountByIdCommand;
        }

        public IVodbCommand GetDeleteCommand(IVodbCommandFactory factory)
        {
            if (_DeleteCommand != null)
            {
                return _DeleteCommand;
            }

            _DeleteCommand = factory.MakeCommand(SqlDeleteById);
            SetKeysAsParameters(PARAMETER_PREFIX, _DeleteCommand);

            return _DeleteCommand;
        }

        public IVodbCommand GetInsertCommand(IVodbCommandFactory factory)
        {
            if (_InsertCommand != null)
            {
                return _InsertCommand;
            }

            _InsertCommand = factory.MakeCommand(SqlInsert);
            SetFieldsAsParameters(PARAMETER_PREFIX, _InsertCommand);

            return _InsertCommand;
        }

        public IVodbCommand GetUpdateCommand(IVodbCommandFactory factory)
        {
            if (_UpdateCommand != null)
            {
                return _UpdateCommand;
            }

            _UpdateCommand = factory.MakeCommand(SqlInsert);
            SetFieldsAsParameters(PARAMETER_PREFIX, _UpdateCommand);
            SetKeysAsParameters(PARAMETER_PREFIX_OLD, _UpdateCommand);

            return _UpdateCommand;
        }
    }
}
