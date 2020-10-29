using SmartTutor.ContentModel;

namespace SmartTutor.Repository
{
    public interface IContentRepository
    {
        /// <param name="smellType"> issue </param>
        /// <param name="indexOfContent"> Index of content in list of contents for some type of smell </param>
        /// <returns></returns>
        EducationContent FindEducationalContent(SmellType smellType, int indexOfContent);
    }
}


