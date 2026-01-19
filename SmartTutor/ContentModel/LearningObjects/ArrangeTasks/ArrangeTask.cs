using SmartTutor.ProgressModel.Submissions;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.ArrangeTasks
{
    public class ArrangeTask : LearningObject
    {
        public string Text { get; private set; }
        public List<ArrangeTaskContainer> Containers { get; private set; }

        private ArrangeTask() {}
        public ArrangeTask(int id, int learningObjectSummaryId, string text, List<ArrangeTaskContainer> containers) : base(id, learningObjectSummaryId)
        {
            Text = text;
            Containers = containers;
        }

        internal List<ArrangeTaskContainerEvaluation> EvaluateSubmission(List<ArrangeTaskContainerSubmission> containers)
        {
            var evaluations = new List<ArrangeTaskContainerEvaluation>();
            foreach (var container in Containers)
            {
                var submittedContainer = containers.Find(c => c.ContainerId == container.Id);
                //TODO: If null throw exception since it is an invalid submission and see what the controller should return following best practices.
                if (submittedContainer == null) return null;

                evaluations.Add(new ArrangeTaskContainerEvaluation(container,
                    container.IsCorrectSubmission(submittedContainer.ElementIds)));
            }

            return evaluations;
        }
    }
}