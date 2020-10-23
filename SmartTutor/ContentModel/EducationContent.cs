using SmartTutor.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SmartTutor.ContentModel
{
    public class EducationContent
    {
        public int EducationQuality { get; set; }
        public int EducationDifficulty { get; set; }
        public List<EducationSnippet> EducationSnippets { get; set; }
    }
}
