using System;
using System.Collections.Generic;
using System.Linq;
using static CodeModel.CodeParsers.CSharp.ICBMCGraph;

namespace CodeModel.CodeParsers.CSharp
{
    internal class ICBMCCalculator
    {

        /// <summary>
        /// Process base cases since this is recursive function,
        /// then calculate cohesion for each subgraph pair and return maximum value.
        /// </summary>
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

        /// <summary>
        /// ICBMC calculation formula. Rounds the result to 2 digits.
        /// </summary>
        private double ICMBC(ICBMCGraph graph, SubGraphPair subGraphPair)
        {
            double cohesion = subGraphPair.GetNumberOfCutEdges() / graph.GetMaximumNumberOfConnections() *
                   (Calculate(subGraphPair.LeftSubGraph) + Calculate(subGraphPair.RightSubGraph)) / 2;
            return Math.Round(cohesion, 2);
        }

    }
}