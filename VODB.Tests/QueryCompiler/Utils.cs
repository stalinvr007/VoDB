using NUnit.Framework;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Linq;
using VODB.Expressions;
using VODB.Tests.Models.Northwind;
using System.Collections.Generic;
using VODB.EntityTranslation;
using VODB.Exceptions;

namespace VODB.Tests.QueryCompiler
{
    internal static class Utils
    {
        private static IEntityTranslator _Translator = new EntityTranslator();

        public static IEntityTranslator Translator
        {
            get
            {
                return _Translator;
            }
        }

        public static IExpressionPiece MakePiece<TEntity>(String propertyName)
        {
            var type = typeof(TEntity);
            var table = Translator.Translate(type);

            return new ExpressionPiece
            {
                EntityType = type,
                EntityTable = table,
                PropertyName = propertyName,
                Field = table.Fields.First(f => f.Info.Name == propertyName)
            };
        }
    }
}
