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

        public double GetNumberOfCutEdges()
        {
            return CutEdges.Length;
        }

    }
}