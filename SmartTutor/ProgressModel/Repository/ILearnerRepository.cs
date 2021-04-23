using SmartTutor.ProgressModel.Submissions;
using System.Collections.Generic;
using SmartTutor.LearnerModel.Learners;

namespace SmartTutor.ProgressModel.Repository
{
    public interface ILearnerRepository
    {
        /// <summary>
        /// Returns Nodes that have been completed or at least started.
        /// </summary>
        List<NodeProgress> GetActivatedNodes(int learnerId);
        void SaveNodeProgress(NodeProgress nodeProgress);
        NodeProgress GetNodeProgressForLearner(int learnerId, int nodeId);
        void SaveChallengeSubmission(ChallengeSubmission challengeSubmission);
        void SaveQuestionSubmission(QuestionSubmission submission);
        void SaveArrangeTaskSubmission(ArrangeTaskSubmission submission);

        Learner GetById(int learnerId);
        Learner GetByIndex(string index);
        void Save(Learner learner);
        void Update(Learner learner);
    }
}