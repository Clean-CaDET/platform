using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.Recommenders
{
    public interface IRecommender
    {
        List<LearningObject> FindEducationalContent(List<SmellType> issues);
    }
}
