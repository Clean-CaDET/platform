using System;
using System.Collections.Generic;
using System.Linq;
using static CodeModel.CodeParsers.CSharp.ICBMCGraph;

namespace CodeModel.CodeParsers.CSharp
{
    internal class ICBMCCalculator
    {

        public double Calculate(ICBMCGraph graph)
        {
            if (graph.IsDisconnected())
                return 0;
            if (graph.IsFullyConnected())
                return 1;
            var cohesionValues = new List<double>();
            foreach (var subGraphPair in graph.SubGraphPairs)
                cohesionValues.Add(ICMBC(graph, subGraphPair));
            return cohesionValues.Count == 0 ? 0 : cohesionValues.Max(); // TODO
        }

        private double ICMBC(ICBMCGraph graph, SubGraphPair subGraphPair)
        {
            double cohesion = subGraphPair.GetNumberOfCutEdges() / graph.GetMaximumNumberOfConnections() *
                   (Calculate(subGraphPair.SubGraphs[0]) + Calculate(subGraphPair.SubGraphs[1])) / 2;
            return Math.Round(cohesion, 2);
        }

    }
}