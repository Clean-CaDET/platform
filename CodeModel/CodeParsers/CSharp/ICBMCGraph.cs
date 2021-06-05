using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;

namespace CodeModel.CodeParsers.CSharp
{
    public class ICBMCGraph
    {
        private int[,] Matrix { get; }
        private List<Edge> EdgesInGraph { get; }
        internal List<SubGraphPair> SubGraphPairs { get; set; }

        public ICBMCGraph(CaDETClass parsedClass)
        {
            var normalMethods = parsedClass.Members.Where(m => IsMemberNormalMethod(m) && IsMemberPublic(m)).ToList();
            var fields = parsedClass.Fields;
            var fieldDefiningAccessors =
                parsedClass.Members.Where(m => m.IsFieldDefiningAccessor()).ToList();
            Matrix = InitializeMatrix(normalMethods, fields, fieldDefiningAccessors);
            EdgesInGraph = GetAllEdgesInGraph();
            SubGraphPairs = GetSubGraphPairs();
        }

        private static bool IsMemberPublic(CaDETMember m)
        {
            return m.Modifiers.Exists(md => md.Value == CaDETModifierValue.Public);
        }

        private ICBMCGraph(int[,] matrix)
        {
            Matrix = matrix.Clone() as int[,];
            EdgesInGraph = GetAllEdgesInGraph();
            SubGraphPairs = GetSubGraphPairs();
        }

        /// <summary>
        /// Member is normal method if it's neither constructor, nor getter or setter,
        /// nor delegate function.
        /// </summary>
        private bool IsMemberNormalMethod(CaDETMember m)
        {
            return m.Type == CaDETMemberType.Method &&
                   m.Metrics[CaDETMetric.MELOC] > 1; // && m.Modifiers.Contains(new CaDETModifier("public"));
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
            for (var i = 0; i < Matrix.GetLength(0); i++)
            for (var j = 0; j < Matrix.GetLength(1); j++)
                if (Matrix[i, j] == 1)
                    edges.Add(new Edge(i, j));
            return edges;
        }

        /// <summary>
        /// Find all possible subgraphs that can be created from this graph
        /// by removing method-field combinations (edges).
        /// </summary>
        private List<SubGraphPair> GetSubGraphPairs()
        {
            var cutEdgeGroupCandidates = GetCutEdgeGroupCandidates();
            var subGraphPairs = new List<SubGraphPair>();
            foreach (var edgeGroup in cutEdgeGroupCandidates)
            {
                var cutMatrix = RemoveEdgesFromMatrix(edgeGroup);
                if (CheckIfMethodsInvokeAtLeastOneField(cutMatrix)) continue;
                if (CheckIfFieldsAreInvokedByAtLeastOneMethod(cutMatrix)) continue;

                // TODO strategy

                var cohesionGraph = new ICBMCGraph(cutMatrix);
                if (cohesionGraph.IsDisconnected())
                {
                    SubGraphPair subGraphPair = new SubGraphPair(cohesionGraph, edgeGroup);
                    if (subGraphPair.LeftSubGraph.IsDisconnected() || subGraphPair.RightSubGraph.IsDisconnected())
                        continue;
                    subGraphPairs.Add(subGraphPair);
                }
            }

            return subGraphPairs;
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
        private List<Edge> FindEdgesStartingFromEdge(Edge firstEdge)
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
            return EdgesInGraph.Count == Matrix.Length;
        }

        private IEnumerable<Edge[]> GetCutEdgeGroupCandidates()
        {
            var data = EdgesInGraph.ToArray();
            if (data.Length == 0) return new List<Edge[]>(); // TODO

            return Enumerable
                .Range(1, (1 << data.Length) - 2)
                .Select(index => data
                    .Where((v, i) => (index & (1 << i)) != 0)
                    .ToArray());
        }

        private bool CheckIfMethodsInvokeAtLeastOneField(int[,] matrix)
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

        private bool CheckIfFieldsAreInvokedByAtLeastOneMethod(int[,] matrix)
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

        private int[,] RemoveEdgesFromMatrix(IEnumerable<Edge> edges)
        {
            var cutMatrix = Matrix.Clone() as int[,];
            foreach (var edge in edges) cutMatrix[edge.Method, edge.Field] = 0;

            return cutMatrix;
        }

        public int GetMaximumNumberOfConnections()
        {
            return Matrix.Length;
        }

        internal class Edge
        {
            public int Method { get; }
            public int Field { get; }

            public Edge(int method, int field)
            {
                Method = method;
                Field = field;
            }

            public override bool Equals(object? obj)
            {
                var edge = obj as Edge;
                if (edge == null) return false;
                return Method == edge.Method && Field == edge.Field;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Method, Field);
            }
        }

        internal class SubGraphPair
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
}