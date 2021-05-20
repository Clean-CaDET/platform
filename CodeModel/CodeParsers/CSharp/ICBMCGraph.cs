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
            var normalMethods = parsedClass.Members.Where(IsMemberNormalMethod).ToList();
            var fields = parsedClass.Fields;
            var fieldDefiningAccessors =
                parsedClass.Members.Where(m => m.IsFieldDefiningAccessor()).ToList();
            Matrix = InitializeMatrix(normalMethods, fields, fieldDefiningAccessors);
            EdgesInGraph = GetAllEdgesInGraph();
            SubGraphPairs = GetSubGraphPairs();
        }

        private ICBMCGraph(int[,] matrix)
        {
            Matrix = matrix.Clone() as int[,];
            EdgesInGraph = GetAllEdgesInGraph();
            SubGraphPairs = GetSubGraphPairs();
        }

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
                for (var j = 0; j < fields.Count; j++)
                    if (normalMethods[i].AccessedFields.Contains(fields[j]))
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

        private List<SubGraphPair> GetSubGraphPairs()
        {
            var cutEdgeGroupCandidates = GetCutEdgeGroupCandidates(); // TODO CutEdgeGroup
            var subGraphPairs = new List<SubGraphPair>();
            foreach (var edgeGroup in cutEdgeGroupCandidates)
            {
                var cutMatrix = RemoveEdgesFromMatrix(edgeGroup);
                if (CheckIfRowOrColumnIsEmpty(cutMatrix)) continue;

                // TODO strategy

                var cohesionGraph = new ICBMCGraph(cutMatrix);
                if (cohesionGraph.IsDisconnected())
                {
                    subGraphPairs.Add(new SubGraphPair(cohesionGraph, edgeGroup));
                }
            }

            return subGraphPairs;
        }

        // TODO
        public bool IsDisconnected()
        {
            if (EdgesInGraph.Count == 0) return true;
            Edge firstEdge = EdgesInGraph[0];
            var edges = StartSearchForEdges(new List<int>(), new List<int>(), new List<Edge> {firstEdge},
                new List<Edge> {firstEdge});
            return edges.Count != EdgesInGraph.Count;
        }

        private List<Edge> StartSearchForEdges(List<int> visitedRows, List<int> visitedColumns, List<Edge> visited,
            List<Edge> edgesToVisit)
        {
            var foundEdges = new List<Edge>();

            foreach (var edge in edgesToVisit)
            {
                if (!visitedRows.Contains(edge.Method))
                {
                    foundEdges.AddRange(SearchRowForConnections(visited, edge.Method));
                    visitedRows.Add(edge.Method);
                }
                else if (!visited.Contains(edge))
                {
                    visited.Add(edge);
                }

                if (!visitedColumns.Contains(edge.Field))
                {
                    foundEdges.AddRange(SearchColumnForConnections(visited, edge.Field));
                    visitedColumns.Add(edge.Field);
                }
                else if (!visited.Contains(edge))
                {
                    visited.Add(edge);
                }
            }

            return foundEdges.Count == 0
                ? visited
                : StartSearchForEdges(visitedRows, visitedColumns, visited, foundEdges);
        }

        private List<Edge> SarchForEdges(List<Edge> visited, List<Edge> edges)
        {
            List<int> visitedRows = new List<int>();
            List<int> visitedColumns = new List<int>();
            var foundEdges = new List<Edge>();
            foreach (var edge in edges)
            {
                if (!visitedRows.Contains(edge.Method))
                {
                    foundEdges.AddRange(SearchRowForConnections(visited, edge.Method));
                    visitedRows.Add(edge.Method);
                }

                if (!visitedColumns.Contains(edge.Field))
                {
                    foundEdges.AddRange(SearchColumnForConnections(visited, edge.Field));
                    visitedColumns.Add(edge.Field);
                }

                visited.Add(edge);
            }

            visited.AddRange(edges);

            return SarchForEdges(edges, foundEdges);
        }

        private List<Edge> SearchRowForConnections(List<Edge> edges, int row)
        {
            var newEdges = new List<Edge>();
            for (var j = 0; j < Matrix.GetLength(1); j++)
            {
                var temp = new Edge(row, j);
                if (!edges.Contains(temp) && Matrix[row, j] == 1)
                {
                    newEdges.Add(temp);
                }
            }

            return newEdges;
        }

        private List<Edge> SearchColumnForConnections(List<Edge> edges, int column)
        {
            var newEdges = new List<Edge>();
            for (var i = 0; i < Matrix.GetLength(1); i++)
            {
                var temp = new Edge(i, column);
                if (!edges.Contains(temp) && Matrix[i, column] == 1)
                {
                    newEdges.Add(temp);
                }
            }

            return newEdges;
        }

        public bool IsFullyConnected()
        {
            return EdgesInGraph.Count == Matrix.Length;
        }

        private IEnumerable<Edge[]> GetCutEdgeGroupCandidates()
        {
            var data = EdgesInGraph.ToArray();

            return Enumerable
                .Range(1, (1 << data.Length) - 2)
                .Select(index => data
                    .Where((v, i) => (index & (1 << i)) != 0)
                    .ToArray());
        }

        private bool CheckIfRowOrColumnIsEmpty(int[,] matrix)
        {
            if (CheckForEmptyLineInDimension(matrix, 0)) return true;
            if (CheckForEmptyLineInDimension(matrix, 1)) return true; // TODO if has only one dimension
            return false;
        }

        private bool CheckForEmptyLineInDimension(int[,] matrix, int dimensionToCheck)
        {
            int otherDimension = 1 == dimensionToCheck ? 0 : 1;
            for (int i = 0; i < matrix.GetLength(dimensionToCheck); i++)
            {
                bool isEmpty = true;
                for (int j = 0; j < matrix.GetLength(otherDimension); j++)
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

        private bool CheckForEmptyLineInColumn(int[,] matrix)
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
                return Equals(edge);
            }

            private bool Equals(Edge other)
            {
                return Method == other.Method && Field == other.Field;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Method, Field);
            }

            public Edge Copy()
            {
                return new Edge(this.Method, this.Field);
            }
        }

        internal class SubGraphPair
        {
            internal List<ICBMCGraph> SubGraphs { get; set; }
            internal Edge[] CutEdges { get; set; }

            public SubGraphPair(ICBMCGraph cohesionGraph, Edge[] edgeGroup)
            {
                CutEdges = new Edge[edgeGroup.Length];
                Array.Copy(edgeGroup, CutEdges, edgeGroup.Length);

                var edgesInSubGraph = cohesionGraph.StartSearchForEdges(new List<int>(), new List<int>(),
                    new List<Edge>() {cohesionGraph.EdgesInGraph[0]}, new List<Edge>() {cohesionGraph.EdgesInGraph[0]});
                var remainingEdges = cohesionGraph.EdgesInGraph.Where(e => !edgesInSubGraph.Contains(e)).ToList();
                var subGraph = CreateSubGraph(edgesInSubGraph);
                var subGraphFromRemainingEdges = CreateSubGraph(remainingEdges);
                SubGraphs = new List<ICBMCGraph>()
                    {new ICBMCGraph(subGraph), new ICBMCGraph(subGraphFromRemainingEdges)};
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