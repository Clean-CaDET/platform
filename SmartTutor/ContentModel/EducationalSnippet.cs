using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel
{
    public class EducationalSnippet
    {
        [Key] public int Id { get; set; }
        public int Quality { get; set; }
        public int Difficulty { get; set; }
        public SnippetType Type { get; set; }
        public List<Tag> Tags { get; set; }
        public string Content { get; set; }
    }
}