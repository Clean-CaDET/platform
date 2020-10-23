using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTutor.ContentModel
{
    public class EducationSnippet
    {
        public int SnippetQuality { get; set; }
        public int SnippetDifficulty { get; set; }   
        public EducationType EducationType { get; set; }
        public List<Tag> Tags { get; set; }
        public string Content { get; set; }
    }
}
