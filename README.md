# C# Expression Tree Query Builder #

This is implementation of C# expression tree query builder for filtering and sorting the data.

### The Task ###

Let say you have an an collection of entities, like:
```
 public class Entity
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int AnotherNumber { get; set; }
        public bool Selected { get; set; }
        public Kind Kind { get; set; }
        public DateTime Time { get; set; }    
    } 
```
And you want to do filtering and sorting them by using simple string query language which is supporting the following operations:
- OR
- AND
- EQ (equals)
- NE (not equals)
- GT (greater than)
- LT (lower than)

Also language can use parenthesis for defining operations priority and use combinations of the available fields, for example:
```(date eq '2021-01-01') AND ((number gt 100) OR (AnotherNumber lt 200))```

For doing sort you can define sort property and sort direction, for example:
```date asc```
    
### Usage ###

1. Define the collection of the data:
```
var data = new List<Entity>();
data.Add(new Entity{new Entity {Id=  1, Number = 1, AnotherNumber = 1, Selected = true, Kind = Kind.Kind1, Time = DateTime.Now},})
...
```

2. Define filtering and sorting query:
```
  string exampleFilter = "(number eq 2) or ((anotherNumber eq 3) and (number eq 4)) and (Selected eq true) and (kind eq Kind1)";
  string exampleSort = "time desc";
```  

3. Filter and sort the data:
```
var filteredData = data
    .FilterCondition(exampleFilter)
    .SortCondition(exampleSort)
    .ToList();
```

### Example ###
1. Full example is provided in ```ExpressionTreeQueryBuilderTest``` project