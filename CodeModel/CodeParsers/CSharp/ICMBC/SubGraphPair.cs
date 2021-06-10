using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel.CodeParsers.CSharp.ICMBC
{
    class SubGraphPair
    {
        internal ICBMCGraph LeftSubGraph { get; set; }
        internal ICBMCGraph RightSubGraph { get; set; }
        internal Edge[] CutEdges { get; set; }

        public SubGraphPair(ICBMCGraph cohesionGraph, Edge[] edgeGroup)
        {
            CutEdges = new Edge[edgeGroup.Length];
            Array.Copy(edgeGroup, CutEdges, edgeGroup.Length);

            var edgesInSubGraph = cohesionGraph.FindEdgesStartingFromEdge(cohesionGraph.EdgesInGraph[0]);
            var remainingEdges = cohesionGraph.EdgesInGraph.Where(e => !edgesInSubGraph.Contains(e)).ToList();
            var leftSubGraphMatrix = CreateSubGraph(edgesInSubGraph);
            var rightSubGraphMatrix = CreateSubGraph(remainingEdges);
            LeftSubGraph = new ICBMCGraph(leftSubGraphMatrix);
            RightSubGraph = new ICBMCGraph(rightSubGraphMatrix);
        }

        private int[,] CreateSubGraph(List<Edge> edges)
        {
            var fieldsIndexMapping = GetFieldsIndexMapping(edges);
            var methodsIndexMapping = GetMethodsIndexMapping(edges);
            var subGraph = new int[methodsIndexMapping.Count, fieldsIndexMapping.Count];
            foreach (var edge in edges)
                subGraph[methodsIndexMapping[edge.Method], fieldsIndexMapping[edge.Field]] = 1;

            return subGraph;
        }

        private Dictionary<int, int> GetFieldsIndexMapping(List<Edge> edges)
        {
            var oldIndexToNewOne = new Dictionary<int, int>();
            var i = 0;
            foreach (var edge in edges)
            {
                if (oldIndexToNewOne.ContainsKey(edge.Field)) continue;
                oldIndexToNewOne[edge.Field] = i++;
            }

            return oldIndexToNewOne;
        }

        private Dictionary<int, int> GetMethodsIndexMapping(List<Edge> edges)
        {
            var oldIndexToNewOne = new Dictionary<int, int>();
            var i = 0;
            foreach (var edge in edges)
            {
                if (oldIndexToNewOne.ContainsKey(edge.Method)) continue;
                oldIndexToNewOne[edge.Method] = i++;
            }

            return oldIndexToNewOne;
        }

        /// <summary>
        /// Find all possible subgraphs that can be created from this graph
        /// by removing method-field combinations (edges).
        /// </summary>
        public static List<SubGraphPair> FindValidSubGraphPairs(List<Edge> edgesInGraph, int[,] methodFieldIndexMapping)
        {
            var cutEdgeGroupCandidates = GetCutEdgeGroupCandidates(edgesInGraph);
            var subGraphPairs = new List<SubGraphPair>();
            foreach (var edgeGroup in cutEdgeGroupCandidates)
            {
                var subGraphPair = CreateValidSubGraphPair(edgeGroup, methodFieldIndexMapping);
                if (subGraphPair == null) continue;
                subGraphPairs.Add(subGraphPair);
            }

            return subGraphPairs;
        }

        private static SubGraphPair CreateValidSubGraphPair(Edge[] edgeGroup, int[,] methodFieldIndexMapping)
        {
            var cutMatrix = RemoveEdgesFromMatrix(edgeGroup, methodFieldIndexMapping);
            if (CheckIfMethodsInvokeAtLeastOneField(cutMatrix)) return null;
            if (CheckIfFieldsAreInvokedByAtLeastOneMethod(cutMatrix)) return null;

            // TODO strategy

            var cohesionGraph = new ICBMCGraph(cutMatrix);
            if (!cohesionGraph.IsDisconnected()) return null;
            SubGraphPair subGraphPair = new SubGraphPair(cohesionGraph, edgeGroup);
            if (subGraphPair.LeftSubGraph.IsDisconnected() || subGraphPair.RightSubGraph.IsDisconnected())
                return null;
            return subGraphPair;
        }

        private static IEnumerable<Edge[]> GetCutEdgeGroupCandidates(List<Edge> edgesInGraph)
        {
            var data = edgesInGraph.ToArray();
            if (data.Length == 0) return new List<Edge[]>(); // TODO

            return Enumerable
                .Range(1, (1 << data.Length) - 2)
                .Select(index => data
                    .Where((v, i) => (index & (1 << i)) != 0)
                    .ToArray());
        }

        private static bool CheckIfMethodsInvokeAtLeastOneField(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                bool isEmpty = true;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] != 0)
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty) return true;
            }

            return false;
        }

        private static bool CheckIfFieldsAreInvokedByAtLeastOneMethod(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                bool isEmpty = true;
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (matrix[j, i] != 0)
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty) return true;
            }

            return false;
        }

        private static int[,] RemoveEdgesFromMatrix(IEnumerable<Edge> edges, int[,] methodFieldAccessMapping)
        {
            var cutMatrix = methodFieldAccessMapping.Clone() as int[,];
            foreach (var edge in edges) cutMatrix[edge.Method, edge.Field] = 0;

            return cutMatrix;
        }

        public double GetNumberOfCutEdges()
        {
            return CutEdges.Length;
        }

    }
}