using SmartTutor.ContentModel;
using SmartTutor.Repository;
using System.Collections.Generic;

namespace SmartTutor.Recommenders
{
    class KnowledgeBasedRecommender : IRecommender
    {
        private IContentRepository contentRepository;
        public KnowledgeBasedRecommender(IContentRepository contentRepository)
        {
            this.contentRepository = contentRepository;
        }
        public List<EducationalContent> FindEducationalContent(List<SmellType> issues)
        {
            List<EducationalContent> result = new List<EducationalContent>();
            foreach(SmellType smellType in issues)
            {
                 result.AddRange(contentRepository.FindEducationalContent(smellType));
            }
            return result;
        }
    }
}
