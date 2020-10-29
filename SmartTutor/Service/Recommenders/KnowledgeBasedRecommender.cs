using SmartTutor.ContentModel;
using SmartTutor.Repository;
using System.Collections.Generic;

namespace SmartTutor.Service.Recommenders
{
    class KnowledgeBasedRecommender : IRecommender
    {
        private IContentRepository contentRepository;
        public KnowledgeBasedRecommender(IContentRepository contentRepository)
        {
            this.contentRepository = contentRepository;
        }
        public List<EducationContent> FindEducationalContent(List<SmellType> issues)
        {
            List<EducationContent> result = new List<EducationContent>();
            foreach(SmellType smellType in issues)
            {
                 result.AddRange(contentRepository.FindEducationalContent(smellType));
            }
            return result;
        }
    }
}
