using SmartTutor.ProgressModel.Submissions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTutor.ContentModel.LearningObjects.ArrangeTasks
{
    [Table("ArrangeTasks")]
    public class ArrangeTask : LearningObject
    {
        public string Text { get; set; }
        public List<ArrangeTaskContainer> Containers { get; set; }

        internal List<ArrangeTaskContainerEvaluation> EvaluateSubmission(List<ArrangeTaskContainerSubmission> containers)
        {
            var evaluations = new List<ArrangeTaskContainerEvaluation>();
            foreach (var container in Containers)
            {
                var submittedContainer = containers.Find(c => c.ContainerId == container.Id);
                //TODO: If null throw exception since it is an invalid submission and see what the controller should return following best practices.
                if (submittedContainer == null) return null;

                evaluations.Add(new ArrangeTaskContainerEvaluation
                {
                    FullAnswer = container,
                    SubmissionWasCorrect = container.IsCorrectSubmission(submittedContainer.ElementIds)
                });
            }

            return evaluations;
        }
    }
}