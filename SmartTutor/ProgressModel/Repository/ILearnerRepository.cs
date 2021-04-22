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
        List<NodeProgress> GetActivatedNodes(int traineeId);
        void SaveNodeProgress(NodeProgress nodeProgress);
        NodeProgress GetNodeProgressForTrainee(int traineeId, int nodeId);
        void SaveChallengeSubmission(ChallengeSubmission challengeSubmission);
        void SaveQuestionSubmission(QuestionSubmission submission);
        void SaveArrangeTaskSubmission(ArrangeTaskSubmission submission);

        Learner GetTraineeById(int traineeId);
        Learner GetLearnerByIndex(string index);
        void SaveTrainee(Learner learner);
        void UpdateTrainee(Learner learner);
    }
}