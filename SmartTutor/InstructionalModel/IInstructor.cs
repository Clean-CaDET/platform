using SmartTutor.ContentModel.Lectures;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.ProgressModel;

namespace SmartTutor.InstructionalModel
{
    public interface IInstructor
    {
        NodeProgress BuildNodeProgressForTrainee(Learner learner, KnowledgeNode knowledgeNode);
    }
}