using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTutor.ContentModel
{
    public class EducationSnippet
    {
        public int SnippetQuaility { get; set; }
        public EducationType Snippet { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
