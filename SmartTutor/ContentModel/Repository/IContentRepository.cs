using System.Collections.Generic;
using SmartTutor.ContentModel.LectureModel;

namespace SmartTutor.ContentModel.Repository
{
    public interface IContentRepository
    {
        List<LearningObject> FindEducationalContent(SmellType smellType);
    }
}


