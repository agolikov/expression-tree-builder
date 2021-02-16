using System;
using System.Collections.Generic;
using System.Linq;
using ExpressionTreeQueryBuilder;

namespace ExpressionTreeQueryBuilderTest
{
    public class Program
    {
        public class Entity
        {
            public int Id { get; set; }
            public int Number { get; set; }
            public int AnotherNumber { get; set; }
            public bool Selected { get; set; }
            public Kind Kind { get; set; }
            public DateTime Time { get; set; }
        }

        public enum Kind
        {
            Kind1,
            Kind2,
            Kind3
        }

        static void Main(string[] args)
        {
            var data = new List<Entity>
            {
                new Entity {Id=  1, Number = 1, AnotherNumber = 1, Selected = true, Kind = Kind.Kind1, Time = DateTime.Now.AddDays(-1)},
                new Entity {Id = 2, Number = 2, AnotherNumber = 3, Selected = true, Kind = Kind.Kind1, Time = DateTime.Now.AddDays(-2)},
                new Entity {Id = 3, Number = 4, AnotherNumber = 3, Selected = true, Kind = Kind.Kind1, Time = DateTime.Now.AddDays(-3)},
                new Entity {Id = 4, Number = 3, AnotherNumber = 3, Selected = true, Kind = Kind.Kind3, Time = DateTime.Now.AddDays(-4)},
                new Entity {Id = 5, Number = 5, AnotherNumber = 1, Selected = true, Kind = Kind.Kind2, Time = DateTime.Now.AddDays(-5)}
            }.AsQueryable();

            string exampleFilter = "(number eq 2) or ((anotherNumber eq 3) and (number eq 4)) and (Selected eq true) and (kind eq Kind1)";
            string exampleSort = "time desc";

            var filteredData = data
                .FilterCondition(exampleFilter)
                .SortCondition(exampleSort)
                .ToList();

            foreach (var entity in filteredData)
            {
                Console.WriteLine($"{entity.Id} {entity.Kind} {entity.Number} {entity.AnotherNumber} {entity.Selected} {entity.Time}");
            }
        }
    }
}
