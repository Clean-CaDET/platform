using SmartTutor.ContentModel;
using SmartTutor.Repository;


namespace SmartTutor.Service
{
    public class ContentRepositoryService
    {
        public IContentRepository ContentRepository;

        public ContentRepositoryService(IContentRepository contentRepository)
        {
            ContentRepository = contentRepository;
        }

        public EducationContent FindLongParamListIssue()
        {
            return ContentRepository.FindEducationalContent(SmellType.LONG_PARAM_LISTS);
        }

        public EducationContent FindLongMethodIssue()
        {
            return ContentRepository.FindEducationalContent(SmellType.LONG_METHOD);
        }
    }
}