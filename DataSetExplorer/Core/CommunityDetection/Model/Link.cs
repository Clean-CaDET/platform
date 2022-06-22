using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.Core.CommunityDetection.Model
{
    public class Link
    {
        public string Source { get; set; }
        public string Target { get; set; }

        public Link() { }

        public Link(string Source, string Target)
        {
            this.Source = Source;
            this.Target = Target;
        }
    }
}
