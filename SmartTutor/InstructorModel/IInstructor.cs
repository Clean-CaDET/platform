using SmartTutor.ContentModel.Lectures;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.ProgressModel.Content;

namespace SmartTutor.InstructorModel
{
    public interface IInstructor
    {
        NodeProgress BuildNodeProgressForLearner(Learner learner, KnowledgeNode knowledgeNode);
    }
}