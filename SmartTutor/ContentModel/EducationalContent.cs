using SmartTutor.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SmartTutor.ContentModel
{
    public class EducationalContent
    {
        public int Id { get; set; }
        public int ContentQuality { get; set; }
        public int ContentDifficulty { get; set; }
        public List<EducationalSnippet> EducationSnippets { get; set; }
        public List<int> EducationSnippetsIds { get; set; }
    }
}
