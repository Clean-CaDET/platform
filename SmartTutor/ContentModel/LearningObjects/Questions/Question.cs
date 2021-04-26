using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTutor.ContentModel.LearningObjects.Questions
{
    [Table("Questions")]
    public class Question : LearningObject
    {
        public string Text { get; set; }
        public List<QuestionAnswer> PossibleAnswers { get; set; }

        public List<AnswerEvaluation> EvaluateAnswers(List<int> submittedAnswerIds)
        {
            var evaluations = new List<AnswerEvaluation>();
            foreach (var answer in PossibleAnswers)
            {
                var answerWasMarked = submittedAnswerIds.Contains(answer.Id);
                evaluations.Add(new AnswerEvaluation
                {
                    FullAnswer = answer,
                    SubmissionWasCorrect = answer.IsCorrect ? answerWasMarked : !answerWasMarked
                });
            }

            return evaluations;
        }
    }
}
