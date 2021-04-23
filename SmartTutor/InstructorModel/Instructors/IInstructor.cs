using SmartTutor.ContentModel.Lectures;
using SmartTutor.ProgressModel.Progress;

namespace SmartTutor.InstructorModel.Instructors
{
    public interface IInstructor
    {
        NodeProgress BuildNodeForLearner(int learnerId, KnowledgeNode knowledgeNode);
        NodeProgress BuildSimpleNode(KnowledgeNode knowledgeNode);
    }
}