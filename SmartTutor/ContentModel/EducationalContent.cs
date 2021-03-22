using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel
{
    public class EducationalContent
    {
        [Key] public int Id { get; set; }
        public int Quality { get; set; }
        public int Difficulty { get; set; }
        public List<EducationalSnippet> EducationalSnippets { get; set; }
    }
}