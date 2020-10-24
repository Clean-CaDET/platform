using SmartTutor.ContentModel;
using SmartTutor.Repository;
using SmartTutor.Service;

namespace SmartTutor.Controllers
{
    public class ContentRepositoryController
    {
        public ContentRepositoryService ContentRepositoryService;

        public ContentRepositoryController()
        {
            ContentRepositoryService = new ContentRepositoryService(new ContentInMemoryRepository());
        }

        public EducationContent FindLongParamListIssue()
        {
            return ContentRepositoryService.FindLongParamListIssue();
        }

        public EducationContent FindLongMethodIssue()
        {
            return ContentRepositoryService.FindLongMethodIssue();
        }
    }
}
