using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.ProgressModel;

namespace SmartTutor.Recommenders
{
    public interface IRecommender
    {
        List<LearningObject> FindEducationalContent(List<SmellType> issues);
        NodeProgress BuildNodeProgressForTrainee(Trainee trainee, KnowledgeNode knowledgeNode);
    }
}