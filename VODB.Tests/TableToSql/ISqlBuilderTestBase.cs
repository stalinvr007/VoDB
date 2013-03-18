using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.EntityTranslation;
using VODB.Infrastructure;
using VODB.TableToSql;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests.TableToSql
{
    public abstract class ISqlBuilderTestBase<TEntityTranslator, TSqlBuilder>
        where TEntityTranslator : IEntityTranslator, new()
        where TSqlBuilder : ISqlBuilder, new()
    {

        private static IEntityTranslator translator = 
            new CachingTranslator(new TEntityTranslator());

        private static IEnumerable GetTables(ISqlBuilder builder)
        {
            return Utils.TestModels
                .ToTables(translator)
                .Select(t => new TestCaseData(builder, t));
        }

        protected IEnumerable GetTables()
        {
            return GetTables(new TSqlBuilder());
        }

    }
}
