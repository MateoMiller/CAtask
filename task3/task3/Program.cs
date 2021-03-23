using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace task3
{
    class Edge
    {
        public int first;
        public int second;
        public int price;

        public Edge(int f, int s, int p)
        {
            first = f;
            second = s;
            price = p;
        }
    }

    class Path
    {
        public int price;
        public List<int> nodes;

        public Path(int price, List<int> nodes)
        {
            this.price = price;
            this.nodes = nodes;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var file = new StreamReader("in.txt");
            var n = int.Parse(file.ReadLine());
            var edges = new Dictionary<int, List<Edge>>();
            for (var i = 1; i <= n; i++)
            {
                var edges1 = new List<Edge>();
                edges.Add(i, edges1);
            }
            for (var i = 1; i <= n; i++)
            {
                var splitedLine = file.ReadLine().Split();
                var j = 0;
                while (j + 1 < splitedLine.Length)
                {
                    var endOfEdge = int.Parse(splitedLine[j]);
                    var price = int.Parse(splitedLine[j + 1]);
                    j += 2;
                    var edge = new Edge(endOfEdge, i, price);
                    edges[endOfEdge].Add(edge);
                }
            }
            var start = int.Parse(file.ReadLine());
            var finish = int.Parse(file.ReadLine());
            Solve(start, finish, edges);
        }

        static int GetNextNode(HashSet<int> targets, Dictionary<int, Path> paths)
        {
            var lowestNode = -1;
            foreach (var e in targets)
            {
                if (lowestNode == -1)
                {
                    lowestNode = e;
                }
                if (paths[e].price <= paths[lowestNode].price)
                {
                    lowestNode = e;
                }
            }
            return lowestNode;
        }

        static void Solve(int start, int finish, Dictionary<int, List<Edge>> edges)
        {
            var file = new StreamWriter("out.txt");
            if (start == finish)
            {
                file.WriteLine("Y");
                file.WriteLine(start);
                file.Write("0");
                file.Flush();
                return;
            }
            var targetsHashset = new HashSet<int>(edges.Keys);
            var paths = new Dictionary<int, Path>();
            foreach (var key in edges.Keys)
            {
                if (key != start)
                {
                    paths.Add(key, new Path(int.MaxValue, new List<int>() { key }));
                }
                else
                    paths.Add(start, new Path(1, new List<int>() { start }));
            }
            var currentNode = start;
            while (currentNode != finish)
            {
                currentNode = GetNextNode(targetsHashset, paths);
                if (paths[currentNode].price == int.MaxValue)
                {
                    file.Write("N");
                    file.Flush();
                    return;
                }

                if (currentNode == finish)
                {
                    file.WriteLine("Y");
                    file.WriteLine(string.Join(' ', paths[finish].nodes));
                    file.Write(paths[finish].price);
                    file.Flush();
                }

                foreach (var edge in edges[currentNode])
                {
                    var newPrice = paths[currentNode].price * edge.price;
                    if (newPrice <= paths[edge.second].price)
                    {
                        var newList = paths[currentNode].nodes.Append(edge.second).ToList();
                        paths[edge.second] = new Path(newPrice, newList);
                    }
                }
                targetsHashset.Remove(currentNode);
            }
        }
    }
}
