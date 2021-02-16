using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeQueryBuilder
{
    public class ExpressionFactory<T>
    {
        private readonly ExpressionBuilder<T> _builder;
        private readonly ExpressionConverter _converter;
        public ExpressionFactory()
        {
            _converter = new ExpressionConverter(ExpressionParameters.Priorities);
            _builder = new ExpressionBuilder<T>(ExpressionParameters.Functions);
        }

        public Expression<Func<T, bool>> GetFilterExpressionFromString(string infixQuery)
        {
            string postfix = _converter.ConvertInfixToPostfix(infixQuery);

            return _builder.Build(postfix);
        }

        public (PropertyInfo property, ListSortDirection direction) ParseSortExpression(string sortExpression)
        {
            var propertyItems = sortExpression.Split(" ");
            if (propertyItems.Length != 2)
            {
                throw new Exception("Sort expression is not valid.");
            }


            var property = this.GetType().GetGenericArguments()[0].GetProperty(propertyItems[0],
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
            {
                throw new Exception("SortProperty not found");
            }

            ListSortDirection? direction = null;

            if (String.Compare(propertyItems[1], "asc", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                direction = ListSortDirection.Ascending;
            }
            if (String.Compare(propertyItems[1], "desc", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                direction = ListSortDirection.Descending;
            }
            if (direction == null)
            {
                throw new Exception("Direction is not valid.");
            }

            return (property, direction.Value);
        }
    }
}
