using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace task2
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
                var nodes = new List<int>();
                for (var nodeId = 0; nodeId < n; nodeId += 1)
                {
                    if (input[nodeId] != "0")
                    {
                        nodes.Add(nodeId + 1);
                    }
                }
                edges.Add(i, nodes);
            }
            Solve(n, edges);
        }

        static void Solve(int n, Dictionary<int, List<int>> edges)
        {
            var prev = new Dictionary<int, int>();
            var queue = new Queue<int>();
            var currentNode = 1;
            queue.Enqueue(currentNode);
            prev.Add(currentNode, -1);
            while (prev.Keys.Count != n || queue.Count != 0)
            {
                if (queue.Count == 0)
                {
                    currentNode = edges.Keys.First(x => !prev.ContainsKey(x));
                    queue.Enqueue(currentNode);
                    prev.Add(currentNode, -1);
                }
                currentNode = queue.Dequeue();
                foreach (var nextNode in edges[currentNode])
                {
                    if (prev.ContainsKey(nextNode))
                    {
                        if (prev[currentNode] != nextNode)
                        {
                            FindCycle(nextNode, currentNode, prev);
                            return;
                        }
                    }
                    else
                    {
                        prev.Add(nextNode, currentNode);
                        queue.Enqueue(nextNode);
                    }
                }
            }
            var file = new StreamWriter("out.txt");
            file.WriteLine("A");
            file.Flush();
        }

        static void FindCycle(int firstPoint, int secondPoint, Dictionary<int, int> prev)
        {
            var nodes1 = GetNodesInPath(firstPoint, prev);
            var nodes2 = GetNodesInPath(secondPoint, prev);
            for (var i = 0; i < nodes1.Count; i++)
            {
                for (var j = 0; j < nodes2.Count; j++)
                {
                    if (nodes1[i] == nodes2[j])
                    {
                        var result = nodes1
                            .Take(i + 1)
                            .Concat(nodes2.Take(j))
                            .ToList();
                        var file = new StreamWriter("out.txt");
                        file.WriteLine("N");
                        file.WriteLine(string.Join(' ', result.OrderBy(x => x)));
                        file.Flush();
                        return;
                    }
                }
            }
        }

        static List<int> GetNodesInPath(int lastNode, Dictionary<int, int> prev)
        {
            var nodes = new List<int>();
            while (lastNode != -1)
            {
                nodes.Add(lastNode);
                lastNode = prev[lastNode];
            }
            return nodes;
        }
    }
}
