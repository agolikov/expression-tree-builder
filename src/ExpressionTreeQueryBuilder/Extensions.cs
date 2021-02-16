using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeQueryBuilder
{
    public static class Extensions
    {
        public static IQueryable<T> FilterCondition<T>(this IQueryable<T> source, string filterQuery)
        {
            ExpressionFactory<T> factory = new ExpressionFactory<T>();
            return source.Where(factory.GetFilterExpressionFromString(filterQuery));
        }

        public static IQueryable<T> SortCondition<T>(this IQueryable<T> source, string sortQuery)
        {
            ExpressionFactory<T> factory = new ExpressionFactory<T>();
            var result = factory.ParseSortExpression(sortQuery);
            var property = result.property;
            ListSortDirection sortOrder = result.direction;
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var typeArguments = new Type[] { type, property.PropertyType };
            var methodName = sortOrder == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending";
            var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}