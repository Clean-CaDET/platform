using SmartTutor.ContentModel;
using System.Collections.Generic;

namespace SmartTutor.Service.Recommenders
{
    public interface IRecommender
    {
        List<EducationContent> FindEducationalContent(List<SmellType> issues);
    }
}
