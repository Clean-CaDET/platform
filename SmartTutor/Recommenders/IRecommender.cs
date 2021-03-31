using SmartTutor.ContentModel.LearningObjects;
using System.Collections.Generic;

namespace SmartTutor.Recommenders
{
    public interface IRecommender
    {
        List<LearningObject> FindEducationalContent(List<SmellType> issues);
    }
}
