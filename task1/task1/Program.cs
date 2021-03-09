using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace task1
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = new StreamReader("in.txt");
            var n = int.Parse(file.ReadLine());
            var edges = new Dictionary<int, List<int>>();
            for (var i = 1; i <= n; i++)
            {
                var input = file.ReadLine().Split();
                edges.Add(i, input.Select(x => int.Parse(x)).Where(x => x != 0).ToList());
            }
            Solve(n, edges);
        }

        static void Solve(int n, Dictionary<int, List<int>> edges)
        {
            var components = new List<List<int>>();
            var currentComponent = new List<int>();
            var visited = new HashSet<int>();
            var stack = new Stack<int>();
            var currentNode = 1;
            stack.Push(currentNode);
            visited.Add(currentNode);
            currentComponent.Add(currentNode);
            while (visited.Count != n)
            {
                if (stack.Count == 0)
                {
                    var nextNode = edges.Keys.First(x => !visited.Contains(x));
                    components.Add(currentComponent);
                    currentComponent = new List<int>();
                    stack.Push(nextNode);
                    visited.Add(nextNode);
                    currentComponent.Add(nextNode);
                }
                currentNode = stack.Pop();
                foreach (var node in edges[currentNode].Where(x => !visited.Contains(x)))
                {
                    visited.Add(node);
                    stack.Push(node);
                    currentComponent.Add(node);
                }
            }
            components.Add(currentComponent);
            var file = new StreamWriter("out.txt");
            file.WriteLine(components.Count);
            components.ForEach(x => file.WriteLine(string.Join(' ', x.OrderBy(x => x))));
            file.Flush();
        }
    }
}
