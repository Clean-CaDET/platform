using SmartTutor.ContentModel;
using System.Collections.Generic;

namespace SmartTutor.Recommenders
{
    public interface IRecommender
    {
        List<EducationalContent> FindEducationalContent(List<SmellType> issues);
    }
}
