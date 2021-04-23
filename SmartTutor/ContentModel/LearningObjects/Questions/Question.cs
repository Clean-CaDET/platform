using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTutor.ContentModel.LearningObjects.Questions
{
    [Table("Questions")]
    public class Question : LearningObject
    {
        public string Text { get; set; }
        public List<QuestionAnswer> PossibleAnswers { get; set; }
    }
}
