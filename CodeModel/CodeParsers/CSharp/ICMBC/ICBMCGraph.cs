using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;
using CodeModel.CodeParsers.CSharp.ICMBC;

namespace CodeModel.CodeParsers.CSharp
{
    public class ICBMCGraph
    {
        private int[,] MethodFieldAccessMapping { get; }
        internal List<Edge> EdgesInGraph { get; }
        internal List<SubGraphPair> SubGraphPairs { get; set; }

        public ICBMCGraph(CaDETClass parsedClass)
        {
            var normalMethods = parsedClass.Members.Where(m => m.IsMemberNormalMethod()).ToList();
            var fields = parsedClass.Fields;
            var fieldDefiningAccessors =
                parsedClass.Members.Where(m => m.IsFieldDefiningAccessor()).ToList();
            MethodFieldAccessMapping = InitializeMatrix(normalMethods, fields, fieldDefiningAccessors);
            EdgesInGraph = GetAllEdgesInGraph();
            SubGraphPairs = SubGraphPair.FindValidSubGraphPairs(EdgesInGraph, MethodFieldAccessMapping);
        }

        public ICBMCGraph(int[,] matrix)
        {
            MethodFieldAccessMapping = matrix.Clone() as int[,];
            EdgesInGraph = GetAllEdgesInGraph();
            SubGraphPairs = SubGraphPair.FindValidSubGraphPairs(EdgesInGraph, MethodFieldAccessMapping);
        }

        private int[,] InitializeMatrix(List<CaDETMember> normalMethods, List<CaDETField> fields,
            List<CaDETMember> fieldDefiningAccessors)
        {
            int[,] matrix = new int[normalMethods.Count, fields.Count + fieldDefiningAccessors.Count];
            for (var i = 0; i < normalMethods.Count; i++)
            {
                var accessedFields = normalMethods[i].GetDirectlyAndIndirectlyAccessedOwnFields();
                for (var j = 0; j < fields.Count; j++)
                    if (accessedFields.Contains(fields[j]))
                        matrix[i, j] = 1;
                    else
                        matrix[i, j] = 0;

                for (var j = 0; j < fieldDefiningAccessors.Count; j++)
                    if (normalMethods[i].AccessedAccessors.Contains(fieldDefiningAccessors[j]))
                        matrix[i, j + fields.Count] = 1;
                    else
                        matrix[i, j + fields.Count] = 0;
            }

            return matrix;
        }

        private List<Edge> GetAllEdgesInGraph()
        {
            var edges = new List<Edge>();
            for (var i = 0; i < MethodFieldAccessMapping.GetLength(0); i++)
            for (var j = 0; j < MethodFieldAccessMapping.GetLength(1); j++)
                if (MethodFieldAccessMapping[i, j] == 1)
                    edges.Add(new Edge(i, j));
            return edges;
        }

        /// <summary>
        /// Graph is disconnected if we can't find all edges in it
        /// by searching for them starting from some edge.
        /// </summary>
        public bool IsDisconnected()
        {
            if (EdgesInGraph.Count == 0) return true;
            Edge firstEdge = EdgesInGraph[0];
            var edges = FindEdgesStartingFromEdge(firstEdge);
            return edges.Count != EdgesInGraph.Count;
        }

        /// <summary>
        /// Start from given edge and search for edges in that edge's
        /// row or column. Returns all edges that it found.
        /// </summary>
        internal List<Edge> FindEdgesStartingFromEdge(Edge firstEdge)
        {
            List<int> visitedMethods = new List<int>();
            List<int> visitedFields = new List<int>();
            HashSet<Edge> collectedEdges = new HashSet<Edge>();
            HashSet<Edge> edgesToVisit = new HashSet<Edge>() {firstEdge};

            while (edgesToVisit.Count != 0)
            {
                HashSet<Edge> edgesForNextIteration = new HashSet<Edge>();
                foreach (var edge in edgesToVisit)
                {
                    if (!visitedMethods.Contains(edge.Method))
                    {
                        List<Edge> foundRowEdges = FindEdgesInDimension(0, edge.Method);
                        foundRowEdges.ForEach(e => edgesForNextIteration.Add(e));
                        visitedMethods.Add(edge.Method);
                    }

                    if (!visitedFields.Contains(edge.Field))
                    {
                        List<Edge> foundColumnEdges = FindEdgesInDimension(1, edge.Field);
                        foundColumnEdges.ForEach(e => edgesForNextIteration.Add(e));
                        visitedFields.Add(edge.Field);
                    }
                }

                foreach (var visitedEdge in edgesToVisit)
                {
                    collectedEdges.Add(visitedEdge);
                }

                edgesToVisit = edgesForNextIteration;
            }

            return collectedEdges.ToList();
        }

        /// <summary>
        /// Finds fields that method is using, and vice versa.
        /// </summary>
        private List<Edge> FindEdgesInDimension(int dimension, int lineInDimension)
        {
            List<Edge> foundEdges = new List<Edge>();

            foreach (var edge in EdgesInGraph)
            {
                if (dimension == 0 && edge.Method == lineInDimension)
                {
                    foundEdges.Add(edge);
                }
                else if (dimension == 1 && edge.Field == lineInDimension)
                {
                    foundEdges.Add(edge);
                }
            }

            return foundEdges;
        }

        public bool IsFullyConnected()
        {
            return EdgesInGraph.Count == MethodFieldAccessMapping.Length;
        }

        public int GetMaximumNumberOfConnections()
        {
            return MethodFieldAccessMapping.Length;
        }

        /// <summary>
        /// Process base cases since this is recursive function,
        /// then calculate cohesion for each subgraph pair and return maximum value.
        /// </summary>
        public double Calculate()
        {
            if (IsDisconnected())
                return 0;
            if (IsFullyConnected())
                return 1;
            var cohesionValues = new List<double>();
            foreach (var subGraphPair in SubGraphPairs)
                cohesionValues.Add(ICMBC(subGraphPair));
            return cohesionValues.Count == 0 ? 0 : cohesionValues.Max(); // TODO
        }

        /// <summary>
        /// ICBMC calculation formula. Rounds the result to 2 digits.
        /// </summary>
        private double ICMBC(SubGraphPair subGraphPair)
        {
            double cohesion = subGraphPair.GetNumberOfCutEdges() / GetMaximumNumberOfConnections() *
                   (subGraphPair.LeftSubGraph.Calculate() + subGraphPair.RightSubGraph.Calculate()) / 2;
            return Math.Round(cohesion, 2);
        }
    }
}