using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.Lectures;
using System.Collections.Generic;

namespace SmartTutor.InstructorModel.Instructors
{
    public interface IInstructor
    {
        List<LearningObject> BuildNodeForLearner(int learnerId, KnowledgeNode knowledgeNode);
        List<LearningObject> BuildSimpleNode(KnowledgeNode knowledgeNode);
    }
}