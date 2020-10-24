using SmartTutor.ContentModel;

namespace SmartTutor.Repository
{
    public interface IContentRepository
    {
        EducationContent FindEducationalContent(SmellType smellType);
    }
}


