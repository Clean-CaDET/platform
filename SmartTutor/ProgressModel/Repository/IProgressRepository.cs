using SmartTutor.LearnerModel.Learners;
using SmartTutor.ProgressModel.Submissions;

namespace SmartTutor.ProgressModel.Repository
{
    public interface IProgressRepository
    {
        void SaveNodeProgress(NodeProgress nodeProgress);
        NodeProgress GetNodeProgressForLearner(int learnerId, int nodeId);
        void SaveChallengeSubmission(ChallengeSubmission challengeSubmission);
        void SaveQuestionSubmission(QuestionSubmission submission);
        void SaveArrangeTaskSubmission(ArrangeTaskSubmission submission);
    }
}