using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.Core.CommunityDetection.Model
{
    public class Graph
    {
        public List<string> Nodes { get; set; }
        public List<Link> Links { get; set; }
        public string Algorithm { get; set; }

        public Graph() { }

        public Graph(List<string> Nodes, List<Link> Links, string Algorithm)
        {
            this.Nodes = Nodes;
            this.Links = Links;
            this.Algorithm = Algorithm;
        }
    }
}
