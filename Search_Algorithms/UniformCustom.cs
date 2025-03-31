using System.Security.Cryptography.X509Certificates;

namespace Search_Algorithms
{
    internal class UniformCustom
    {
        public readonly Dictionary<char, Dictionary<char, int>> graph = new() {
            ['A'] = new() { ['B'] = 2, ['C'] = 3},
            ['B'] = new() { ['A'] = 2, ['D'] = 3},
            ['C'] = new() { ['A'] = 3, ['D'] = 2, ['F'] = 1},
            ['D'] = new() { ['B'] = 3, ['C'] = 2, ['E'] = 5, ['F'] = 6},
            ['E'] = new() { ['D'] = 5, ['F'] = 3},
            ['F'] = new() { ['C'] = 1, ['D'] = 6, ['E'] = 3}
        };

        public void ExecuteSearch(char startNode)
        {
            if (!graph.ContainsKey(startNode))
            {
                Console.WriteLine($"The graph doesn`t contain the start node {startNode}");
                return;
            }

            foreach (var key in graph.Keys.Where(node => node != startNode))
            {
                ExecuteSearch(startNode, key);
            }
        }

        private void ExecuteSearch(char startNode, char objectiveNode)
        {
            var visiteds = new HashSet<char>();
            var border = new SortedSet<(char Node, int Cost)>(
                Comparer<(char Node, int Cost)>.Create((a,b) => 
                    a.Cost == b.Cost 
                        ? a.Node.CompareTo(b.Node) 
                        : a.Cost.CompareTo(b.Cost)));
            var nodeComeFrom = new Dictionary<char, char>();

            border.Add((startNode, 0));

            while (border.Count > 0)
            {
                var currentNode = border.Min;
                border.Remove(currentNode);

                if (currentNode.Node == objectiveNode)
                {
                    Console.WriteLine($"The path between the init node '{startNode}' and the node '{objectiveNode}' cost {currentNode.Cost}");
                    Console.WriteLine(GetPath(startNode, objectiveNode, nodeComeFrom) + "\n");

                    return;
                }

                visiteds.Add(currentNode.Node);

                foreach (char neighbor in graph[currentNode.Node].Keys)
                {
                    if (visiteds.Contains(neighbor))
                        continue;

                    int accumulatedCost = currentNode.Cost + graph[currentNode.Node][neighbor];
                    var newNodeCost = (neighbor, accumulatedCost);

                    if (!border.Contains(newNodeCost) 
                        || accumulatedCost < border.FirstOrDefault(nodeCost => nodeCost.Node == neighbor).Cost)
                    {
                        border.RemoveWhere(nodeCost => nodeCost.Node == neighbor);
                        border.Add(newNodeCost);
                        nodeComeFrom[neighbor] = currentNode.Node;
                    }
                }
            }
        }

        private string GetPath(char startNode, char objectiveNode, Dictionary<char, char> nodeComeFrom)
        {
            char current = objectiveNode;
            var path = new List<char>() { objectiveNode };

            while (current != startNode)
            {
                current = nodeComeFrom[current];
                path.Add(current);
            }

            path.Reverse();

            return string.Join(" - ", path);
        }
    }
}
