using SmartTutor.ContentModel;
using System.Collections.Generic;
using SmartTutor.Repository.CreateSnippet;

namespace SmartTutor.Repository
{
    public class ContentInMemoryRepository : IContentRepository
    {
        private readonly CreateLongParameterList _createLongParameterList = new CreateLongParameterList();
        private readonly CreateLongMethod _createLongMethod = new CreateLongMethod();

        public EducationContent FindEducationalContent(SmellType smellType)
        {
            // TODO: Use better design solution, to remove this if-else-else solution
            if (smellType == SmellType.LONG_METHOD) return GetLongMethod();
            if (smellType == SmellType.LONG_PARAM_LISTS) return GetLongParameterListContent();

            return null;
        }

        private EducationContent GetLongParameterListContent()
        {
            EducationContent educationContent = new EducationContent();

            educationContent.ContentQuality = 4;
            educationContent.ContentDifficulty = 4;
            educationContent.EducationSnippets = new List<EducationSnippet>
            {
                _createLongParameterList.ShortTextSnippet(),
                _createLongParameterList.LongTextSnippet(),
                _createLongParameterList.LongTexTwoSnippet(),
                _createLongParameterList.CodeSnippet(),
                _createLongParameterList.ImageSnippet()
            };

            return educationContent;
        }

        private EducationContent GetLongMethod()
        {
            EducationContent educationContent = new EducationContent();

            educationContent.ContentQuality = 4;
            educationContent.ContentDifficulty = 5;
            educationContent.EducationSnippets = new List<EducationSnippet>
            {
                _createLongMethod.ShortTextSnippet(),
                _createLongMethod.LongTextSnippet(),
                _createLongMethod.ShortTextTwoSnippet(),
                _createLongMethod.ImageSnippet(),
                _createLongMethod.VideoSnippet()
            };

            return educationContent;
        }
    }
}
