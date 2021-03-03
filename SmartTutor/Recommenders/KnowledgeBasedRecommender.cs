using SmartTutor.ContentModel;
using SmartTutor.Repository;
using System.Collections.Generic;

namespace SmartTutor.Recommenders
{
    internal class KnowledgeBasedRecommender : IRecommender
    {
        private readonly IContentRepository _contentRepository;

        public KnowledgeBasedRecommender(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public List<EducationalContent> FindEducationalContent(List<SmellType> issues)
        {
            var result = new List<EducationalContent>();
            issues.ForEach(smellType => result.AddRange(_contentRepository.FindEducationalContent(smellType)));
            return result;
        }
    }
}