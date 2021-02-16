using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeQueryBuilder
{
    class ExpressionBuilder<T>
    {
        private readonly Dictionary<string, PropertyInfo> _availableProperties;
        private readonly Dictionary<string, Func<Expression, Expression, Expression>> _availableOperations;

        public ExpressionBuilder(Dictionary<string, Func<Expression, Expression, Expression>> availableOperationImpls)
        {
            _availableProperties = typeof(T).GetProperties().ToDictionary(t => t.Name.ToLower(), t => t);
            _availableOperations = availableOperationImpls;
        }

        private bool IsOperation(string token)
        {
            return _availableOperations.ContainsKey(token);
        }

        public Expression<Func<T, bool>> Build(string postfixQuery)
        {
            string[] tokens = postfixQuery.TrimEnd().Split(' ');
            Stack<Expression> stack = new Stack<Expression>();
            var param = Expression.Parameter(typeof(T), "x");

            bool parsedMember = false;
            foreach (var t in tokens)
            {
                if (!IsOperation(t))
                {
                    if (!parsedMember)
                    {
                        if (!_availableProperties.ContainsKey(t))
                        {
                            throw new Exception("Property not found.");
                        }

                        var member = Expression.Property(param, t);
                        stack.Push(member);
                        parsedMember = true;
                    }
                    else
                    {
                        var memberExpression = (MemberExpression) stack.Peek();
                        var propertyType = ((PropertyInfo) memberExpression.Member).PropertyType;
                        var converter = TypeDescriptor.GetConverter(propertyType);
                        if (!converter.CanConvertFrom(typeof(string)))
                            throw new NotSupportedException();

                        var propertyValue = converter.ConvertFromInvariantString(t);
                        var constant = Expression.Constant(propertyValue);
                        var valueExpression = Expression.Convert(constant, propertyType);

                        stack.Push(valueExpression);
                        parsedMember = false;
                    }
                }
                else
                {
                    Expression right = stack.Pop();
                    Expression left = stack.Pop();
                    stack.Push(_availableOperations[t](left, right));
                }
            }

            return Expression.Lambda<Func<T, bool>>(body: stack.Pop(), param);
        }
    }
}