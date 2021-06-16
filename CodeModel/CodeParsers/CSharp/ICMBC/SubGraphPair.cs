using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeModel.CodeParsers.CSharp.ICMBC
{
    class SubGraphPair
    {
        internal ICBMCGraph LeftSubGraph { get; set; }
        internal ICBMCGraph RightSubGraph { get; set; }
        internal HashSet<Edge> CutEdges { get; set; }

        public SubGraphPair(ICBMCGraph cohesionGraph, HashSet<Edge> edgeGroup)
        {
            CutEdges = edgeGroup.ToHashSet();

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
            var invalidEdgeGroups = GetInvalidEdgeGroups(edgesInGraph, methodFieldIndexMapping);
            var cutEdgeGroupCandidates = GetCutEdgeGroupCandidates(edgesInGraph, invalidEdgeGroups);
            var subGraphPairs = new List<SubGraphPair>();
            foreach (var edgeGroup in cutEdgeGroupCandidates)
            {
                if (subGraphPairs.Any(s => edgeGroup.IsProperSupersetOf(s.CutEdges))) continue;
                var subGraphPair = CreateValidSubGraphPair(edgeGroup, methodFieldIndexMapping);
                if (subGraphPair == null) continue;
                subGraphPairs.RemoveAll(s => s.CutEdges.IsProperSupersetOf(subGraphPair.CutEdges));
                subGraphPairs.Add(subGraphPair);
            }

            return subGraphPairs;
        }

        private static IEnumerable<HashSet<Edge>> GetInvalidEdgeGroups(List<Edge> edgesInGraph, int[,] methodFieldIndexMapping)
        {
            var retVal = new List<HashSet<Edge>>();
            int biggerDimension = methodFieldIndexMapping.GetLength(0) > methodFieldIndexMapping.GetLength(1)
                ? methodFieldIndexMapping.GetLength(0)
                : methodFieldIndexMapping.GetLength(1);
            for (int i = 0; i < biggerDimension; i++)
            {
                var edgesInARow = edgesInGraph.Where(e => e.Method == i).ToHashSet();
                if (edgesInARow.Count == 0) continue;
                retVal.Add(edgesInARow);
                var edgesInAColumn = edgesInGraph.Where(e => e.Field == i).ToHashSet();
                if (edgesInAColumn.Count == 0) continue;
                retVal.Add(edgesInAColumn);
            }

            return retVal;
        }

        private static SubGraphPair CreateValidSubGraphPair(HashSet<Edge> edgeGroup, int[,] methodFieldIndexMapping)
        {
            var cutMatrix = RemoveEdgesFromMatrix(edgeGroup, methodFieldIndexMapping);

            // TODO strategy

            var cohesionGraph = new ICBMCGraph(cutMatrix);
            if (!cohesionGraph.IsDisconnected()) return null;
            SubGraphPair subGraphPair = new SubGraphPair(cohesionGraph, edgeGroup);
            if (subGraphPair.LeftSubGraph.IsDisconnected() || subGraphPair.RightSubGraph.IsDisconnected())
                return null;
            return subGraphPair;
        }

        private static IEnumerable<HashSet<Edge>> GetCutEdgeGroupCandidates(List<Edge> edgesInGraph, IEnumerable<HashSet<Edge>> invalidEdgeGroups)
        {
            var data = edgesInGraph.ToArray();
            if (data.Length == 0) return new List<HashSet<Edge>>(); // TODO

            var cutEdgeCombinations = Enumerable
                .Range(1, (1 << data.Length) - 2)
                .Select(index => data
                    .Where((v, i) => (index & (1 << i)) != 0)
                    .ToHashSet());

            return cutEdgeCombinations.Where(edgeGroup =>
                !invalidEdgeGroups.Any(edgeGroup.IsSupersetOf)
            );
        }

        private static int[,] RemoveEdgesFromMatrix(IEnumerable<Edge> edges, int[,] methodFieldAccessMapping)
        {
            var cutMatrix = methodFieldAccessMapping.Clone() as int[,];
            foreach (var edge in edges) cutMatrix[edge.Method, edge.Field] = 0;

            return cutMatrix;
        }

        public double GetNumberOfCutEdges()
        {
            return CutEdges.Count;
        }
    }
}