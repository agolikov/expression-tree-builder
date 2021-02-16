using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionTreeQueryBuilder
{
    class ExpressionConverter
    {
        private HashSet<string> ops;
        private readonly Dictionary<string, int> _opsPriorities;

        public ExpressionConverter(Dictionary<string, int> operatorPriorities)
        {
            ops = operatorPriorities.Select(t => t.Key).ToHashSet();
            ops.Add("(");
            ops.Add(")");
            _opsPriorities = new Dictionary<string, int>(operatorPriorities);
        }

        private string[] GetTokens(string query)
        {
            query = query.ToLower();
            List<string> tokens = new List<string>();
            string tmp = "";
            int skip = 0;
            for (int i = 0; i < query.Length; ++i)
            {
                if (skip > 0)
                {
                    skip--;
                    continue;
                }

                bool isOperation = false;
                foreach (var op in ops)
                {
                    if (query.IndexOf(op, i) == i)
                    {
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            tokens.Add(tmp);
                            tmp = "";
                        }

                        tokens.Add(op);
                        isOperation = true;
                        skip = op.Length - 1;
                        break;
                    }
                }

                if (!isOperation)
                {
                    if (query[i] == ' ' && !string.IsNullOrEmpty(tmp))
                    {
                        tokens.Add(tmp);
                        tmp = "";
                    }

                    ;
                    if (query[i] != ' ') tmp += query[i];
                }
            }

            return tokens.ToArray();
        }

        public string ConvertInfixToPostfix(string infixQuery)
        {
            string[] tokens = GetTokens(infixQuery);

            Stack<string> s = new Stack<string>();
            List<string> outputList = new List<string>();
            int n;
            foreach (string c in tokens)
            {
                if (c == "(")
                {
                    s.Push(c);
                }
                else if (c == ")")
                {
                    while (s.Count != 0 && s.Peek() != "(")
                    {
                        outputList.Add(s.Pop());
                    }

                    s.Pop();
                }
                else if (IsOperator(c) == true)
                {
                    while (s.Count != 0 && Priority(s.Peek()) >= Priority(c))
                    {
                        outputList.Add(s.Pop());
                    }

                    s.Push(c);
                }
                else
                {
                    outputList.Add(c);
                }
            }

            while (s.Count != 0
            ) //if any operators remain in the stack, pop all & add to output list until stack is empty 
            {
                outputList.Add(s.Pop());
            }

            var sb = new StringBuilder();
            int i = 0;
            for (; i < outputList.Count - 1; i++)
            {
                sb.Append($"{outputList[i]} ");
            }

            sb.Append($"{outputList[i]}");

            return sb.ToString();
        }

        private int Priority(string c)
        {
            if (_opsPriorities.ContainsKey(c))
                return _opsPriorities[c];

            return 0;
        }

        private bool IsOperator(string c)
        {
            return ops.Contains(c);
        }
    }
}