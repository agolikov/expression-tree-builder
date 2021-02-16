using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeQueryBuilder
{
    public static class ExpressionParameters
    {
        private class Operation
        {
            public string Token { get; set; }
            public int Priority { get; set; }
            public Func<Expression, Expression, Expression> Expression { get; set; }
        }

        private static readonly ICollection<Operation> Operations;

        static ExpressionParameters()
        {
            Operations = new List<Operation>
            {
                new Operation {Token = "and", Priority = 1, Expression = Expression.And},
                new Operation {Token = "or", Priority = 1, Expression = Expression.Or},
                new Operation {Token = "eq", Priority = 2, Expression = Expression.Equal},
                new Operation {Token = "gt", Priority = 2, Expression = Expression.GreaterThan},
                new Operation {Token = "lt", Priority = 2, Expression = Expression.LessThan}
            };
        }

        public static Dictionary<string, int> Priorities =>
            Operations.ToDictionary(t => t.Token, t => t.Priority);

        public static Dictionary<string, Func<Expression, Expression, Expression>> Functions =>
            Operations.ToDictionary(t => t.Token, t => t.Expression);
    }
}