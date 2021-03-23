using System.Collections.Generic;
using SmartTutor.ContentModel.LectureModel;

namespace SmartTutor.Recommenders
{
    public interface IRecommender
    {
        List<LearningObject> FindEducationalContent(List<SmellType> issues);
    }
}
