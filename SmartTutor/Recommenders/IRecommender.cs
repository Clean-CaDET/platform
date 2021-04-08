using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LectureModel;
using SmartTutor.ContentModel.ProgressModel;
using System.Collections.Generic;

namespace SmartTutor.Recommenders
{
    public interface IRecommender
    {
        List<LearningObject> FindEducationalContent(List<SmellType> issues);
        NodeProgress BuildNodeProgressForTrainee(Trainee trainee, KnowledgeNode knowledgeNode);
    }
}